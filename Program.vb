Option Strict On
Imports System.Windows.Forms

Public Module Program

    <STAThread>
    Public Sub Main()
        ' Comment the next two lines out for the true Windows 2000 grey look.
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        AddHandler Application.ThreadException, AddressOf OnUnhandledThreadException

        ' The database, documents and accounts file all live beside the executable, so if
        ' this folder is read only there is nothing sensible to do but say so.
        If Not Config.DataFolderIsWritable() Then
            MessageBox.Show(
                "Claims Manager keeps its database, documents and sign-in details in the " &
                "same folder as the program, and that folder cannot be written to:" &
                vbCrLf & vbCrLf & Config.DataFolder & vbCrLf & vbCrLf &
                "Copy the folder somewhere you can write to, your Desktop or Downloads for " &
                "instance, and run it from there.",
                "Claims Manager cannot start",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim splash As New frmSplash()
        splash.Show()
        splash.Refresh()
        Application.DoEvents()

        Try
            ' The first run builds the database and generates a year of demo claims,
            ' which is why the splash reports progress rather than just sitting there.
            DbInitializer.EnsureDatabase(AddressOf splash.ReportProgress)
        Catch ex As Exception
            splash.Close()
            splash.Dispose()
            MessageBox.Show(ex.Message, "Northbridge Assurance Claims Manager cannot start",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        splash.CloseWhenReady()
        splash.Dispose()

        Using signIn As New frmLogin()
            If signIn.ShowDialog() <> DialogResult.OK Then Return
        End Using

        Application.Run(New frmMain())
    End Sub

    ' ThreadExceptionEventArgs is in System.Threading, not System.Windows.Forms, so it is
    ' qualified here rather than adding an import that would make Timer ambiguous.
    Private Sub OnUnhandledThreadException(sender As Object,
                                           e As System.Threading.ThreadExceptionEventArgs)
        MessageBox.Show("Something went wrong and the action was not completed." & vbCrLf & vbCrLf &
                        e.Exception.Message,
                        "Claims Manager", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

End Module
