Option Strict On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.grpSignIn = New System.Windows.Forms.GroupBox()
        Me.lblPrompt = New System.Windows.Forms.Label()
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblHint = New System.Windows.Forms.Label()
        Me.btnSignIn = New System.Windows.Forms.Button()
        Me.btnQuit = New System.Windows.Forms.Button()
        Me.lblDatabase = New System.Windows.Forms.Label()
        Me.grpSignIn.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpSignIn
        '
        Me.grpSignIn.Controls.Add(Me.lblPrompt)
        Me.grpSignIn.Controls.Add(Me.txtUserName)
        Me.grpSignIn.Controls.Add(Me.lblPassword)
        Me.grpSignIn.Controls.Add(Me.txtPassword)
        Me.grpSignIn.Location = New System.Drawing.Point(12, 12)
        Me.grpSignIn.Name = "grpSignIn"
        Me.grpSignIn.Size = New System.Drawing.Size(372, 96)
        Me.grpSignIn.TabIndex = 0
        Me.grpSignIn.TabStop = False
        Me.grpSignIn.Text = "Claims handling desk"
        '
        'lblPrompt
        '
        Me.lblPrompt.AutoSize = True
        Me.lblPrompt.Location = New System.Drawing.Point(12, 28)
        Me.lblPrompt.Name = "lblPrompt"
        Me.lblPrompt.Size = New System.Drawing.Size(58, 13)
        Me.lblPrompt.TabIndex = 0
        Me.lblPrompt.Text = "&User name"
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(76, 25)
        Me.txtUserName.MaxLength = 50
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(280, 20)
        Me.txtUserName.TabIndex = 1
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(12, 60)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(53, 13)
        Me.lblPassword.TabIndex = 2
        Me.lblPassword.Text = "&Password"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(76, 57)
        Me.txtPassword.MaxLength = 50
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(280, 20)
        Me.txtPassword.TabIndex = 3
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'lblHint
        '
        Me.lblHint.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblHint.Location = New System.Drawing.Point(12, 114)
        Me.lblHint.Name = "lblHint"
        Me.lblHint.Size = New System.Drawing.Size(372, 30)
        Me.lblHint.TabIndex = 1
        Me.lblHint.Text = "Demo build: the sign-in details are listed in demo-accounts.csv, in the same folder as the database."
        '
        'btnSignIn
        '
        Me.btnSignIn.Location = New System.Drawing.Point(228, 150)
        Me.btnSignIn.Name = "btnSignIn"
        Me.btnSignIn.Size = New System.Drawing.Size(75, 23)
        Me.btnSignIn.TabIndex = 2
        Me.btnSignIn.Text = "Sign in"
        '
        'btnQuit
        '
        Me.btnQuit.Location = New System.Drawing.Point(309, 150)
        Me.btnQuit.Name = "btnQuit"
        Me.btnQuit.Size = New System.Drawing.Size(75, 23)
        Me.btnQuit.TabIndex = 3
        Me.btnQuit.Text = "Quit"
        '
        'lblDatabase
        '
        Me.lblDatabase.AutoEllipsis = True
        Me.lblDatabase.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblDatabase.Location = New System.Drawing.Point(12, 152)
        Me.lblDatabase.Name = "lblDatabase"
        Me.lblDatabase.Size = New System.Drawing.Size(210, 26)
        Me.lblDatabase.TabIndex = 4
        Me.lblDatabase.Text = "Database"
        '
        'frmLogin
        '
        Me.AcceptButton = Me.btnSignIn
        Me.CancelButton = Me.btnQuit
        Me.ClientSize = New System.Drawing.Size(396, 188)
        Me.Controls.Add(Me.grpSignIn)
        Me.Controls.Add(Me.lblHint)
        Me.Controls.Add(Me.lblDatabase)
        Me.Controls.Add(Me.btnSignIn)
        Me.Controls.Add(Me.btnQuit)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLogin"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Northbridge Assurance - Sign in"
        Me.grpSignIn.ResumeLayout(False)
        Me.grpSignIn.PerformLayout()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents grpSignIn As System.Windows.Forms.GroupBox
    Friend WithEvents lblPrompt As System.Windows.Forms.Label
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblHint As System.Windows.Forms.Label
    Friend WithEvents btnSignIn As System.Windows.Forms.Button
    Friend WithEvents btnQuit As System.Windows.Forms.Button
    Friend WithEvents lblDatabase As System.Windows.Forms.Label
End Class
