Option Strict On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUnmatched
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
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.btnRecordNew = New System.Windows.Forms.Button()
        Me.btnFindClaim = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.lblQueueCount = New System.Windows.Forms.Label()
        Me.split = New System.Windows.Forms.SplitContainer()
        Me.dgvQueue = New System.Windows.Forms.DataGridView()
        Me.grpPreview = New System.Windows.Forms.GroupBox()
        Me.txtPreview = New System.Windows.Forms.TextBox()
        Me.lvDocs = New System.Windows.Forms.ListView()
        Me.colDocFile = New System.Windows.Forms.ColumnHeader()
        Me.colDocType = New System.Windows.Forms.ColumnHeader()
        Me.btnOpenDoc = New System.Windows.Forms.Button()
        Me.pnlTop.SuspendLayout()
        CType(Me.split, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.split.Panel1.SuspendLayout()
        Me.split.Panel2.SuspendLayout()
        Me.split.SuspendLayout()
        Me.grpPreview.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.btnRecordNew)
        Me.pnlTop.Controls.Add(Me.btnFindClaim)
        Me.pnlTop.Controls.Add(Me.btnRefresh)
        Me.pnlTop.Controls.Add(Me.lblQueueCount)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(900, 38)
        Me.pnlTop.TabIndex = 0
        '
        'btnRecordNew
        '
        Me.btnRecordNew.Location = New System.Drawing.Point(12, 8)
        Me.btnRecordNew.Name = "btnRecordNew"
        Me.btnRecordNew.Size = New System.Drawing.Size(150, 23)
        Me.btnRecordNew.TabIndex = 0
        Me.btnRecordNew.Text = "&Record inbound item..."
        '
        'btnFindClaim
        '
        Me.btnFindClaim.Location = New System.Drawing.Point(168, 8)
        Me.btnFindClaim.Name = "btnFindClaim"
        Me.btnFindClaim.Size = New System.Drawing.Size(160, 23)
        Me.btnFindClaim.TabIndex = 1
        Me.btnFindClaim.Text = "&Find claim for this item..."
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(334, 8)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(80, 23)
        Me.btnRefresh.TabIndex = 2
        Me.btnRefresh.Text = "Re&fresh"
        '
        'lblQueueCount
        '
        Me.lblQueueCount.AutoSize = True
        Me.lblQueueCount.Location = New System.Drawing.Point(428, 13)
        Me.lblQueueCount.Name = "lblQueueCount"
        Me.lblQueueCount.Size = New System.Drawing.Size(0, 13)
        Me.lblQueueCount.TabIndex = 3
        '
        'split
        '
        Me.split.Dock = System.Windows.Forms.DockStyle.Fill
        Me.split.Location = New System.Drawing.Point(0, 38)
        Me.split.Name = "split"
        Me.split.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.split.Panel1.Controls.Add(Me.dgvQueue)
        Me.split.Panel1MinSize = 120
        Me.split.Panel2.Controls.Add(Me.grpPreview)
        Me.split.Panel2MinSize = 120
        Me.split.Size = New System.Drawing.Size(900, 562)
        Me.split.SplitterDistance = 250
        Me.split.TabIndex = 1
        '
        'dgvQueue
        '
        Me.dgvQueue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvQueue.Location = New System.Drawing.Point(0, 0)
        Me.dgvQueue.Name = "dgvQueue"
        Me.dgvQueue.Size = New System.Drawing.Size(900, 250)
        Me.dgvQueue.TabIndex = 0
        '
        'grpPreview
        '
        Me.grpPreview.Controls.Add(Me.txtPreview)
        Me.grpPreview.Controls.Add(Me.lvDocs)
        Me.grpPreview.Controls.Add(Me.btnOpenDoc)
        Me.grpPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpPreview.Location = New System.Drawing.Point(0, 0)
        Me.grpPreview.Name = "grpPreview"
        Me.grpPreview.Size = New System.Drawing.Size(900, 308)
        Me.grpPreview.TabIndex = 0
        Me.grpPreview.TabStop = False
        Me.grpPreview.Text = "Selected item"
        '
        'txtPreview
        '
        Me.txtPreview.Anchor = CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.txtPreview.BackColor = System.Drawing.Color.White
        Me.txtPreview.Location = New System.Drawing.Point(12, 22)
        Me.txtPreview.Multiline = True
        Me.txtPreview.Name = "txtPreview"
        Me.txtPreview.ReadOnly = True
        Me.txtPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPreview.Size = New System.Drawing.Size(560, 274)
        Me.txtPreview.TabIndex = 0
        '
        'lvDocs
        '
        Me.lvDocs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.lvDocs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colDocFile, Me.colDocType})
        Me.lvDocs.FullRowSelect = True
        Me.lvDocs.Location = New System.Drawing.Point(580, 22)
        Me.lvDocs.MultiSelect = False
        Me.lvDocs.Name = "lvDocs"
        Me.lvDocs.Size = New System.Drawing.Size(306, 245)
        Me.lvDocs.TabIndex = 1
        Me.lvDocs.UseCompatibleStateImageBehavior = False
        Me.lvDocs.View = System.Windows.Forms.View.Details
        '
        'colDocFile
        '
        Me.colDocFile.Text = "Attachment"
        Me.colDocFile.Width = 190
        '
        'colDocType
        '
        Me.colDocType.Text = "Type"
        Me.colDocType.Width = 100
        '
        'btnOpenDoc
        '
        Me.btnOpenDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenDoc.Location = New System.Drawing.Point(580, 273)
        Me.btnOpenDoc.Name = "btnOpenDoc"
        Me.btnOpenDoc.Size = New System.Drawing.Size(306, 23)
        Me.btnOpenDoc.TabIndex = 2
        Me.btnOpenDoc.Text = "&Open attachment"
        '
        'frmUnmatched
        '
        Me.ClientSize = New System.Drawing.Size(900, 600)
        Me.Controls.Add(Me.split)
        Me.Controls.Add(Me.pnlTop)
        Me.Name = "frmUnmatched"
        Me.Text = "Intake queue - items not yet matched"
        Me.pnlTop.ResumeLayout(False)
        Me.split.Panel1.ResumeLayout(False)
        Me.split.Panel2.ResumeLayout(False)
        CType(Me.split, System.ComponentModel.ISupportInitialize).EndInit()
        Me.split.ResumeLayout(False)
        Me.grpPreview.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents btnRecordNew As System.Windows.Forms.Button
    Friend WithEvents btnFindClaim As System.Windows.Forms.Button
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents lblQueueCount As System.Windows.Forms.Label
    Friend WithEvents split As System.Windows.Forms.SplitContainer
    Friend WithEvents dgvQueue As System.Windows.Forms.DataGridView
    Friend WithEvents grpPreview As System.Windows.Forms.GroupBox
    Friend WithEvents txtPreview As System.Windows.Forms.TextBox
    Friend WithEvents lvDocs As System.Windows.Forms.ListView
    Friend WithEvents colDocFile As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDocType As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnOpenDoc As System.Windows.Forms.Button
End Class
