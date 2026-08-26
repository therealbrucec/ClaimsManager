Option Strict On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTransition
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
        Me.lblQuestion = New System.Windows.Forms.Label()
        Me.lblFromTo = New System.Windows.Forms.Label()
        Me.lblRationalePrompt = New System.Windows.Forms.Label()
        Me.txtRationale = New System.Windows.Forms.TextBox()
        Me.lblRequired = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblQuestion
        '
        Me.lblQuestion.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblQuestion.Location = New System.Drawing.Point(12, 12)
        Me.lblQuestion.Name = "lblQuestion"
        Me.lblQuestion.Size = New System.Drawing.Size(436, 18)
        Me.lblQuestion.TabIndex = 0
        '
        'lblFromTo
        '
        Me.lblFromTo.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblFromTo.Location = New System.Drawing.Point(12, 34)
        Me.lblFromTo.Name = "lblFromTo"
        Me.lblFromTo.Size = New System.Drawing.Size(436, 15)
        Me.lblFromTo.TabIndex = 1
        '
        'lblRationalePrompt
        '
        Me.lblRationalePrompt.AutoSize = True
        Me.lblRationalePrompt.Location = New System.Drawing.Point(12, 60)
        Me.lblRationalePrompt.Name = "lblRationalePrompt"
        Me.lblRationalePrompt.Size = New System.Drawing.Size(180, 13)
        Me.lblRationalePrompt.TabIndex = 2
        Me.lblRationalePrompt.Text = "&Reason, recorded on the claim file"
        '
        'txtRationale
        '
        Me.txtRationale.AcceptsReturn = True
        Me.txtRationale.Location = New System.Drawing.Point(12, 78)
        Me.txtRationale.Multiline = True
        Me.txtRationale.Name = "txtRationale"
        Me.txtRationale.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRationale.Size = New System.Drawing.Size(436, 130)
        Me.txtRationale.TabIndex = 3
        '
        'lblRequired
        '
        Me.lblRequired.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblRequired.Location = New System.Drawing.Point(12, 216)
        Me.lblRequired.Name = "lblRequired"
        Me.lblRequired.Size = New System.Drawing.Size(266, 30)
        Me.lblRequired.TabIndex = 4
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(292, 218)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 5
        Me.btnOk.Text = "Confirm"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(373, 218)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        '
        'frmTransition
        '
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(460, 253)
        Me.Controls.Add(Me.lblQuestion)
        Me.Controls.Add(Me.lblFromTo)
        Me.Controls.Add(Me.lblRationalePrompt)
        Me.Controls.Add(Me.txtRationale)
        Me.Controls.Add(Me.lblRequired)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTransition"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Confirm decision"
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents lblQuestion As System.Windows.Forms.Label
    Friend WithEvents lblFromTo As System.Windows.Forms.Label
    Friend WithEvents lblRationalePrompt As System.Windows.Forms.Label
    Friend WithEvents txtRationale As System.Windows.Forms.TextBox
    Friend WithEvents lblRequired As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
