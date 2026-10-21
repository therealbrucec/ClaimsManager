Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Build.Framework

''' <summary>
''' Compiles the HTML Help project in this folder into ClaimsManager.chm.
'''
''' This is an MSBuild inline task, not part of the application: ClaimsManager.vbproj loads
''' it with RoslynCodeTaskFactory and runs it after every build. It exists because Microsoft
''' no longer distributes HTML Help Workshop, so hhc.exe cannot be assumed on a build
''' machine. ClaimsManager.hhp is still an ordinary Workshop project, and hhc.exe will
''' compile it too if you have a copy.
'''
''' What it writes is a standard ITSF/ITSS container, the format hh.exe has read since 1997:
''' a directory, a stored section holding the storage metadata, and an "MSCompressed"
''' section holding the topics. That section is LZX in name only. LZX allows a block to be
''' stored uncompressed, so every 32K frame is written as a stored block rather than this
''' file carrying an LZX compressor. The help file is larger than hhc would make it, which
''' at this size does not matter.
'''
''' Deliberately left out: the full-text search index ($FIftiMain), so the viewer shows
''' Contents and Index tabs but no Search tab, and the binary TOC and index, which only
''' matter for very large or merged help files. The viewer reads contents.hhc and
''' index.hhk directly instead.
'''
''' The build fails on a link to a file that is not in the project, which is the warning
''' hhc.exe printed and everyone ignored.
''' </summary>
Public Class BuildChm
    Inherits Microsoft.Build.Utilities.Task

    <Required>
    Public Property ProjectFile As String

    <Required>
    Public Property OutputFile As String

    Public Overrides Function Execute() As Boolean
        Try
            Dim project As HelpProject = HelpProject.Load(ProjectFile)
            Dim files As SortedDictionary(Of String, Byte()) = project.CollectFiles()

            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(OutputFile)))
            Dim baseName As String = Path.GetFileNameWithoutExtension(OutputFile)
            File.WriteAllBytes(OutputFile, ChmWriter.Write(project, baseName, files))

            Log.LogMessage(MessageImportance.High, "  {0} -> {1} ({2} files)",
                           Path.GetFileName(ProjectFile), OutputFile, files.Count)
            Return True
        Catch ex As HelpBuildException
            Log.LogError(Nothing, "HELP", Nothing, ex.SourceFile, 0, 0, 0, 0, ex.Message)
            Return False
        End Try
    End Function

End Class

Friend Class HelpBuildException
    Inherits Exception

    Public ReadOnly Property SourceFile As String

    Public Sub New(sourceFile As String, message As String)
        MyBase.New(message)
        Me.SourceFile = sourceFile
    End Sub
End Class

