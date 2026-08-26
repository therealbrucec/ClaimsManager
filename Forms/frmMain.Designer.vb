Option Strict On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
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
        Me.mnuMain = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSignOut = New System.Windows.Forms.ToolStripMenuItem()
        Me.sepFile1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuIntake = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRecordInbound = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUnmatchedQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuClaims = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuNewClaim = New System.Windows.Forms.ToolStripMenuItem()
        Me.sepClaims1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMyClaims = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAllClaims = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAdjusterQueue = New System.Windows.Forms.ToolStripMenuItem()
        Me.sepClaims2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuOpenByRef = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExportReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCloseAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsMain = New System.Windows.Forms.ToolStrip()
        Me.tsNewClaim = New System.Windows.Forms.ToolStripButton()
        Me.tsRecordInbound = New System.Windows.Forms.ToolStripButton()
        Me.tsUnmatched = New System.Windows.Forms.ToolStripButton()
        Me.tsSep1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsMyClaims = New System.Windows.Forms.ToolStripButton()
        Me.tsAdjusterQueue = New System.Windows.Forms.ToolStripButton()
        Me.stsMain = New System.Windows.Forms.StatusStrip()
        Me.lblStatusUser = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblStatusRole = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblStatusUnmatched = New System.Windows.Forms.ToolStripStatusLabel()
        Me.mnuMain.SuspendLayout()
        Me.tsMain.SuspendLayout()
        Me.stsMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuMain
        '
        Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuIntake, Me.mnuClaims, Me.mnuReports, Me.mnuWindow, Me.mnuHelp})
        Me.mnuMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuMain.MdiWindowListItem = Me.mnuWindow
        Me.mnuMain.Name = "mnuMain"
        Me.mnuMain.Size = New System.Drawing.Size(1004, 24)
        Me.mnuMain.TabIndex = 0
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuSignOut, Me.sepFile1, Me.mnuExit})
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "&File"
        '
        'mnuSignOut
        '
        Me.mnuSignOut.Name = "mnuSignOut"
        Me.mnuSignOut.Size = New System.Drawing.Size(180, 22)
        Me.mnuSignOut.Text = "Sign &out"
        '
        'sepFile1
        '
        Me.sepFile1.Name = "sepFile1"
        Me.sepFile1.Size = New System.Drawing.Size(177, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(180, 22)
        Me.mnuExit.Text = "E&xit"
        '
        'mnuIntake
        '
        Me.mnuIntake.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRecordInbound, Me.mnuUnmatchedQueue})
        Me.mnuIntake.Name = "mnuIntake"
        Me.mnuIntake.Size = New System.Drawing.Size(52, 20)
        Me.mnuIntake.Text = "&Intake"
        '
        'mnuRecordInbound
        '
        Me.mnuRecordInbound.Name = "mnuRecordInbound"
        Me.mnuRecordInbound.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.mnuRecordInbound.Size = New System.Drawing.Size(230, 22)
        Me.mnuRecordInbound.Text = "&Record inbound item..."
        '
        'mnuUnmatchedQueue
        '
        Me.mnuUnmatchedQueue.Name = "mnuUnmatchedQueue"
        Me.mnuUnmatchedQueue.Size = New System.Drawing.Size(230, 22)
        Me.mnuUnmatchedQueue.Text = "&Unmatched queue"
        '
        'mnuClaims
        '
        Me.mnuClaims.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNewClaim, Me.sepClaims1, Me.mnuMyClaims, Me.mnuAllClaims, Me.mnuAdjusterQueue, Me.sepClaims2, Me.mnuOpenByRef})
        Me.mnuClaims.Name = "mnuClaims"
        Me.mnuClaims.Size = New System.Drawing.Size(55, 20)
        Me.mnuClaims.Text = "&Claims"
        '
        'mnuNewClaim
        '
        Me.mnuNewClaim.Name = "mnuNewClaim"
        Me.mnuNewClaim.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuNewClaim.Size = New System.Drawing.Size(230, 22)
        Me.mnuNewClaim.Text = "&New claim..."
        '
        'sepClaims1
        '
        Me.sepClaims1.Name = "sepClaims1"
        Me.sepClaims1.Size = New System.Drawing.Size(227, 6)
        '
        'mnuMyClaims
        '
        Me.mnuMyClaims.Name = "mnuMyClaims"
        Me.mnuMyClaims.Size = New System.Drawing.Size(230, 22)
        Me.mnuMyClaims.Text = "&My claims"
        '
        'mnuAllClaims
        '
        Me.mnuAllClaims.Name = "mnuAllClaims"
        Me.mnuAllClaims.Size = New System.Drawing.Size(230, 22)
        Me.mnuAllClaims.Text = "&All claims"
        '
        'mnuAdjusterQueue
        '
        Me.mnuAdjusterQueue.Name = "mnuAdjusterQueue"
        Me.mnuAdjusterQueue.Size = New System.Drawing.Size(230, 22)
        Me.mnuAdjusterQueue.Text = "Adjuster &review queue"
        '
        'sepClaims2
        '
        Me.sepClaims2.Name = "sepClaims2"
        Me.sepClaims2.Size = New System.Drawing.Size(227, 6)
        '
        'mnuOpenByRef
        '
        Me.mnuOpenByRef.Name = "mnuOpenByRef"
        Me.mnuOpenByRef.Size = New System.Drawing.Size(230, 22)
        Me.mnuOpenByRef.Text = "Open by re&ference..."
        '
        'mnuReports
        '
        Me.mnuReports.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExportReports})
        Me.mnuReports.Name = "mnuReports"
        Me.mnuReports.Size = New System.Drawing.Size(62, 20)
        Me.mnuReports.Text = "&Reports"
        '
        'mnuExportReports
        '
        Me.mnuExportReports.Name = "mnuExportReports"
        Me.mnuExportReports.Size = New System.Drawing.Size(230, 22)
        Me.mnuExportReports.Text = "&Export to Excel..."
        '
        'mnuWindow
        '
        Me.mnuWindow.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCloseAll})
        Me.mnuWindow.Name = "mnuWindow"
        Me.mnuWindow.Size = New System.Drawing.Size(63, 20)
        Me.mnuWindow.Text = "&Window"
        '
        'mnuCloseAll
        '
        Me.mnuCloseAll.Name = "mnuCloseAll"
        Me.mnuCloseAll.Size = New System.Drawing.Size(180, 22)
        Me.mnuCloseAll.Text = "Close &all windows"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "&Help"
        '
        'mnuAbout
        '
        Me.mnuAbout.Name = "mnuAbout"
        Me.mnuAbout.Size = New System.Drawing.Size(180, 22)
        Me.mnuAbout.Text = "&About"
        '
        'tsMain
        '
        Me.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsNewClaim, Me.tsRecordInbound, Me.tsUnmatched, Me.tsSep1, Me.tsMyClaims, Me.tsAdjusterQueue})
        Me.tsMain.Location = New System.Drawing.Point(0, 24)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Size = New System.Drawing.Size(1004, 25)
        Me.tsMain.TabIndex = 1
        '
        'tsNewClaim
        '
        Me.tsNewClaim.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsNewClaim.Name = "tsNewClaim"
        Me.tsNewClaim.Size = New System.Drawing.Size(66, 22)
        Me.tsNewClaim.Text = "New claim"
        '
        'tsRecordInbound
        '
        Me.tsRecordInbound.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsRecordInbound.Name = "tsRecordInbound"
        Me.tsRecordInbound.Size = New System.Drawing.Size(120, 22)
        Me.tsRecordInbound.Text = "Record inbound item"
        '
        'tsUnmatched
        '
        Me.tsUnmatched.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsUnmatched.Name = "tsUnmatched"
        Me.tsUnmatched.Size = New System.Drawing.Size(112, 22)
        Me.tsUnmatched.Text = "Unmatched queue"
        '
        'tsSep1
        '
        Me.tsSep1.Name = "tsSep1"
        Me.tsSep1.Size = New System.Drawing.Size(6, 25)
        '
        'tsMyClaims
        '
        Me.tsMyClaims.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsMyClaims.Name = "tsMyClaims"
        Me.tsMyClaims.Size = New System.Drawing.Size(66, 22)
        Me.tsMyClaims.Text = "My claims"
        '
        'tsAdjusterQueue
        '
        Me.tsAdjusterQueue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsAdjusterQueue.Name = "tsAdjusterQueue"
        Me.tsAdjusterQueue.Size = New System.Drawing.Size(96, 22)
        Me.tsAdjusterQueue.Text = "Review queue"
        '
        'stsMain
        '
        Me.stsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatusUser, Me.lblStatusRole, Me.lblStatusUnmatched})
        Me.stsMain.Location = New System.Drawing.Point(0, 619)
        Me.stsMain.Name = "stsMain"
        Me.stsMain.Size = New System.Drawing.Size(1004, 22)
        Me.stsMain.SizingGrip = False
        Me.stsMain.TabIndex = 2
        '
        'lblStatusUser
        '
        Me.lblStatusUser.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right
        Me.lblStatusUser.Name = "lblStatusUser"
        Me.lblStatusUser.Size = New System.Drawing.Size(60, 17)
        Me.lblStatusUser.Text = "Signed in"
        '
        'lblStatusRole
        '
        Me.lblStatusRole.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right
        Me.lblStatusRole.Name = "lblStatusRole"
        Me.lblStatusRole.Size = New System.Drawing.Size(40, 17)
        Me.lblStatusRole.Text = "Role"
        '
        'lblStatusUnmatched
        '
        Me.lblStatusUnmatched.Name = "lblStatusUnmatched"
        Me.lblStatusUnmatched.Size = New System.Drawing.Size(100, 17)
        Me.lblStatusUnmatched.Text = "Intake"
        '
        'frmMain
        '
        Me.ClientSize = New System.Drawing.Size(1004, 641)
        Me.Controls.Add(Me.stsMain)
        Me.Controls.Add(Me.tsMain)
        Me.Controls.Add(Me.mnuMain)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.mnuMain
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Northbridge Assurance - Claims Manager"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.mnuMain.ResumeLayout(False)
        Me.tsMain.ResumeLayout(False)
        Me.stsMain.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents mnuMain As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSignOut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sepFile1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuIntake As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRecordInbound As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuUnmatchedQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuClaims As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewClaim As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sepClaims1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuMyClaims As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllClaims As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAdjusterQueue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sepClaims2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuOpenByRef As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuReports As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExportReports As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuWindow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCloseAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents tsNewClaim As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsRecordInbound As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsUnmatched As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsMyClaims As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsAdjusterQueue As System.Windows.Forms.ToolStripButton
    Friend WithEvents stsMain As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatusUser As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblStatusRole As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblStatusUnmatched As System.Windows.Forms.ToolStripStatusLabel
End Class
