Option Strict On
Imports System.Windows.Forms

''' <summary>
''' The user name is typed rather than picked, so the account list is not on display.
''' The role attached to whichever account signs in is what drives every permission in
''' the workflow. The password is a single shared constant for the whole demo build,
''' see Session.DemoPassword.
''' </summary>
Public Class frmLogin

    Public Sub New()
        InitializeComponent()
        UiStyle.ApplyTo(Me)
    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblDatabase.Text = Config.DatabasePath

        Try
            If UserRepository.GetActive().Count = 0 Then
                UiStyle.ShowProblem(Me, "No users are set up in the database. " &
                                        "Delete Claims.mdb and restart to rebuild it.")
                btnSignIn.Enabled = False
            End If
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
            btnSignIn.Enabled = False
        End Try

        If btnSignIn.Enabled Then txtUserName.Focus()
    End Sub

    Private Sub btnSignIn_Click(sender As Object, e As EventArgs) Handles btnSignIn.Click
        Dim entered As String = txtUserName.Text.Trim()

        If entered.Length = 0 Then
            UiStyle.ShowProblem(Me, "Enter the user name you are signing in with.")
            txtUserName.Focus()
            Return
        End If

        Dim account As AppUser
        Try
            account = UserRepository.FindByUserName(entered)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
            Return
        End Try

        If account Is Nothing Then
            UiStyle.ShowProblem(Me, "There is no account called " & entered & ". " &
                                    "The user names are listed in demo-accounts.csv, " &
                                    "beside the database.")
            txtUserName.SelectAll()
            txtUserName.Focus()
            Return
        End If

        If Not account.IsActive Then
            UiStyle.ShowProblem(Me, "That account is no longer active.")
            Return
        End If

        If Not Session.PasswordIsCorrect(txtPassword.Text) Then
            UiStyle.ShowProblem(Me, "That password is not right. Every account on this " &
                                    "demo build uses the same one.")
            txtPassword.SelectAll()
            txtPassword.Focus()
            Return
        End If

        Session.CurrentUser = account
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnQuit_Click(sender As Object, e As EventArgs) Handles btnQuit.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

End Class