''' <summary>The parts of an HTML Help Workshop .hhp file that this compiler acts on.</summary>
Friend Class HelpProject

    Public Property ProjectPath As String = String.Empty
    Public Property Folder As String = String.Empty
    Public ReadOnly Property Options As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
    Public ReadOnly Property ListedFiles As New List(Of String)()
    Public ReadOnly Property Windows As New List(Of HelpWindow)()

    ' Anything a topic, a stylesheet or a sitemap can point at.
    Private Shared ReadOnly LinkPattern As New Regex(
        "(?:\b(?:href|src)\s*=\s*[""']([^""']+)[""'])" &
        "|(?:<param\s+name\s*=\s*[""']Local[""']\s+value\s*=\s*[""']([^""']+)[""'])" &
        "|(?:\burl\(\s*[""']?([^""')]+)[""']?\s*\))",
        RegexOptions.IgnoreCase)

    Public Shared Function Load(projectPath As String) As HelpProject
        If Not File.Exists(projectPath) Then
            Throw New HelpBuildException(projectPath, "The help project was not found.")
        End If

        Dim project As New HelpProject With {
            .ProjectPath = Path.GetFullPath(projectPath),
            .Folder = Path.GetDirectoryName(Path.GetFullPath(projectPath))
        }

        Dim section As String = String.Empty
        For Each rawLine As String In File.ReadAllLines(projectPath)
            Dim line As String = rawLine.Trim()
            If line.Length = 0 OrElse line.StartsWith(";") Then Continue For

            If line.StartsWith("[") AndAlso line.EndsWith("]") Then
                section = line.Trim("["c, "]"c).ToUpperInvariant()
                Continue For
            End If

            ' Inline tasks compile without the VB runtime, so strings are compared with
            ' String.Equals rather than = or Select Case.
            If String.Equals(section, "OPTIONS") Then
                Dim equals As Integer = line.IndexOf("="c)
                If equals > 0 Then
                    project.Options(line.Substring(0, equals).Trim()) = line.Substring(equals + 1).Trim()
                End If
            ElseIf String.Equals(section, "FILES") Then
                project.ListedFiles.Add(line)
            ElseIf String.Equals(section, "WINDOWS") Then
                project.Windows.Add(HelpWindow.Parse(projectPath, line))
            End If
        Next

        Return project
    End Function

    ''' <summary>The window the viewer opens when the caller names none.</summary>
    Public Function DefaultWindow() As HelpWindow
        Dim wanted As String = OptionOrEmpty("Default Window")
        For Each window As HelpWindow In Windows
            If wanted.Length = 0 OrElse String.Equals(window.Name, wanted, StringComparison.OrdinalIgnoreCase) Then
                Return window
            End If
        Next
        If wanted.Length > 0 Then
            Throw New HelpBuildException(ProjectPath, "Default Window names " & wanted &
                                         ", which is not defined under [WINDOWS].")
        End If
        Return Nothing
    End Function

    Public Function OptionOrEmpty(name As String) As String
        Dim value As String = Nothing
        If Options.TryGetValue(name, value) Then Return value
        Return String.Empty
    End Function

    ''' <summary>The language id from "Language=0x809 English (United Kingdom)".</summary>
    Public Function LanguageId() As Integer
        Dim match As Match = Regex.Match(OptionOrEmpty("Language"), "0x([0-9A-Fa-f]+)")
        If Not match.Success Then Return &H409
        Return Convert.ToInt32(match.Groups(1).Value, 16)
    End Function

    ''' <summary>
    ''' Every file the help needs, keyed by its path inside the .chm. Starts from the
    ''' [FILES] list, the contents and the index, then follows links the way hhc.exe did,
    ''' so stylesheets and images are picked up without being listed.
    ''' </summary>
    Public Function CollectFiles() As SortedDictionary(Of String, Byte())
        Dim found As New SortedDictionary(Of String, Byte())(StringComparer.OrdinalIgnoreCase)
        Dim pending As New Queue(Of String)()

        Dim roots As New List(Of String)(ListedFiles)
        For Each key As String In {"Contents file", "Index file", "Default topic"}
            If OptionOrEmpty(key).Length > 0 Then roots.Add(OptionOrEmpty(key))
        Next
        For Each window As HelpWindow In Windows
            roots.AddRange(window.Files().Where(Function(f) f.Length > 0))
        Next

        For Each root As String In roots
            pending.Enqueue(Resolve(ProjectPath, root))
        Next

        Do While pending.Count > 0
            Dim fullPath As String = pending.Dequeue()
            Dim internalName As String = InternalNameFor(fullPath)
            If found.ContainsKey(internalName) Then Continue Do

            Dim content As Byte() = File.ReadAllBytes(fullPath)
            found.Add(internalName, content)

            Dim extension As String = Path.GetExtension(fullPath).ToLowerInvariant()
            If String.Equals(extension, ".hhc") OrElse String.Equals(extension, ".hhk") Then
                RequireAscii(fullPath, content)
            End If

            If {".htm", ".html", ".hhc", ".hhk", ".css"}.Contains(extension) Then
                Dim text As String = Encoding.UTF8.GetString(content)
                For Each match As Match In LinkPattern.Matches(text)
                    Dim target As String = FirstGroup(match)
                    If IsExternal(target) Then Continue For
                    pending.Enqueue(Resolve(fullPath, target))
                Next
            End If
        Loop

        Return found
    End Function

    Private Shared Function FirstGroup(match As Match) As String
        For i As Integer = 1 To match.Groups.Count - 1
            If match.Groups(i).Success Then Return match.Groups(i).Value.Trim()
        Next
        Return String.Empty
    End Function

    Private Shared Function IsExternal(target As String) As Boolean
        If target.Length = 0 OrElse target.StartsWith("#") Then Return True
        Return Regex.IsMatch(target, "^[a-z][a-z0-9+.\-]*:", RegexOptions.IgnoreCase)
    End Function

    ''' <summary>Resolves a link relative to the file it appears in, and insists it exists.</summary>
    Private Function Resolve(referringFile As String, target As String) As String
        Dim cut As Integer = target.IndexOfAny({"#"c, "?"c})
        If cut >= 0 Then target = target.Substring(0, cut)

        Dim fullPath As String = Path.GetFullPath(
            Path.Combine(Path.GetDirectoryName(referringFile), target.Replace("/"c, "\"c)))

        If Not fullPath.StartsWith(Folder & "\", StringComparison.OrdinalIgnoreCase) Then
            Throw New HelpBuildException(referringFile, "Links to " & target &
                                         ", which is outside the help project folder.")
        End If
        If Not File.Exists(fullPath) Then
            Throw New HelpBuildException(referringFile, "Links to " & target &
                                         ", which does not exist.")
        End If
        Return fullPath
    End Function

    Private Function InternalNameFor(fullPath As String) As String
        Return "/" & fullPath.Substring(Folder.Length + 1).Replace("\"c, "/"c)
    End Function

    ''' <summary>The viewer parses sitemaps in the system code page, so they stay ASCII and
    ''' use HTML entities for anything else.</summary>
    Private Shared Sub RequireAscii(fullPath As String, content As Byte())
        For i As Integer = 0 To content.Length - 1
            If content(i) > 127 Then
                Throw New HelpBuildException(fullPath, "Contains a non-ASCII character at byte " &
                                             i.ToString() & ". Use an HTML entity instead.")
            End If
        Next
    End Sub

End Class

''' <summary>
''' One line of [WINDOWS], in Workshop's own layout:
''' name="Caption","toc","index","default","home",jump1 url,jump1 text,jump2 url,jump2 text,
''' properties,navigation width,buttons,[left,top,right,bottom],...
''' Fields after the window position are not used here.
''' </summary>
Friend Class HelpWindow

    Public Property Name As String = String.Empty
    Public Property Caption As String = String.Empty
    Public Property ContentsFile As String = String.Empty
    Public Property IndexFile As String = String.Empty
    Public Property DefaultFile As String = String.Empty
    Public Property HomeFile As String = String.Empty
    Public Property Properties As Integer
    Public Property NavigationWidth As Integer
    Public Property Buttons As Integer
    Public Property Position As Integer() = {0, 0, 0, 0}

    Public Function Files() As String()
        Return {ContentsFile, IndexFile, DefaultFile, HomeFile}
    End Function

    Public Shared Function Parse(projectPath As String, line As String) As HelpWindow
        Dim equals As Integer = line.IndexOf("="c)
        If equals <= 0 Then Throw New HelpBuildException(projectPath, "Cannot read window: " & line)

        ' Split on commas that are not inside quotes or the [ ] of the position.
        Dim fields As New List(Of String)()
        Dim current As New StringBuilder()
        Dim inQuotes As Boolean = False
        Dim inBrackets As Boolean = False
        For Each c As Char In line.Substring(equals + 1)
            If c = """"c Then
                inQuotes = Not inQuotes
            ElseIf c = "["c AndAlso Not inQuotes Then
                inBrackets = True
            ElseIf c = "]"c AndAlso Not inQuotes Then
                inBrackets = False
            ElseIf c = ","c AndAlso Not inQuotes AndAlso Not inBrackets Then
                fields.Add(current.ToString().Trim())
                current.Clear()
                Continue For
            End If
            If c <> """"c Then current.Append(c)
        Next
        fields.Add(current.ToString().Trim())
        Do While fields.Count < 13
            fields.Add(String.Empty)
        Loop

        Dim window As New HelpWindow With {
            .Name = line.Substring(0, equals).Trim(),
            .Caption = fields(0),
            .ContentsFile = fields(1),
            .IndexFile = fields(2),
            .DefaultFile = fields(3),
            .HomeFile = fields(4),
            .Properties = Number(fields(9)),
            .NavigationWidth = Number(fields(10)),
            .Buttons = Number(fields(11))
        }

        Dim corners As String() = fields(12).Trim("["c, "]"c).Split(","c)
        If corners.Length = 4 Then window.Position = corners.Select(Function(n) Number(n)).ToArray()
        Return window
    End Function

    Private Shared Function Number(text As String) As Integer
        text = text.Trim()
        If text.Length = 0 Then Return 0
        If text.StartsWith("0x", StringComparison.OrdinalIgnoreCase) Then
            Return Convert.ToInt32(text.Substring(2), 16)
        End If
        Return Integer.Parse(text)
    End Function

End Class

''' <summary>Lays out the ITSF container. Offsets and constants follow what hhc.exe writes.</summary>
Friend NotInheritable Class ChmWriter

    Private Const DirectoryChunkSize As Integer = &H1000
    Private Const QuickRefInterval As Integer = 5          ' 1 + (1 << density), density 2
    Private Const LzxFrameSize As Integer = &H8000
    Private Const ResetIntervalFrames As Integer = 2       ' matches ControlData below

    Private Const TransformGuid As String = "{7FC28940-9D31-11D0-9B27-00A0C91E9C7C}"
    Private Const StoragePrefix As String = "::DataSpace/Storage/MSCompressed/"
    Private Const InstancePrefix As String = StoragePrefix & "Transform/" & TransformGuid & "/InstanceData/"

    Private Class DirectoryEntry
        Public Name As String = String.Empty
        Public Section As Integer
        Public Offset As Long
        Public Length As Long
    End Class

    Private Sub New()
    End Sub

    Public Shared Function Write(project As HelpProject, baseName As String,
                                 files As SortedDictionary(Of String, Byte())) As Byte()
        Dim latest As Date = New Date(2003, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        For Each name As String In files.Keys
            Dim written As Date = File.GetLastWriteTimeUtc(
                Path.Combine(project.Folder, name.TrimStart("/"c).Replace("/"c, "\"c)))
            If written > latest Then latest = written
        Next

        Dim entries As New List(Of DirectoryEntry)()
        entries.Add(New DirectoryEntry With {.Name = "/"})

        ' Section 1: #SYSTEM and every project file, end to end, then wrapped as LZX.
        Dim contents As New List(Of KeyValuePair(Of String, Byte()))()
        contents.Add(New KeyValuePair(Of String, Byte())("/#SYSTEM", SystemFile(project, baseName, latest)))
        If project.Windows.Count > 0 Then
            Dim strings As New StringTable()
            contents.Add(New KeyValuePair(Of String, Byte())("/#WINDOWS", WindowsFile(project.Windows, strings)))
            contents.Add(New KeyValuePair(Of String, Byte())("/#STRINGS", strings.ToArray()))
        End If
        contents.AddRange(files)

        Dim folders As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Dim raw As New MemoryStream()
        For Each item As KeyValuePair(Of String, Byte()) In contents
            entries.Add(New DirectoryEntry With {
                .Name = item.Key, .Section = 1, .Offset = raw.Position, .Length = item.Value.Length})
            raw.Write(item.Value, 0, item.Value.Length)

            Dim slash As Integer = item.Key.IndexOf("/"c, 1)
            Do While slash > 0
                folders.Add(item.Key.Substring(0, slash + 1))
                slash = item.Key.IndexOf("/"c, slash + 1)
            Loop
        Next
        For Each folder As String In folders
            entries.Add(New DirectoryEntry With {.Name = folder})
        Next

        Dim frameOffsets As New List(Of Long)()
        Dim compressed As Byte() = StoreAsLzx(raw.ToArray(), frameOffsets)

        ' Section 0: the storage metadata the reader needs to find and unwrap section 1.
        Dim section0 As New MemoryStream()
        AddStored(entries, section0, "::DataSpace/NameList", NameList())
        AddStored(entries, section0, StoragePrefix & "Transform/List", TransformList())
        AddStored(entries, section0, StoragePrefix & "SpanInfo", UInt64Bytes(CULng(raw.Length)))
        AddStored(entries, section0, StoragePrefix & "ControlData", ControlData())
        AddStored(entries, section0, InstancePrefix & "ResetTable",
                  ResetTable(raw.Length, compressed.Length, frameOffsets))
        AddStored(entries, section0, StoragePrefix & "Content", compressed)
        entries.Add(New DirectoryEntry With {.Name = InstancePrefix})

        Dim chunks As List(Of Byte()) = DirectoryChunks(entries)
        Dim listingChunkCount As Integer = If(chunks.Count = 1, 1, chunks.Count - 1)

        Const HeaderLength As Integer = &H60
        Const HeaderSection0Length As Integer = &H18
        Const DirectoryHeaderLength As Integer = &H54

        Dim directoryOffset As Long = HeaderLength + HeaderSection0Length
        Dim directoryLength As Long = DirectoryHeaderLength + CLng(chunks.Count) * DirectoryChunkSize
        Dim dataOffset As Long = directoryOffset + directoryLength
        Dim fileLength As Long = dataOffset + section0.Length

        Dim output As New MemoryStream()
        Dim w As New BinaryWriter(output)

        ' ITSF header
        w.Write(Encoding.ASCII.GetBytes("ITSF"))
        w.Write(3)
        w.Write(HeaderLength)
        w.Write(1)
        w.Write(Timestamp(latest))
        w.Write(project.LanguageId())
        w.Write(New Guid("7C01FD10-7BAA-11D0-9E0C-00A0C922E6EC").ToByteArray())
        w.Write(New Guid("7C01FD11-7BAA-11D0-9E0C-00A0C922E6EC").ToByteArray())
        w.Write(CLng(HeaderLength))
        w.Write(CLng(HeaderSection0Length))
        w.Write(directoryOffset)
        w.Write(directoryLength)
        w.Write(dataOffset)

        ' Header section 0
        w.Write(&H1FE)
        w.Write(0)
        w.Write(fileLength)
        w.Write(0)
        w.Write(0)

        ' ITSP directory header
        w.Write(Encoding.ASCII.GetBytes("ITSP"))
        w.Write(1)
        w.Write(DirectoryHeaderLength)
        w.Write(&HA)
        w.Write(DirectoryChunkSize)
        w.Write(2)                                           ' quickref density
        w.Write(If(chunks.Count = 1, 1, 2))                  ' depth of the index tree
        w.Write(If(chunks.Count = 1, -1, chunks.Count - 1))  ' root index chunk
        w.Write(0)                                           ' first listing chunk
        w.Write(listingChunkCount - 1)                       ' last listing chunk
        w.Write(-1)
        w.Write(chunks.Count)
        w.Write(project.LanguageId())
        w.Write(New Guid("5D02926A-212E-11D0-9DF9-00A0C922E6EC").ToByteArray())
        w.Write(DirectoryHeaderLength)
        w.Write(-1)
        w.Write(-1)
        w.Write(-1)

        For Each chunk As Byte() In chunks
            w.Write(chunk)
        Next
        w.Write(section0.ToArray())
        w.Flush()

        Return output.ToArray()
    End Function

    Private Shared Sub AddStored(entries As List(Of DirectoryEntry), section0 As MemoryStream,
                                 name As String, content As Byte())
        entries.Add(New DirectoryEntry With {
            .Name = name, .Section = 0, .Offset = section0.Position, .Length = content.Length})
        section0.Write(content, 0, content.Length)
    End Sub

#Region "Internal files"

    ''' <summary>The project options as the viewer reads them: titles, start page, sitemaps.</summary>
    Private Shared Function SystemFile(project As HelpProject, baseName As String, latest As Date) As Byte()
        Dim ms As New MemoryStream()
        Dim w As New BinaryWriter(ms)
        w.Write(3)

        Dim settings As New MemoryStream()
        Dim s As New BinaryWriter(settings)
        s.Write(project.LanguageId())
        s.Write(0)                       ' DBCS
        s.Write(0)                       ' full-text search: no
        s.Write(0)                       ' keyword links
        s.Write(0)                       ' associative links
        s.Write(latest.ToFileTimeUtc())
        s.Write(0)
        s.Write(0)
        s.Flush()

        SystemEntry(w, 10, BitConverter.GetBytes(Timestamp(latest)))
        SystemEntry(w, 9, AnsiZ("HHA Version 4.74.8702"))
        SystemEntry(w, 4, settings.ToArray())
        SystemEntry(w, 0, AnsiZ(project.OptionOrEmpty("Contents file")))
        SystemEntry(w, 1, AnsiZ(project.OptionOrEmpty("Index file")))
        SystemEntry(w, 2, AnsiZ(project.OptionOrEmpty("Default topic")))
        SystemEntry(w, 3, AnsiZ(project.OptionOrEmpty("Title")))
        SystemEntry(w, 6, AnsiZ(baseName))
        If project.OptionOrEmpty("Default Font").Length > 0 Then
            SystemEntry(w, 16, AnsiZ(project.OptionOrEmpty("Default Font")))
        End If
        Dim window As HelpWindow = project.DefaultWindow()
        If window IsNot Nothing Then SystemEntry(w, 5, AnsiZ(window.Name))

        w.Flush()
        Return ms.ToArray()
    End Function

    ''' <summary>
    ''' Window definitions, laid out as the viewer's HH_WINTYPE structure with every string
    ''' replaced by its offset in #STRINGS. Entries are the structure's own size, 0xC4: the
    ''' viewer refuses to open the file at all when given the longer 0x188 entries that
    ''' later versions of hhc.exe wrote, presumably because those carry fields this does not.
    ''' </summary>
    Private Shared Function WindowsFile(windows As List(Of HelpWindow), strings As StringTable) As Byte()
        Const EntrySize As Integer = &HC4
        Const ValidProperties As Integer = &H2
        Const ValidPosition As Integer = &H10
        Const ValidNavigationWidth As Integer = &H20
        Const ValidButtons As Integer = &H100

        Dim ms As New MemoryStream()
        Dim w As New BinaryWriter(ms)
        w.Write(windows.Count)
        w.Write(EntrySize)

        For Each window As HelpWindow In windows
            Dim valid As Integer = ValidProperties Or ValidButtons
            If window.Position.Any(Function(n) n <> 0) Then valid = valid Or ValidPosition
            If window.NavigationWidth > 0 Then valid = valid Or ValidNavigationWidth

            Dim entry(EntrySize - 1) As Byte
            PutInt(entry, &H0, EntrySize)
            PutInt(entry, &H4, 0)                                   ' strings are ANSI
            PutInt(entry, &H8, strings.Add(window.Name))
            PutInt(entry, &HC, valid)
            PutInt(entry, &H10, window.Properties)
            PutInt(entry, &H14, strings.Add(window.Caption))
            For i As Integer = 0 To 3
                PutInt(entry, &H20 + 4 * i, window.Position(i))
            Next
            PutInt(entry, &H4C, window.NavigationWidth)
            PutInt(entry, &H60, strings.Add(window.ContentsFile))
            PutInt(entry, &H64, strings.Add(window.IndexFile))
            PutInt(entry, &H68, strings.Add(window.DefaultFile))
            PutInt(entry, &H6C, strings.Add(window.HomeFile))
            PutInt(entry, &H70, window.Buttons)
            w.Write(entry)
        Next

        w.Flush()
        Return ms.ToArray()
    End Function

    Private Shared Sub PutInt(target As Byte(), offset As Integer, value As Integer)
        Array.Copy(BitConverter.GetBytes(value), 0, target, offset, 4)
    End Sub

    ''' <summary>#STRINGS: null-terminated ANSI strings, referred to by offset. Offset 0 is
    ''' the empty string.</summary>
    Private Class StringTable
        Private ReadOnly _bytes As New MemoryStream()
        Private ReadOnly _offsets As New Dictionary(Of String, Integer)(StringComparer.Ordinal)

        Public Sub New()
            _bytes.WriteByte(0)
        End Sub

        Public Function Add(text As String) As Integer
            If String.IsNullOrEmpty(text) Then Return 0
            Dim offset As Integer
            If _offsets.TryGetValue(text, offset) Then Return offset

            offset = CInt(_bytes.Position)
            Dim encoded As Byte() = AnsiZ(text)
            _bytes.Write(encoded, 0, encoded.Length)
            _offsets.Add(text, offset)
            Return offset
        End Function

        Public Function ToArray() As Byte()
            Return _bytes.ToArray()
        End Function
    End Class

    Private Shared Sub SystemEntry(w As BinaryWriter, code As Short, data As Byte())
        w.Write(code)
        w.Write(CShort(data.Length))
        w.Write(data)
    End Sub

    Private Shared Function AnsiZ(text As String) As Byte()
        Return Concat(Encoding.GetEncoding(1252).GetBytes(text), {CByte(0)})
    End Function

    Private Shared Function NameList() As Byte()
        Dim ms As New MemoryStream()
        Dim w As New BinaryWriter(ms)
        Dim names As String() = {"Uncompressed", "MSCompressed"}
        w.Write(CShort(2 + names.Sum(Function(n) n.Length + 2)))   ' length in 16-bit words
        w.Write(CShort(names.Length))
        For Each name As String In names
            w.Write(CShort(name.Length))
            w.Write(Encoding.Unicode.GetBytes(name))
            w.Write(CShort(0))
        Next
        w.Flush()
        Return ms.ToArray()
    End Function

    ''' <summary>hhc.exe writes the transform GUID with a length of 38 bytes, which is half
    ''' of it. Readers only look at the start, and this matches what they are used to.</summary>
    Private Shared Function TransformList() As Byte()
        Return Encoding.Unicode.GetBytes(TransformGuid).Take(38).ToArray()
    End Function

    Private Shared Function ControlData() As Byte()
        Dim ms As New MemoryStream()
        Dim w As New BinaryWriter(ms)
        w.Write(6)                                        ' dwords that follow
        w.Write(Encoding.ASCII.GetBytes("LZXC"))
        w.Write(2)                                        ' version: sizes below are in 32K units
        w.Write(ResetIntervalFrames)                      ' reset interval
        w.Write(2)                                        ' window size, 64K
        w.Write(1)                                        ' cache size
        w.Write(0)
        w.Flush()
        Return ms.ToArray()
    End Function

    Private Shared Function ResetTable(uncompressedLength As Long, compressedLength As Long,
                                       frameOffsets As List(Of Long)) As Byte()
        Dim ms As New MemoryStream()
        Dim w As New BinaryWriter(ms)
        w.Write(2)
        w.Write(frameOffsets.Count)
        w.Write(8)
        w.Write(&H28)
        w.Write(uncompressedLength)
        w.Write(compressedLength)
        w.Write(CLng(LzxFrameSize))
        For Each offset As Long In frameOffsets
            w.Write(offset)
        Next
        w.Flush()
        Return ms.ToArray()
    End Function

    ''' <summary>
    ''' One stored LZX block per 32K frame. A block header is three bits of type (3, stored)
    ''' and 24 of length, preceded at each reset point by one bit saying there is no E8
    ''' call translation. The bitstream is then padded to a 16-bit boundary and followed by
    ''' the three repeated-offset registers and the bytes themselves, padded to even length.
    ''' </summary>
    Private Shared Function StoreAsLzx(raw As Byte(), frameOffsets As List(Of Long)) As Byte()
        Dim output As New MemoryStream()
        Dim position As Integer = 0
        Dim frame As Integer = 0

        Do While position < raw.Length
            frameOffsets.Add(output.Position)
            Dim size As Integer = Math.Min(LzxFrameSize, raw.Length - position)

            Dim bits As New LzxBitWriter(output)
            If frame Mod ResetIntervalFrames = 0 Then bits.Write(0, 1)
            bits.Write(3, 3)
            bits.Write(LzxFrameSize, 24)
            bits.Flush()

            For register As Integer = 1 To 3
                output.Write(BitConverter.GetBytes(1), 0, 4)
            Next
            output.Write(raw, position, size)

            ' Readers decode whole frames and stop at the length in the reset table, so
            ' the last frame is padded out rather than declared short.
            If size < LzxFrameSize Then output.Write(New Byte(LzxFrameSize - size - 1) {}, 0, LzxFrameSize - size)

            position += size
            frame += 1
        Loop

        Return output.ToArray()
    End Function

    Private Class LzxBitWriter
        Private ReadOnly _output As Stream
        Private _word As Integer
        Private _used As Integer

        Public Sub New(output As Stream)
            _output = output
        End Sub

        ''' <summary>Bits fill 16-bit little-endian words from the most significant end.</summary>
        Public Sub Write(value As Integer, count As Integer)
            For i As Integer = count - 1 To 0 Step -1
                _word = (_word << 1) Or ((value >> i) And 1)
                _used += 1
                If _used = 16 Then EmitWord()
            Next
        End Sub

        Public Sub Flush()
            If _used = 0 Then Return
            _word <<= (16 - _used)
            EmitWord()
        End Sub

        Private Sub EmitWord()
            _output.WriteByte(CByte(_word And &HFF))
            _output.WriteByte(CByte((_word >> 8) And &HFF))
            _word = 0
            _used = 0
        End Sub
    End Class

#End Region

#Region "Directory"

    ''' <summary>
    ''' Listing (PMGL) chunks in name order, plus one index (PMGI) chunk when more than one
    ''' listing chunk is needed. Readers search the directory case-insensitively, so it is
    ''' sorted that way.
    ''' </summary>
    Private Shared Function DirectoryChunks(entries As List(Of DirectoryEntry)) As List(Of Byte())
        Dim sorted As List(Of DirectoryEntry) =
            entries.OrderBy(Function(e) e.Name.ToLowerInvariant(), StringComparer.Ordinal).ToList()

        Dim listings As New List(Of List(Of Byte()))()
        Dim firstNames As New List(Of String)()
        Dim current As New List(Of Byte())()

        For Each entry As DirectoryEntry In sorted
            Dim encoded As Byte() = Concat(EncodedName(entry.Name), EncInt(entry.Section),
                                           EncInt(entry.Offset), EncInt(entry.Length))
            If Not Fits(current, encoded, &H14) Then
                listings.Add(current)
                current = New List(Of Byte())()
            End If
            If current.Count = 0 Then firstNames.Add(entry.Name)
            current.Add(encoded)
        Next
        listings.Add(current)

        Dim chunks As New List(Of Byte())()
        For i As Integer = 0 To listings.Count - 1
            Dim header As New MemoryStream()
            Dim w As New BinaryWriter(header)
            w.Write(Encoding.ASCII.GetBytes("PMGL"))
            w.Write(DirectoryChunkSize - &H14 - listings(i).Sum(Function(e) e.Length))
            w.Write(0)
            w.Write(If(i = 0, -1, i - 1))
            w.Write(If(i = listings.Count - 1, -1, i + 1))
            w.Flush()
            chunks.Add(Chunk(header.ToArray(), listings(i)))
        Next

        If listings.Count > 1 Then
            Dim index As New List(Of Byte())()
            For i As Integer = 0 To firstNames.Count - 1
                Dim encoded As Byte() = Concat(EncodedName(firstNames(i)), EncInt(i))
                If Not Fits(index, encoded, 8) Then
                    Throw New HelpBuildException(Nothing, "The help project has too many files " &
                                                 "for a single directory index chunk.")
                End If
                index.Add(encoded)
            Next

            Dim header As New MemoryStream()
            Dim w As New BinaryWriter(header)
            w.Write(Encoding.ASCII.GetBytes("PMGI"))
            w.Write(DirectoryChunkSize - 8 - index.Sum(Function(e) e.Length))
            w.Flush()
            chunks.Add(Chunk(header.ToArray(), index))
        End If

        Return chunks
    End Function

    ''' <summary>Room for the entry and for the quickref table growing back from the end.</summary>
    Private Shared Function Fits(existing As List(Of Byte()), encoded As Byte(), headerLength As Integer) As Boolean
        Dim count As Integer = existing.Count + 1
        Dim quickRefs As Integer = (count - 1) \ QuickRefInterval
        Dim needed As Integer = headerLength + existing.Sum(Function(e) e.Length) + encoded.Length +
                                2 + 2 * quickRefs
        Return needed <= DirectoryChunkSize
    End Function

    ''' <summary>
    ''' Entries from the front, and at the back the entry count followed (backwards) by the
    ''' offset of every fifth entry, measured from the end of the chunk header.
    ''' </summary>
    Private Shared Function Chunk(header As Byte(), encodedEntries As List(Of Byte())) As Byte()
        Dim block(DirectoryChunkSize - 1) As Byte
        Array.Copy(header, block, header.Length)

        Dim position As Integer = header.Length
        For i As Integer = 0 To encodedEntries.Count - 1
            If i > 0 AndAlso i Mod QuickRefInterval = 0 Then
                Dim slot As Integer = DirectoryChunkSize - 2 - 2 * (i \ QuickRefInterval)
                Array.Copy(BitConverter.GetBytes(CUShort(position - header.Length)), 0, block, slot, 2)
            End If
            Array.Copy(encodedEntries(i), 0, block, position, encodedEntries(i).Length)
            position += encodedEntries(i).Length
        Next

        Array.Copy(BitConverter.GetBytes(CUShort(encodedEntries.Count)), 0, block, DirectoryChunkSize - 2, 2)
        Return block
    End Function

    Private Shared Function EncodedName(name As String) As Byte()
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(name)
        Return Concat(EncInt(bytes.Length), bytes)
    End Function

    ''' <summary>Seven bits per byte, most significant first, high bit set on all but the last.</summary>
    Private Shared Function EncInt(value As Long) As Byte()
        Dim groups As New List(Of Byte)()
        groups.Add(CByte(value And &H7F))
        value >>= 7
        Do While value > 0
            groups.Insert(0, CByte((value And &H7F) Or &H80))
            value >>= 7
        Loop
        Return groups.ToArray()
    End Function

#End Region

#Region "Helpers"

    Private Shared Function Concat(ParamArray parts As Byte()()) As Byte()
        Return parts.SelectMany(Function(p) p).ToArray()
    End Function

    Private Shared Function UInt64Bytes(value As ULong) As Byte()
        Return BitConverter.GetBytes(value)
    End Function

    Private Shared Function Timestamp(value As Date) As Integer
        Dim seconds As Long = CLng((value - New Date(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds)
        Return CInt(seconds And &H7FFFFFFFL)
    End Function

#End Region

End Class
