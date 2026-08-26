Option Strict On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmIntake
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
        Me.lblContext = New System.Windows.Forms.Label()
        Me.grpItem = New System.Windows.Forms.GroupBox()
        Me.lblSource = New System.Windows.Forms.Label()
        Me.cboSource = New System.Windows.Forms.ComboBox()
        Me.lblReceived = New System.Windows.Forms.Label()
        Me.dtpReceived = New System.Windows.Forms.DateTimePicker()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.txtFromName = New System.Windows.Forms.TextBox()
        Me.lblFromAddress = New System.Windows.Forms.Label()
        Me.txtFromAddress = New System.Windows.Forms.TextBox()
        Me.lblSubject = New System.Windows.Forms.Label()
        Me.txtSubject = New System.Windows.Forms.TextBox()
        Me.lblBody = New System.Windows.Forms.Label()
        Me.txtBody = New System.Windows.Forms.TextBox()
        Me.grpDocs = New System.Windows.Forms.GroupBox()
        Me.lvFiles = New System.Windows.Forms.ListView()
        Me.colFile = New System.Windows.Forms.ColumnHeader()
        Me.colType = New System.Windows.Forms.ColumnHeader()
        Me.lblDocType = New System.Windows.Forms.Label()
        Me.cboDocType = New System.Windows.Forms.ComboBox()
        Me.btnAddFile = New System.Windows.Forms.Button()
        Me.btnRemoveFile = New System.Windows.Forms.Button()
        Me.lblDocsHint = New System.Windows.Forms.Label()
        Me.btnSaveAndMatch = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grpItem.SuspendLayout()
        Me.grpDocs.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblContext
        '
        Me.lblContext.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblContext.Location = New System.Drawing.Point(12, 12)
        Me.lblContext.Name = "lblContext"
        Me.lblContext.Size = New System.Drawing.Size(660, 16)
        Me.lblContext.TabIndex = 0
        Me.lblContext.Text = "Recording an inbound item"
        '
        'grpItem
        '
        Me.grpItem.Controls.Add(Me.lblSource)
        Me.grpItem.Controls.Add(Me.cboSource)
        Me.grpItem.Controls.Add(Me.lblReceived)
        Me.grpItem.Controls.Add(Me.dtpReceived)
        Me.grpItem.Controls.Add(Me.lblFrom)
        Me.grpItem.Controls.Add(Me.txtFromName)
        Me.grpItem.Controls.Add(Me.lblFromAddress)
        Me.grpItem.Controls.Add(Me.txtFromAddress)
        Me.grpItem.Controls.Add(Me.lblSubject)
        Me.grpItem.Controls.Add(Me.txtSubject)
        Me.grpItem.Controls.Add(Me.lblBody)
        Me.grpItem.Controls.Add(Me.txtBody)
        Me.grpItem.Location = New System.Drawing.Point(12, 32)
        Me.grpItem.Name = "grpItem"
        Me.grpItem.Size = New System.Drawing.Size(660, 262)
        Me.grpItem.TabIndex = 1
        Me.grpItem.TabStop = False
        Me.grpItem.Text = "What arrived"
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Location = New System.Drawing.Point(12, 28)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(44, 13)
        Me.lblSource.TabIndex = 0
        Me.lblSource.Text = "Arri&ved by"
        '
        'cboSource
        '
        Me.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSource.Location = New System.Drawing.Point(90, 25)
        Me.cboSource.Name = "cboSource"
        Me.cboSource.Size = New System.Drawing.Size(140, 21)
        Me.cboSource.TabIndex = 1
        '
        'lblReceived
        '
        Me.lblReceived.AutoSize = True
        Me.lblReceived.Location = New System.Drawing.Point(250, 28)
        Me.lblReceived.Name = "lblReceived"
        Me.lblReceived.Size = New System.Drawing.Size(52, 13)
        Me.lblReceived.TabIndex = 2
        Me.lblReceived.Text = "&Received"
        '
        'dtpReceived
        '
        Me.dtpReceived.CustomFormat = "dd/MM/yyyy HH:mm"
        Me.dtpReceived.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpReceived.Location = New System.Drawing.Point(330, 25)
        Me.dtpReceived.Name = "dtpReceived"
        Me.dtpReceived.Size = New System.Drawing.Size(160, 20)
        Me.dtpReceived.TabIndex = 3
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(12, 58)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(30, 13)
        Me.lblFrom.TabIndex = 4
        Me.lblFrom.Text = "Fro&m"
        '
        'txtFromName
        '
        Me.txtFromName.Location = New System.Drawing.Point(90, 55)
        Me.txtFromName.MaxLength = 150
        Me.txtFromName.Name = "txtFromName"
        Me.txtFromName.Size = New System.Drawing.Size(240, 20)
        Me.txtFromName.TabIndex = 5
        '
        'lblFromAddress
        '
        Me.lblFromAddress.AutoSize = True
        Me.lblFromAddress.Location = New System.Drawing.Point(340, 58)
        Me.lblFromAddress.Name = "lblFromAddress"
        Me.lblFromAddress.Size = New System.Drawing.Size(84, 13)
        Me.lblFromAddress.TabIndex = 6
        Me.lblFromAddress.Text = "Email or fa&x no"
        '
        'txtFromAddress
        '
        Me.txtFromAddress.Location = New System.Drawing.Point(440, 55)
        Me.txtFromAddress.MaxLength = 150
        Me.txtFromAddress.Name = "txtFromAddress"
        Me.txtFromAddress.Size = New System.Drawing.Size(210, 20)
        Me.txtFromAddress.TabIndex = 7
        '
        'lblSubject
        '
        Me.lblSubject.AutoSize = True
        Me.lblSubject.Location = New System.Drawing.Point(12, 88)
        Me.lblSubject.Name = "lblSubject"
        Me.lblSubject.Size = New System.Drawing.Size(43, 13)
        Me.lblSubject.TabIndex = 8
        Me.lblSubject.Text = "S&ubject"
        '
        'txtSubject
        '
        Me.txtSubject.Location = New System.Drawing.Point(90, 85)
        Me.txtSubject.MaxLength = 255
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.Size = New System.Drawing.Size(560, 20)
        Me.txtSubject.TabIndex = 9
        '
        'lblBody
        '
        Me.lblBody.AutoSize = True
        Me.lblBody.Location = New System.Drawing.Point(12, 118)
        Me.lblBody.Name = "lblBody"
        Me.lblBody.Size = New System.Drawing.Size(60, 13)
        Me.lblBody.TabIndex = 10
        Me.lblBody.Text = "&Text of item"
        '
        'txtBody
        '
        Me.txtBody.AcceptsReturn = True
        Me.txtBody.Location = New System.Drawing.Point(90, 115)
        Me.txtBody.Multiline = True
        Me.txtBody.Name = "txtBody"
        Me.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBody.Size = New System.Drawing.Size(560, 135)
        Me.txtBody.TabIndex = 11
        '
        'grpDocs
        '
        Me.grpDocs.Controls.Add(Me.lvFiles)
        Me.grpDocs.Controls.Add(Me.lblDocType)
        Me.grpDocs.Controls.Add(Me.cboDocType)
        Me.grpDocs.Controls.Add(Me.btnAddFile)
        Me.grpDocs.Controls.Add(Me.btnRemoveFile)
        Me.grpDocs.Controls.Add(Me.lblDocsHint)
        Me.grpDocs.Location = New System.Drawing.Point(12, 300)
        Me.grpDocs.Name = "grpDocs"
        Me.grpDocs.Size = New System.Drawing.Size(660, 200)
        Me.grpDocs.TabIndex = 2
        Me.grpDocs.TabStop = False
        Me.grpDocs.Text = "Attachments"
        '
        'lvFiles
        '
        Me.lvFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colFile, Me.colType})
        Me.lvFiles.FullRowSelect = True
        Me.lvFiles.HideSelection = False
        Me.lvFiles.Location = New System.Drawing.Point(12, 22)
        Me.lvFiles.MultiSelect = False
        Me.lvFiles.Name = "lvFiles"
        Me.lvFiles.Size = New System.Drawing.Size(440, 165)
        Me.lvFiles.TabIndex = 0
        Me.lvFiles.UseCompatibleStateImageBehavior = False
        Me.lvFiles.View = System.Windows.Forms.View.Details
        '
        'colFile
        '
        Me.colFile.Text = "File"
        Me.colFile.Width = 300
        '
        'colType
        '
        Me.colType.Text = "Type"
        Me.colType.Width = 120
        '
        'lblDocType
        '
        Me.lblDocType.AutoSize = True
        Me.lblDocType.Location = New System.Drawing.Point(462, 25)
        Me.lblDocType.Name = "lblDocType"
        Me.lblDocType.Size = New System.Drawing.Size(75, 13)
        Me.lblDocType.TabIndex = 1
        Me.lblDocType.Text = "&File this next as"
        '
        'cboDocType
        '
        Me.cboDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDocType.Location = New System.Drawing.Point(462, 43)
        Me.cboDocType.Name = "cboDocType"
        Me.cboDocType.Size = New System.Drawing.Size(180, 21)
        Me.cboDocType.TabIndex = 2
        '
        'btnAddFile
        '
        Me.btnAddFile.Location = New System.Drawing.Point(462, 72)
        Me.btnAddFile.Name = "btnAddFile"
        Me.btnAddFile.Size = New System.Drawing.Size(180, 23)
        Me.btnAddFile.TabIndex = 3
        Me.btnAddFile.Text = "&Add files..."
        '
        'btnRemoveFile
        '
        Me.btnRemoveFile.Location = New System.Drawing.Point(462, 101)
        Me.btnRemoveFile.Name = "btnRemoveFile"
        Me.btnRemoveFile.Size = New System.Drawing.Size(180, 23)
        Me.btnRemoveFile.TabIndex = 4
        Me.btnRemoveFile.Text = "Remove selected"
        '
        'lblDocsHint
        '
        Me.lblDocsHint.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblDocsHint.Location = New System.Drawing.Point(462, 132)
        Me.lblDocsHint.Name = "lblDocsHint"
        Me.lblDocsHint.Size = New System.Drawing.Size(180, 55)
        Me.lblDocsHint.TabIndex = 5
        Me.lblDocsHint.Text = "Files are copied into the document store, so the originals can be deleted afterwards."
        '
        'btnSaveAndMatch
        '
        Me.btnSaveAndMatch.Location = New System.Drawing.Point(288, 510)
        Me.btnSaveAndMatch.Name = "btnSaveAndMatch"
        Me.btnSaveAndMatch.Size = New System.Drawing.Size(150, 23)
        Me.btnSaveAndMatch.TabIndex = 3
        Me.btnSaveAndMatch.Text = "Save and find claim..."
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(444, 510)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(130, 23)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "Save to intake queue"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(580, 510)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(92, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        '
        'frmIntake
        '
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(684, 545)
        Me.Controls.Add(Me.lblContext)
        Me.Controls.Add(Me.grpItem)
        Me.Controls.Add(Me.grpDocs)
        Me.Controls.Add(Me.btnSaveAndMatch)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmIntake"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Record inbound item"
        Me.grpItem.ResumeLayout(False)
        Me.grpDocs.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents lblContext As System.Windows.Forms.Label
    Friend WithEvents grpItem As System.Windows.Forms.GroupBox
    Friend WithEvents lblSource As System.Windows.Forms.Label
    Friend WithEvents cboSource As System.Windows.Forms.ComboBox
    Friend WithEvents lblReceived As System.Windows.Forms.Label
    Friend WithEvents dtpReceived As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents txtFromName As System.Windows.Forms.TextBox
    Friend WithEvents lblFromAddress As System.Windows.Forms.Label
    Friend WithEvents txtFromAddress As System.Windows.Forms.TextBox
    Friend WithEvents lblSubject As System.Windows.Forms.Label
    Friend WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents lblBody As System.Windows.Forms.Label
    Friend WithEvents txtBody As System.Windows.Forms.TextBox
    Friend WithEvents grpDocs As System.Windows.Forms.GroupBox
    Friend WithEvents lvFiles As System.Windows.Forms.ListView
    Friend WithEvents colFile As System.Windows.Forms.ColumnHeader
    Friend WithEvents colType As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblDocType As System.Windows.Forms.Label
    Friend WithEvents cboDocType As System.Windows.Forms.ComboBox
    Friend WithEvents btnAddFile As System.Windows.Forms.Button
    Friend WithEvents btnRemoveFile As System.Windows.Forms.Button
    Friend WithEvents lblDocsHint As System.Windows.Forms.Label
    Friend WithEvents btnSaveAndMatch As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
