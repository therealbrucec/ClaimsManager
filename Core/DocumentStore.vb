Option Strict On
Imports System.IO

''' <summary>
''' Attachments are copied into a Documents folder beside the executable, and the database
''' stores a path relative to that root, so the whole folder can be moved or backed up.
''' </summary>
Public Module DocumentStore

    ''' <summary>Copies a file into the store for a claim and returns its relative path.</summary>
    Public Function Import(sourceFile As String, claimFolder As String) As String
        If Not File.Exists(sourceFile) Then
            Throw New FileNotFoundException("The file is no longer at " & sourceFile & ".")
        End If

        Dim folderName As String = SafeFolderName(claimFolder)
        Dim targetFolder As String = Path.Combine(Config.DocumentRoot, folderName)
        Directory.CreateDirectory(targetFolder)

        Dim fileName As String = UniqueName(targetFolder, Path.GetFileName(sourceFile))
        File.Copy(sourceFile, Path.Combine(targetFolder, fileName))

        Return Path.Combine(folderName, fileName)
    End Function

    ''' <summary>Moves documents captured before a claim existed into the claim's folder.</summary>
    Public Function Relocate(relativePath As String, newClaimFolder As String) As String
        Dim current As String = FullPath(relativePath)
        If Not File.Exists(current) Then Return relativePath

        Dim folderName As String = SafeFolderName(newClaimFolder)
        Dim targetFolder As String = Path.Combine(Config.DocumentRoot, folderName)
        Directory.CreateDirectory(targetFolder)

        Dim fileName As String = UniqueName(targetFolder, Path.GetFileName(current))
        Dim target As String = Path.Combine(targetFolder, fileName)
        File.Move(current, target)

        Return Path.Combine(folderName, fileName)
    End Function

    Public Function FullPath(relativePath As String) As String
        If String.IsNullOrWhiteSpace(relativePath) Then Return String.Empty
        Return Path.Combine(Config.DocumentRoot, relativePath)
    End Function

    Public Sub OpenDocument(relativePath As String)
        Dim target As String = FullPath(relativePath)
        If Not File.Exists(target) Then
            Throw New FileNotFoundException("This document is missing from the store: " & target)
        End If
        Process.Start(New ProcessStartInfo(target) With {.UseShellExecute = True})
    End Sub

    Public Function SizeOf(sourceFile As String) As Long
        Dim info As New FileInfo(sourceFile)
        If Not info.Exists Then Return 0L
        Return info.Length
    End Function

    Private Function SafeFolderName(name As String) As String
        Dim cleaned As String = If(String.IsNullOrWhiteSpace(name), "Unfiled", name.Trim())
        For Each bad As Char In Path.GetInvalidFileNameChars()
            cleaned = cleaned.Replace(bad, "-"c)
        Next
        Return cleaned
    End Function

    ''' <summary>Two people photograph damage and both files are called IMG_0001.jpg.</summary>
    Private Function UniqueName(folder As String, fileName As String) As String
        Dim candidate As String = fileName
        Dim baseName As String = Path.GetFileNameWithoutExtension(fileName)
        Dim ext As String = Path.GetExtension(fileName)
        Dim counter As Integer = 1

        While File.Exists(Path.Combine(folder, candidate))
            candidate = baseName & " (" & counter.ToString() & ")" & ext
            counter += 1
        End While

        Return candidate
    End Function

End Module
