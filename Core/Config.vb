Option Strict On
Imports System.IO

''' <summary>
''' File paths and the Jet connection string.
'''
''' Everything the application writes - the database, the document store and the demo
''' accounts file - lives in the same folder as ClaimsManager.exe. Nothing is written to
''' the user profile, so a demo is one folder that can be copied, inspected, zipped or
''' thrown away whole.
'''
''' That folder therefore has to be writable. It will not be if the application has been
''' installed under Program Files and is run by a standard user, so the installer grants
''' write access to the install folder. If a copy is run from somewhere read only anyway,
''' DataFolderIsWritable lets start-up say so plainly instead of failing halfway through
''' creating a database.
''' </summary>
Public Module Config

    ''' <summary>The folder the running executable sits in, and everything else with it.</summary>
    Public ReadOnly Property DataFolder As String
        Get
            Return AppDomain.CurrentDomain.BaseDirectory.TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
        End Get
    End Property

    Public ReadOnly Property DatabasePath As String
        Get
            Return Path.Combine(DataFolder, "Claims.mdb")
        End Get
    End Property

    ''' <summary>Root of the document store. Files are kept on disk, not in the .mdb,
    ''' because Jet databases bloat badly once you push photographs into them.</summary>
    Public ReadOnly Property DocumentRoot As String
        Get
            Return Path.Combine(DataFolder, "Documents")
        End Get
    End Property

    ''' <summary>The compiled HTML Help file, built from Help\ and installed beside the
    ''' executable. Read-only, so it is the one file here that needs no write access.</summary>
    Public ReadOnly Property HelpFilePath As String
        Get
            Return Path.Combine(DataFolder, "ClaimsManager.chm")
        End Get
    End Property

    Public ReadOnly Property ConnectionString As String
        Get
            Return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & DatabasePath & ";"
        End Get
    End Property

    ''' <summary>Demo convenience: the sign-in details and each role's permissions,
    ''' written next to the database on first run.</summary>
    Public ReadOnly Property AccountsFilePath As String
        Get
            Return Path.Combine(DataFolder, "demo-accounts.csv")
        End Get
    End Property

    Public Sub EnsureFolders()
        Directory.CreateDirectory(DataFolder)
        Directory.CreateDirectory(DocumentRoot)
    End Sub

    ''' <summary>
    ''' Settles the question by actually writing something, rather than by reading ACLs and
    ''' hoping the interpretation is right.
    ''' </summary>
    Public Function DataFolderIsWritable() As Boolean
        Try
            Directory.CreateDirectory(DataFolder)

            Dim probe As String = Path.Combine(
                DataFolder, "write-probe-" & Guid.NewGuid().ToString("N") & ".tmp")
            File.WriteAllText(probe, "probe")
            File.Delete(probe)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

End Module
