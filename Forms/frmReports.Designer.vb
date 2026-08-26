Option Strict On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmReports
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
        Me.grpRange = New System.Windows.Forms.GroupBox()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.btnLast12 = New System.Windows.Forms.Button()
        Me.grpStatus = New System.Windows.Forms.GroupBox()
        Me.lblStatusHelp = New System.Windows.Forms.Label()
        Me.clbStatuses = New System.Windows.Forms.CheckedListBox()
        Me.btnAllStatuses = New System.Windows.Forms.Button()
        Me.btnNoStatuses = New System.Windows.Forms.Button()
        Me.btnOpenOnly = New System.Windows.Forms.Button()
        Me.lblFormatNote = New System.Windows.Forms.Label()
        Me.btnValueReport = New System.Windows.Forms.Button()
        Me.btnFrequencyReport = New System.Windows.Forms.Button()
        Me.lblResult = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.grpRange.SuspendLayout()
        Me.grpStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpRange
        '
        Me.grpRange.Controls.Add(Me.lblFrom)
        Me.grpRange.Controls.Add(Me.dtpFrom)
        Me.grpRange.Controls.Add(Me.lblTo)
        Me.grpRange.Controls.Add(Me.dtpTo)
        Me.grpRange.Controls.Add(Me.btnLast12)
        Me.grpRange.Location = New System.Drawing.Point(12, 12)
        Me.grpRange.Name = "grpRange"
        Me.grpRange.Size = New System.Drawing.Size(500, 58)
        Me.grpRange.TabIndex = 0
        Me.grpRange.TabStop = False
        Me.grpRange.Text = "Period"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(14, 25)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(30, 13)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "&From"
        '
        'dtpFrom
        '
        Me.dtpFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFrom.Location = New System.Drawing.Point(60, 22)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(120, 20)
        Me.dtpFrom.TabIndex = 1
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(200, 25)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "&To"
        '
        'dtpTo
        '
        Me.dtpTo.CustomFormat = "dd/MM/yyyy"
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTo.Location = New System.Drawing.Point(226, 22)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(120, 20)
        Me.dtpTo.TabIndex = 3
        '
        'btnLast12
        '
        Me.btnLast12.Location = New System.Drawing.Point(366, 21)
        Me.btnLast12.Name = "btnLast12"
        Me.btnLast12.Size = New System.Drawing.Size(120, 23)
        Me.btnLast12.TabIndex = 4
        Me.btnLast12.Text = "Last &12 months"
        '
        'grpStatus
        '
        Me.grpStatus.Controls.Add(Me.lblStatusHelp)
        Me.grpStatus.Controls.Add(Me.clbStatuses)
        Me.grpStatus.Controls.Add(Me.btnAllStatuses)
        Me.grpStatus.Controls.Add(Me.btnNoStatuses)
        Me.grpStatus.Controls.Add(Me.btnOpenOnly)
        Me.grpStatus.Location = New System.Drawing.Point(12, 78)
        Me.grpStatus.Name = "grpStatus"
        Me.grpStatus.Size = New System.Drawing.Size(500, 202)
        Me.grpStatus.TabIndex = 1
        Me.grpStatus.TabStop = False
        Me.grpStatus.Text = "Claim status"
        '
        'lblStatusHelp
        '
        Me.lblStatusHelp.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblStatusHelp.Location = New System.Drawing.Point(14, 18)
        Me.lblStatusHelp.Name = "lblStatusHelp"
        Me.lblStatusHelp.Size = New System.Drawing.Size(472, 30)
        Me.lblStatusHelp.TabIndex = 0
        Me.lblStatusHelp.Text = "The value report counts only the statuses ticked here. The frequency report always counts every claim."
        '
        'clbStatuses
        '
        Me.clbStatuses.CheckOnClick = True
        Me.clbStatuses.Location = New System.Drawing.Point(14, 52)
        Me.clbStatuses.Name = "clbStatuses"
        Me.clbStatuses.Size = New System.Drawing.Size(340, 139)
        Me.clbStatuses.TabIndex = 1
        '
        'btnAllStatuses
        '
        Me.btnAllStatuses.Location = New System.Drawing.Point(374, 52)
        Me.btnAllStatuses.Name = "btnAllStatuses"
        Me.btnAllStatuses.Size = New System.Drawing.Size(112, 23)
        Me.btnAllStatuses.TabIndex = 2
        Me.btnAllStatuses.Text = "Select &all"
        '
        'btnNoStatuses
        '
        Me.btnNoStatuses.Location = New System.Drawing.Point(374, 81)
        Me.btnNoStatuses.Name = "btnNoStatuses"
        Me.btnNoStatuses.Size = New System.Drawing.Size(112, 23)
        Me.btnNoStatuses.TabIndex = 3
        Me.btnNoStatuses.Text = "&Clear all"
        '
        'btnOpenOnly
        '
        Me.btnOpenOnly.Location = New System.Drawing.Point(374, 110)
        Me.btnOpenOnly.Name = "btnOpenOnly"
        Me.btnOpenOnly.Size = New System.Drawing.Size(112, 23)
        Me.btnOpenOnly.TabIndex = 4
        Me.btnOpenOnly.Text = "&Open claims"
        '
        'lblFormatNote
        '
        Me.lblFormatNote.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblFormatNote.Location = New System.Drawing.Point(14, 288)
        Me.lblFormatNote.Name = "lblFormatNote"
        Me.lblFormatNote.Size = New System.Drawing.Size(498, 30)
        Me.lblFormatNote.TabIndex = 2
        Me.lblFormatNote.Text = "Each report is a worksheet with a line chart drawn from the figures. This needs Excel installed; without it the figures can be saved as CSV instead."
        '
        'btnValueReport
        '
        Me.btnValueReport.Location = New System.Drawing.Point(12, 322)
        Me.btnValueReport.Name = "btnValueReport"
        Me.btnValueReport.Size = New System.Drawing.Size(244, 30)
        Me.btnValueReport.TabIndex = 3
        Me.btnValueReport.Text = "Export claim &value over time..."
        '
        'btnFrequencyReport
        '
        Me.btnFrequencyReport.Location = New System.Drawing.Point(268, 322)
        Me.btnFrequencyReport.Name = "btnFrequencyReport"
        Me.btnFrequencyReport.Size = New System.Drawing.Size(244, 30)
        Me.btnFrequencyReport.TabIndex = 4
        Me.btnFrequencyReport.Text = "Export claim fre&quency by month..."
        '
        'lblResult
        '
        Me.lblResult.Location = New System.Drawing.Point(14, 360)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.Size = New System.Drawing.Size(498, 32)
        Me.lblResult.TabIndex = 5
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(437, 398)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 6
        Me.btnClose.Text = "Close"
        '
        'frmReports
        '
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(524, 431)
        Me.Controls.Add(Me.grpRange)
        Me.Controls.Add(Me.grpStatus)
        Me.Controls.Add(Me.lblFormatNote)
        Me.Controls.Add(Me.btnValueReport)
        Me.Controls.Add(Me.btnFrequencyReport)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.btnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReports"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Reports"
        Me.grpRange.ResumeLayout(False)
        Me.grpStatus.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents grpRange As System.Windows.Forms.GroupBox
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnLast12 As System.Windows.Forms.Button
    Friend WithEvents grpStatus As System.Windows.Forms.GroupBox
    Friend WithEvents lblStatusHelp As System.Windows.Forms.Label
    Friend WithEvents clbStatuses As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnAllStatuses As System.Windows.Forms.Button
    Friend WithEvents btnNoStatuses As System.Windows.Forms.Button
    Friend WithEvents btnOpenOnly As System.Windows.Forms.Button
    Friend WithEvents lblFormatNote As System.Windows.Forms.Label
    Friend WithEvents btnValueReport As System.Windows.Forms.Button
    Friend WithEvents btnFrequencyReport As System.Windows.Forms.Button
    Friend WithEvents lblResult As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
