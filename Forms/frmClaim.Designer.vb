Option Strict On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmClaim
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
        Me.components = New System.ComponentModel.Container()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblRefCaption = New System.Windows.Forms.Label()
        Me.lblRef = New System.Windows.Forms.Label()
        Me.lblStatusCaption = New System.Windows.Forms.Label()
        Me.lblStatusValue = New System.Windows.Forms.Label()
        Me.btnAction = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.mnuActions = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tabClaim = New System.Windows.Forms.TabControl()
        Me.tabDetails = New System.Windows.Forms.TabPage()
        Me.lblPolicyNo = New System.Windows.Forms.Label()
        Me.txtPolicyNumber = New System.Windows.Forms.TextBox()
        Me.lblInsuredName = New System.Windows.Forms.Label()
        Me.txtInsuredName = New System.Windows.Forms.TextBox()
        Me.lblPostcode = New System.Windows.Forms.Label()
        Me.txtPostcode = New System.Windows.Forms.TextBox()
        Me.lblPhone = New System.Windows.Forms.Label()
        Me.txtPhone = New System.Windows.Forms.TextBox()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.lblDateOfLoss = New System.Windows.Forms.Label()
        Me.dtpDateOfLoss = New System.Windows.Forms.DateTimePicker()
        Me.chkLossDateKnown = New System.Windows.Forms.CheckBox()
        Me.lblDateNotified = New System.Windows.Forms.Label()
        Me.dtpDateNotified = New System.Windows.Forms.DateTimePicker()
        Me.lblPeril = New System.Windows.Forms.Label()
        Me.cboPeril = New System.Windows.Forms.ComboBox()
        Me.lblEstimate = New System.Windows.Forms.Label()
        Me.txtEstimatedValue = New System.Windows.Forms.TextBox()
        Me.lblReserve = New System.Windows.Forms.Label()
        Me.txtReserve = New System.Windows.Forms.TextBox()
        Me.lblSettled = New System.Windows.Forms.Label()
        Me.txtSettled = New System.Windows.Forms.TextBox()
        Me.lblHandler = New System.Windows.Forms.Label()
        Me.cboHandler = New System.Windows.Forms.ComboBox()
        Me.lblAdjuster = New System.Windows.Forms.Label()
        Me.cboAdjuster = New System.Windows.Forms.ComboBox()
        Me.lblUnderwriter = New System.Windows.Forms.Label()
        Me.cboUnderwriter = New System.Windows.Forms.ComboBox()
        Me.lblLossDescription = New System.Windows.Forms.Label()
        Me.txtLossDescription = New System.Windows.Forms.TextBox()
        Me.tabCorrespondence = New System.Windows.Forms.TabPage()
        Me.dgvCorrespondence = New System.Windows.Forms.DataGridView()
        Me.txtCorrPreview = New System.Windows.Forms.TextBox()
        Me.lvCorrDocs = New System.Windows.Forms.ListView()
        Me.colCorrDocFile = New System.Windows.Forms.ColumnHeader()
        Me.colCorrDocType = New System.Windows.Forms.ColumnHeader()
        Me.btnOpenCorrDoc = New System.Windows.Forms.Button()
        Me.btnRecordInbound = New System.Windows.Forms.Button()
        Me.tabDocuments = New System.Windows.Forms.TabPage()
        Me.dgvDocs = New System.Windows.Forms.DataGridView()
        Me.lblAddType = New System.Windows.Forms.Label()
        Me.cboAddDocType = New System.Windows.Forms.ComboBox()
        Me.btnAddDoc = New System.Windows.Forms.Button()
        Me.btnOpenDoc = New System.Windows.Forms.Button()
        Me.btnRemoveDoc = New System.Windows.Forms.Button()
        Me.lblDocsNote = New System.Windows.Forms.Label()
        Me.tabNotes = New System.Windows.Forms.TabPage()
        Me.dgvNotes = New System.Windows.Forms.DataGridView()
        Me.lblNewNote = New System.Windows.Forms.Label()
        Me.txtNewNote = New System.Windows.Forms.TextBox()
        Me.chkInternal = New System.Windows.Forms.CheckBox()
        Me.btnAddNote = New System.Windows.Forms.Button()
        Me.tabPayments = New System.Windows.Forms.TabPage()
        Me.dgvPayments = New System.Windows.Forms.DataGridView()
        Me.grpAddPayment = New System.Windows.Forms.GroupBox()
        Me.lblPayType = New System.Windows.Forms.Label()
        Me.cboPayType = New System.Windows.Forms.ComboBox()
        Me.lblAmount = New System.Windows.Forms.Label()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.lblPayee = New System.Windows.Forms.Label()
        Me.txtPayee = New System.Windows.Forms.TextBox()
        Me.lblPayRef = New System.Windows.Forms.Label()
        Me.txtPayRef = New System.Windows.Forms.TextBox()
        Me.lblPaidAt = New System.Windows.Forms.Label()
        Me.dtpPaidAt = New System.Windows.Forms.DateTimePicker()
        Me.btnAddPayment = New System.Windows.Forms.Button()
        Me.lblPaymentTotals = New System.Windows.Forms.Label()
        Me.tabHistory = New System.Windows.Forms.TabPage()
        Me.lblHistoryCaption = New System.Windows.Forms.Label()
        Me.dgvHistory = New System.Windows.Forms.DataGridView()
        Me.lblAuditCaption = New System.Windows.Forms.Label()
        Me.dgvAudit = New System.Windows.Forms.DataGridView()
        Me.pnlHeader.SuspendLayout()
        Me.tabClaim.SuspendLayout()
        Me.tabDetails.SuspendLayout()
        Me.tabCorrespondence.SuspendLayout()
        Me.tabDocuments.SuspendLayout()
        Me.tabNotes.SuspendLayout()
        Me.tabPayments.SuspendLayout()
        Me.grpAddPayment.SuspendLayout()
        Me.tabHistory.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.lblRefCaption)
        Me.pnlHeader.Controls.Add(Me.lblRef)
        Me.pnlHeader.Controls.Add(Me.lblStatusCaption)
        Me.pnlHeader.Controls.Add(Me.lblStatusValue)
        Me.pnlHeader.Controls.Add(Me.btnAction)
        Me.pnlHeader.Controls.Add(Me.btnSave)
        Me.pnlHeader.Controls.Add(Me.btnClose)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(908, 48)
        Me.pnlHeader.TabIndex = 0
        '
        'lblRefCaption
        '
        Me.lblRefCaption.AutoSize = True
        Me.lblRefCaption.Location = New System.Drawing.Point(12, 17)
        Me.lblRefCaption.Name = "lblRefCaption"
        Me.lblRefCaption.Size = New System.Drawing.Size(33, 13)
        Me.lblRefCaption.TabIndex = 0
        Me.lblRefCaption.Text = "Claim"
        '
        'lblRef
        '
        Me.lblRef.AutoSize = True
        Me.lblRef.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblRef.Location = New System.Drawing.Point(52, 13)
        Me.lblRef.Name = "lblRef"
        Me.lblRef.Size = New System.Drawing.Size(60, 18)
        Me.lblRef.TabIndex = 1
        Me.lblRef.Text = "New"
        '
        'lblStatusCaption
        '
        Me.lblStatusCaption.AutoSize = True
        Me.lblStatusCaption.Location = New System.Drawing.Point(260, 17)
        Me.lblStatusCaption.Name = "lblStatusCaption"
        Me.lblStatusCaption.Size = New System.Drawing.Size(37, 13)
        Me.lblStatusCaption.TabIndex = 2
        Me.lblStatusCaption.Text = "Status"
        '
        'lblStatusValue
        '
        Me.lblStatusValue.AutoSize = True
        Me.lblStatusValue.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblStatusValue.Location = New System.Drawing.Point(303, 16)
        Me.lblStatusValue.Name = "lblStatusValue"
        Me.lblStatusValue.Size = New System.Drawing.Size(50, 16)
        Me.lblStatusValue.TabIndex = 3
        Me.lblStatusValue.Text = "Notified"
        '
        'btnAction
        '
        Me.btnAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAction.Location = New System.Drawing.Point(586, 13)
        Me.btnAction.Name = "btnAction"
        Me.btnAction.Size = New System.Drawing.Size(140, 23)
        Me.btnAction.TabIndex = 4
        Me.btnAction.Text = "Move claim on..."
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(732, 13)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(80, 23)
        Me.btnSave.TabIndex = 5
        Me.btnSave.Text = "Save claim"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(818, 13)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(78, 23)
        Me.btnClose.TabIndex = 6
        Me.btnClose.Text = "Close"
        '
        'mnuActions
        '
        Me.mnuActions.Name = "mnuActions"
        Me.mnuActions.Size = New System.Drawing.Size(61, 4)
        '
        'tabClaim
        '
        Me.tabClaim.Controls.Add(Me.tabDetails)
        Me.tabClaim.Controls.Add(Me.tabCorrespondence)
        Me.tabClaim.Controls.Add(Me.tabDocuments)
        Me.tabClaim.Controls.Add(Me.tabNotes)
        Me.tabClaim.Controls.Add(Me.tabPayments)
        Me.tabClaim.Controls.Add(Me.tabHistory)
        Me.tabClaim.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabClaim.Location = New System.Drawing.Point(0, 48)
        Me.tabClaim.Name = "tabClaim"
        Me.tabClaim.SelectedIndex = 0
        Me.tabClaim.Size = New System.Drawing.Size(908, 613)
        Me.tabClaim.TabIndex = 1
        '
        'tabDetails
        '
        Me.tabDetails.Controls.Add(Me.lblPolicyNo)
        Me.tabDetails.Controls.Add(Me.txtPolicyNumber)
        Me.tabDetails.Controls.Add(Me.lblInsuredName)
        Me.tabDetails.Controls.Add(Me.txtInsuredName)
        Me.tabDetails.Controls.Add(Me.lblPostcode)
        Me.tabDetails.Controls.Add(Me.txtPostcode)
        Me.tabDetails.Controls.Add(Me.lblPhone)
        Me.tabDetails.Controls.Add(Me.txtPhone)
        Me.tabDetails.Controls.Add(Me.lblEmail)
        Me.tabDetails.Controls.Add(Me.txtEmail)
        Me.tabDetails.Controls.Add(Me.lblDateOfLoss)
        Me.tabDetails.Controls.Add(Me.dtpDateOfLoss)
        Me.tabDetails.Controls.Add(Me.chkLossDateKnown)
        Me.tabDetails.Controls.Add(Me.lblDateNotified)
        Me.tabDetails.Controls.Add(Me.dtpDateNotified)
        Me.tabDetails.Controls.Add(Me.lblPeril)
        Me.tabDetails.Controls.Add(Me.cboPeril)
        Me.tabDetails.Controls.Add(Me.lblEstimate)
        Me.tabDetails.Controls.Add(Me.txtEstimatedValue)
        Me.tabDetails.Controls.Add(Me.lblReserve)
        Me.tabDetails.Controls.Add(Me.txtReserve)
        Me.tabDetails.Controls.Add(Me.lblSettled)
        Me.tabDetails.Controls.Add(Me.txtSettled)
        Me.tabDetails.Controls.Add(Me.lblHandler)
        Me.tabDetails.Controls.Add(Me.cboHandler)
        Me.tabDetails.Controls.Add(Me.lblAdjuster)
        Me.tabDetails.Controls.Add(Me.cboAdjuster)
        Me.tabDetails.Controls.Add(Me.lblUnderwriter)
        Me.tabDetails.Controls.Add(Me.cboUnderwriter)
        Me.tabDetails.Controls.Add(Me.lblLossDescription)
        Me.tabDetails.Controls.Add(Me.txtLossDescription)
        Me.tabDetails.Location = New System.Drawing.Point(4, 22)
        Me.tabDetails.Name = "tabDetails"
        Me.tabDetails.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDetails.Size = New System.Drawing.Size(900, 587)
        Me.tabDetails.TabIndex = 0
        Me.tabDetails.Text = "Details"
        Me.tabDetails.UseVisualStyleBackColor = True
        '
        'lblPolicyNo
        '
        Me.lblPolicyNo.AutoSize = True
        Me.lblPolicyNo.Location = New System.Drawing.Point(14, 21)
        Me.lblPolicyNo.Name = "lblPolicyNo"
        Me.lblPolicyNo.Size = New System.Drawing.Size(55, 13)
        Me.lblPolicyNo.TabIndex = 0
        Me.lblPolicyNo.Text = "&Policy no"
        '
        'txtPolicyNumber
        '
        Me.txtPolicyNumber.Location = New System.Drawing.Point(120, 18)
        Me.txtPolicyNumber.MaxLength = 30
        Me.txtPolicyNumber.Name = "txtPolicyNumber"
        Me.txtPolicyNumber.Size = New System.Drawing.Size(200, 20)
        Me.txtPolicyNumber.TabIndex = 1
        '
        'lblInsuredName
        '
        Me.lblInsuredName.AutoSize = True
        Me.lblInsuredName.Location = New System.Drawing.Point(14, 47)
        Me.lblInsuredName.Name = "lblInsuredName"
        Me.lblInsuredName.Size = New System.Drawing.Size(43, 13)
        Me.lblInsuredName.TabIndex = 2
        Me.lblInsuredName.Text = "&Insured"
        '
        'txtInsuredName
        '
        Me.txtInsuredName.Location = New System.Drawing.Point(120, 44)
        Me.txtInsuredName.MaxLength = 100
        Me.txtInsuredName.Name = "txtInsuredName"
        Me.txtInsuredName.Size = New System.Drawing.Size(280, 20)
        Me.txtInsuredName.TabIndex = 3
        '
        'lblPostcode
        '
        Me.lblPostcode.AutoSize = True
        Me.lblPostcode.Location = New System.Drawing.Point(14, 73)
        Me.lblPostcode.Name = "lblPostcode"
        Me.lblPostcode.Size = New System.Drawing.Size(52, 13)
        Me.lblPostcode.TabIndex = 4
        Me.lblPostcode.Text = "Post&code"
        '
        'txtPostcode
        '
        Me.txtPostcode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPostcode.Location = New System.Drawing.Point(120, 70)
        Me.txtPostcode.MaxLength = 12
        Me.txtPostcode.Name = "txtPostcode"
        Me.txtPostcode.Size = New System.Drawing.Size(100, 20)
        Me.txtPostcode.TabIndex = 5
        '
        'lblPhone
        '
        Me.lblPhone.AutoSize = True
        Me.lblPhone.Location = New System.Drawing.Point(14, 99)
        Me.lblPhone.Name = "lblPhone"
        Me.lblPhone.Size = New System.Drawing.Size(38, 13)
        Me.lblPhone.TabIndex = 6
        Me.lblPhone.Text = "P&hone"
        '
        'txtPhone
        '
        Me.txtPhone.Location = New System.Drawing.Point(120, 96)
        Me.txtPhone.MaxLength = 30
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(160, 20)
        Me.txtPhone.TabIndex = 7
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(14, 125)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(32, 13)
        Me.lblEmail.TabIndex = 8
        Me.lblEmail.Text = "&Email"
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(120, 122)
        Me.txtEmail.MaxLength = 120
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(280, 20)
        Me.txtEmail.TabIndex = 9
        '
        'lblDateOfLoss
        '
        Me.lblDateOfLoss.AutoSize = True
        Me.lblDateOfLoss.Location = New System.Drawing.Point(14, 151)
        Me.lblDateOfLoss.Name = "lblDateOfLoss"
        Me.lblDateOfLoss.Size = New System.Drawing.Size(64, 13)
        Me.lblDateOfLoss.TabIndex = 10
        Me.lblDateOfLoss.Text = "Date of &loss"
        '
        'dtpDateOfLoss
        '
        Me.dtpDateOfLoss.CustomFormat = "dd/MM/yyyy"
        Me.dtpDateOfLoss.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDateOfLoss.Location = New System.Drawing.Point(120, 148)
        Me.dtpDateOfLoss.Name = "dtpDateOfLoss"
        Me.dtpDateOfLoss.Size = New System.Drawing.Size(110, 20)
        Me.dtpDateOfLoss.TabIndex = 11
        '
        'chkLossDateKnown
        '
        Me.chkLossDateKnown.AutoSize = True
        Me.chkLossDateKnown.Checked = True
        Me.chkLossDateKnown.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLossDateKnown.Location = New System.Drawing.Point(240, 150)
        Me.chkLossDateKnown.Name = "chkLossDateKnown"
        Me.chkLossDateKnown.Size = New System.Drawing.Size(84, 17)
        Me.chkLossDateKnown.TabIndex = 12
        Me.chkLossDateKnown.Text = "Date is &known"
        '
        'lblDateNotified
        '
        Me.lblDateNotified.AutoSize = True
        Me.lblDateNotified.Location = New System.Drawing.Point(14, 177)
        Me.lblDateNotified.Name = "lblDateNotified"
        Me.lblDateNotified.Size = New System.Drawing.Size(70, 13)
        Me.lblDateNotified.TabIndex = 13
        Me.lblDateNotified.Text = "Date &notified"
        '
        'dtpDateNotified
        '
        Me.dtpDateNotified.CustomFormat = "dd/MM/yyyy"
        Me.dtpDateNotified.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDateNotified.Location = New System.Drawing.Point(120, 174)
        Me.dtpDateNotified.Name = "dtpDateNotified"
        Me.dtpDateNotified.Size = New System.Drawing.Size(110, 20)
        Me.dtpDateNotified.TabIndex = 14
        '
        'lblPeril
        '
        Me.lblPeril.AutoSize = True
        Me.lblPeril.Location = New System.Drawing.Point(14, 203)
        Me.lblPeril.Name = "lblPeril"
        Me.lblPeril.Size = New System.Drawing.Size(27, 13)
        Me.lblPeril.TabIndex = 15
        Me.lblPeril.Text = "Peri&l"
        '
        'cboPeril
        '
        Me.cboPeril.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPeril.Location = New System.Drawing.Point(120, 200)
        Me.cboPeril.Name = "cboPeril"
        Me.cboPeril.Size = New System.Drawing.Size(200, 21)
        Me.cboPeril.TabIndex = 16
        '
        'lblEstimate
        '
        Me.lblEstimate.AutoSize = True
        Me.lblEstimate.Location = New System.Drawing.Point(440, 21)
        Me.lblEstimate.Name = "lblEstimate"
        Me.lblEstimate.Size = New System.Drawing.Size(83, 13)
        Me.lblEstimate.TabIndex = 17
        Me.lblEstimate.Text = "Estimated val&ue"
        '
        'txtEstimatedValue
        '
        Me.txtEstimatedValue.Location = New System.Drawing.Point(560, 18)
        Me.txtEstimatedValue.Name = "txtEstimatedValue"
        Me.txtEstimatedValue.Size = New System.Drawing.Size(110, 20)
        Me.txtEstimatedValue.TabIndex = 18
        Me.txtEstimatedValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblReserve
        '
        Me.lblReserve.AutoSize = True
        Me.lblReserve.Location = New System.Drawing.Point(440, 47)
        Me.lblReserve.Name = "lblReserve"
        Me.lblReserve.Size = New System.Drawing.Size(47, 13)
        Me.lblReserve.TabIndex = 19
        Me.lblReserve.Text = "Reser&ve"
        '
        'txtReserve
        '
        Me.txtReserve.Location = New System.Drawing.Point(560, 44)
        Me.txtReserve.Name = "txtReserve"
        Me.txtReserve.Size = New System.Drawing.Size(110, 20)
        Me.txtReserve.TabIndex = 20
        Me.txtReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblSettled
        '
        Me.lblSettled.AutoSize = True
        Me.lblSettled.Location = New System.Drawing.Point(440, 73)
        Me.lblSettled.Name = "lblSettled"
        Me.lblSettled.Size = New System.Drawing.Size(48, 13)
        Me.lblSettled.TabIndex = 21
        Me.lblSettled.Text = "Paid net"
        '
        'txtSettled
        '
        Me.txtSettled.BackColor = System.Drawing.SystemColors.Control
        Me.txtSettled.Location = New System.Drawing.Point(560, 70)
        Me.txtSettled.Name = "txtSettled"
        Me.txtSettled.ReadOnly = True
        Me.txtSettled.Size = New System.Drawing.Size(110, 20)
        Me.txtSettled.TabIndex = 22
        Me.txtSettled.TabStop = False
        Me.txtSettled.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblHandler
        '
        Me.lblHandler.AutoSize = True
        Me.lblHandler.Location = New System.Drawing.Point(440, 99)
        Me.lblHandler.Name = "lblHandler"
        Me.lblHandler.Size = New System.Drawing.Size(44, 13)
        Me.lblHandler.TabIndex = 23
        Me.lblHandler.Text = "Han&dler"
        '
        'cboHandler
        '
        Me.cboHandler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHandler.Location = New System.Drawing.Point(560, 96)
        Me.cboHandler.Name = "cboHandler"
        Me.cboHandler.Size = New System.Drawing.Size(280, 21)
        Me.cboHandler.TabIndex = 24
        '
        'lblAdjuster
        '
        Me.lblAdjuster.AutoSize = True
        Me.lblAdjuster.Location = New System.Drawing.Point(440, 125)
        Me.lblAdjuster.Name = "lblAdjuster"
        Me.lblAdjuster.Size = New System.Drawing.Size(69, 13)
        Me.lblAdjuster.TabIndex = 25
        Me.lblAdjuster.Text = "Loss ad&juster"
        '
        'cboAdjuster
        '
        Me.cboAdjuster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAdjuster.Location = New System.Drawing.Point(560, 122)
        Me.cboAdjuster.Name = "cboAdjuster"
        Me.cboAdjuster.Size = New System.Drawing.Size(280, 21)
        Me.cboAdjuster.TabIndex = 26
        '
        'lblUnderwriter
        '
        Me.lblUnderwriter.AutoSize = True
        Me.lblUnderwriter.Location = New System.Drawing.Point(440, 151)
        Me.lblUnderwriter.Name = "lblUnderwriter"
        Me.lblUnderwriter.Size = New System.Drawing.Size(63, 13)
        Me.lblUnderwriter.TabIndex = 27
        Me.lblUnderwriter.Text = "Under&writer"
        '
        'cboUnderwriter
        '
        Me.cboUnderwriter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUnderwriter.Location = New System.Drawing.Point(560, 148)
        Me.cboUnderwriter.Name = "cboUnderwriter"
        Me.cboUnderwriter.Size = New System.Drawing.Size(280, 21)
        Me.cboUnderwriter.TabIndex = 28
        '
        'lblLossDescription
        '
        Me.lblLossDescription.AutoSize = True
        Me.lblLossDescription.Location = New System.Drawing.Point(14, 240)
        Me.lblLossDescription.Name = "lblLossDescription"
        Me.lblLossDescription.Size = New System.Drawing.Size(96, 13)
        Me.lblLossDescription.TabIndex = 29
        Me.lblLossDescription.Text = "What ha&ppened"
        '
        'txtLossDescription
        '
        Me.txtLossDescription.AcceptsReturn = True
        Me.txtLossDescription.Anchor = CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.txtLossDescription.Location = New System.Drawing.Point(120, 240)
        Me.txtLossDescription.Multiline = True
        Me.txtLossDescription.Name = "txtLossDescription"
        Me.txtLossDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLossDescription.Size = New System.Drawing.Size(720, 320)
        Me.txtLossDescription.TabIndex = 30
        '
        'tabCorrespondence
        '
        Me.tabCorrespondence.Controls.Add(Me.dgvCorrespondence)
        Me.tabCorrespondence.Controls.Add(Me.txtCorrPreview)
        Me.tabCorrespondence.Controls.Add(Me.lvCorrDocs)
        Me.tabCorrespondence.Controls.Add(Me.btnOpenCorrDoc)
        Me.tabCorrespondence.Controls.Add(Me.btnRecordInbound)
        Me.tabCorrespondence.Location = New System.Drawing.Point(4, 22)
        Me.tabCorrespondence.Name = "tabCorrespondence"
        Me.tabCorrespondence.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCorrespondence.Size = New System.Drawing.Size(900, 587)
        Me.tabCorrespondence.TabIndex = 1
        Me.tabCorrespondence.Text = "Correspondence"
        Me.tabCorrespondence.UseVisualStyleBackColor = True
        '
        'dgvCorrespondence
        '
        Me.dgvCorrespondence.Anchor = CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.dgvCorrespondence.Location = New System.Drawing.Point(12, 12)
        Me.dgvCorrespondence.Name = "dgvCorrespondence"
        Me.dgvCorrespondence.Size = New System.Drawing.Size(876, 240)
        Me.dgvCorrespondence.TabIndex = 0
        '
        'txtCorrPreview
        '
        Me.txtCorrPreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.txtCorrPreview.BackColor = System.Drawing.Color.White
        Me.txtCorrPreview.Location = New System.Drawing.Point(12, 262)
        Me.txtCorrPreview.Multiline = True
        Me.txtCorrPreview.Name = "txtCorrPreview"
        Me.txtCorrPreview.ReadOnly = True
        Me.txtCorrPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCorrPreview.Size = New System.Drawing.Size(596, 280)
        Me.txtCorrPreview.TabIndex = 1
        '
        'lvCorrDocs
        '
        Me.lvCorrDocs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.lvCorrDocs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colCorrDocFile, Me.colCorrDocType})
        Me.lvCorrDocs.FullRowSelect = True
        Me.lvCorrDocs.Location = New System.Drawing.Point(616, 262)
        Me.lvCorrDocs.MultiSelect = False
        Me.lvCorrDocs.Name = "lvCorrDocs"
        Me.lvCorrDocs.Size = New System.Drawing.Size(272, 251)
        Me.lvCorrDocs.TabIndex = 2
        Me.lvCorrDocs.UseCompatibleStateImageBehavior = False
        Me.lvCorrDocs.View = System.Windows.Forms.View.Details
        '
        'colCorrDocFile
        '
        Me.colCorrDocFile.Text = "Came with this item"
        Me.colCorrDocFile.Width = 170
        '
        'colCorrDocType
        '
        Me.colCorrDocType.Text = "Type"
        Me.colCorrDocType.Width = 90
        '
        'btnOpenCorrDoc
        '
        Me.btnOpenCorrDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenCorrDoc.Location = New System.Drawing.Point(616, 519)
        Me.btnOpenCorrDoc.Name = "btnOpenCorrDoc"
        Me.btnOpenCorrDoc.Size = New System.Drawing.Size(272, 23)
        Me.btnOpenCorrDoc.TabIndex = 3
        Me.btnOpenCorrDoc.Text = "&Open attachment"
        '
        'btnRecordInbound
        '
        Me.btnRecordInbound.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRecordInbound.Location = New System.Drawing.Point(12, 550)
        Me.btnRecordInbound.Name = "btnRecordInbound"
        Me.btnRecordInbound.Size = New System.Drawing.Size(220, 23)
        Me.btnRecordInbound.TabIndex = 4
        Me.btnRecordInbound.Text = "&Record an item against this claim..."
        '
        'tabDocuments
        '
        Me.tabDocuments.Controls.Add(Me.dgvDocs)
        Me.tabDocuments.Controls.Add(Me.lblAddType)
        Me.tabDocuments.Controls.Add(Me.cboAddDocType)
        Me.tabDocuments.Controls.Add(Me.btnAddDoc)
        Me.tabDocuments.Controls.Add(Me.btnOpenDoc)
        Me.tabDocuments.Controls.Add(Me.btnRemoveDoc)
        Me.tabDocuments.Controls.Add(Me.lblDocsNote)
        Me.tabDocuments.Location = New System.Drawing.Point(4, 22)
        Me.tabDocuments.Name = "tabDocuments"
        Me.tabDocuments.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDocuments.Size = New System.Drawing.Size(900, 587)
        Me.tabDocuments.TabIndex = 2
        Me.tabDocuments.Text = "Documents"
        Me.tabDocuments.UseVisualStyleBackColor = True
        '
        'dgvDocs
        '
        Me.dgvDocs.Anchor = CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.dgvDocs.Location = New System.Drawing.Point(12, 12)
        Me.dgvDocs.Name = "dgvDocs"
        Me.dgvDocs.Size = New System.Drawing.Size(876, 478)
        Me.dgvDocs.TabIndex = 0
        '
        'lblAddType
        '
        Me.lblAddType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAddType.AutoSize = True
        Me.lblAddType.Location = New System.Drawing.Point(12, 506)
        Me.lblAddType.Name = "lblAddType"
        Me.lblAddType.Size = New System.Drawing.Size(74, 13)
        Me.lblAddType.TabIndex = 1
        Me.lblAddType.Text = "&File the next as"
        '
        'cboAddDocType
        '
        Me.cboAddDocType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboAddDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAddDocType.Location = New System.Drawing.Point(100, 503)
        Me.cboAddDocType.Name = "cboAddDocType"
        Me.cboAddDocType.Size = New System.Drawing.Size(180, 21)
        Me.cboAddDocType.TabIndex = 2
        '
        'btnAddDoc
        '
        Me.btnAddDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddDoc.Location = New System.Drawing.Point(290, 503)
        Me.btnAddDoc.Name = "btnAddDoc"
        Me.btnAddDoc.Size = New System.Drawing.Size(130, 23)
        Me.btnAddDoc.TabIndex = 3
        Me.btnAddDoc.Text = "&Add documents..."
        '
        'btnOpenDoc
        '
        Me.btnOpenDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOpenDoc.Location = New System.Drawing.Point(426, 503)
        Me.btnOpenDoc.Name = "btnOpenDoc"
        Me.btnOpenDoc.Size = New System.Drawing.Size(110, 23)
        Me.btnOpenDoc.TabIndex = 4
        Me.btnOpenDoc.Text = "&Open"
        '
        'btnRemoveDoc
        '
        Me.btnRemoveDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveDoc.Location = New System.Drawing.Point(542, 503)
        Me.btnRemoveDoc.Name = "btnRemoveDoc"
        Me.btnRemoveDoc.Size = New System.Drawing.Size(110, 23)
        Me.btnRemoveDoc.TabIndex = 5
        Me.btnRemoveDoc.Text = "Remo&ve"
        '
        'lblDocsNote
        '
        Me.lblDocsNote.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDocsNote.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblDocsNote.Location = New System.Drawing.Point(12, 535)
        Me.lblDocsNote.Name = "lblDocsNote"
        Me.lblDocsNote.Size = New System.Drawing.Size(660, 30)
        Me.lblDocsNote.TabIndex = 6
        Me.lblDocsNote.Text = "Removing a document takes it off the claim but leaves the file in the document store."
        '
        'tabNotes
        '
        Me.tabNotes.Controls.Add(Me.dgvNotes)
        Me.tabNotes.Controls.Add(Me.lblNewNote)
        Me.tabNotes.Controls.Add(Me.txtNewNote)
        Me.tabNotes.Controls.Add(Me.chkInternal)
        Me.tabNotes.Controls.Add(Me.btnAddNote)
        Me.tabNotes.Location = New System.Drawing.Point(4, 22)
        Me.tabNotes.Name = "tabNotes"
        Me.tabNotes.Padding = New System.Windows.Forms.Padding(3)
        Me.tabNotes.Size = New System.Drawing.Size(900, 587)
        Me.tabNotes.TabIndex = 3
        Me.tabNotes.Text = "Notes"
        Me.tabNotes.UseVisualStyleBackColor = True
        '
        'dgvNotes
        '
        Me.dgvNotes.Anchor = CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.dgvNotes.Location = New System.Drawing.Point(12, 12)
        Me.dgvNotes.Name = "dgvNotes"
        Me.dgvNotes.Size = New System.Drawing.Size(876, 400)
        Me.dgvNotes.TabIndex = 0
        '
        'lblNewNote
        '
        Me.lblNewNote.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNewNote.AutoSize = True
        Me.lblNewNote.Location = New System.Drawing.Point(12, 424)
        Me.lblNewNote.Name = "lblNewNote"
        Me.lblNewNote.Size = New System.Drawing.Size(52, 13)
        Me.lblNewNote.TabIndex = 1
        Me.lblNewNote.Text = "&New note"
        '
        'txtNewNote
        '
        Me.txtNewNote.AcceptsReturn = True
        Me.txtNewNote.Anchor = CType((((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.txtNewNote.Location = New System.Drawing.Point(12, 442)
        Me.txtNewNote.Multiline = True
        Me.txtNewNote.Name = "txtNewNote"
        Me.txtNewNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNewNote.Size = New System.Drawing.Size(700, 120)
        Me.txtNewNote.TabIndex = 2
        '
        'chkInternal
        '
        Me.chkInternal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkInternal.AutoSize = True
        Me.chkInternal.Checked = True
        Me.chkInternal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkInternal.Location = New System.Drawing.Point(724, 442)
        Me.chkInternal.Name = "chkInternal"
        Me.chkInternal.Size = New System.Drawing.Size(120, 17)
        Me.chkInternal.TabIndex = 3
        Me.chkInternal.Text = "&Internal only"
        '
        'btnAddNote
        '
        Me.btnAddNote.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddNote.Location = New System.Drawing.Point(724, 470)
        Me.btnAddNote.Name = "btnAddNote"
        Me.btnAddNote.Size = New System.Drawing.Size(164, 23)
        Me.btnAddNote.TabIndex = 4
        Me.btnAddNote.Text = "A&dd note"
        '
        'tabPayments
        '
        Me.tabPayments.Controls.Add(Me.dgvPayments)
        Me.tabPayments.Controls.Add(Me.grpAddPayment)
        Me.tabPayments.Controls.Add(Me.lblPaymentTotals)
        Me.tabPayments.Location = New System.Drawing.Point(4, 22)
        Me.tabPayments.Name = "tabPayments"
        Me.tabPayments.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPayments.Size = New System.Drawing.Size(900, 587)
        Me.tabPayments.TabIndex = 4
        Me.tabPayments.Text = "Payments"
        Me.tabPayments.UseVisualStyleBackColor = True
        '
        'dgvPayments
        '
        Me.dgvPayments.Anchor = CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.dgvPayments.Location = New System.Drawing.Point(12, 12)
        Me.dgvPayments.Name = "dgvPayments"
        Me.dgvPayments.Size = New System.Drawing.Size(876, 400)
        Me.dgvPayments.TabIndex = 0
        '
        'grpAddPayment
        '
        Me.grpAddPayment.Anchor = CType((((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.grpAddPayment.Controls.Add(Me.lblPayType)
        Me.grpAddPayment.Controls.Add(Me.cboPayType)
        Me.grpAddPayment.Controls.Add(Me.lblAmount)
        Me.grpAddPayment.Controls.Add(Me.txtAmount)
        Me.grpAddPayment.Controls.Add(Me.lblPayee)
        Me.grpAddPayment.Controls.Add(Me.txtPayee)
        Me.grpAddPayment.Controls.Add(Me.lblPayRef)
        Me.grpAddPayment.Controls.Add(Me.txtPayRef)
        Me.grpAddPayment.Controls.Add(Me.lblPaidAt)
        Me.grpAddPayment.Controls.Add(Me.dtpPaidAt)
        Me.grpAddPayment.Controls.Add(Me.btnAddPayment)
        Me.grpAddPayment.Location = New System.Drawing.Point(12, 424)
        Me.grpAddPayment.Name = "grpAddPayment"
        Me.grpAddPayment.Size = New System.Drawing.Size(876, 110)
        Me.grpAddPayment.TabIndex = 1
        Me.grpAddPayment.TabStop = False
        Me.grpAddPayment.Text = "Record a payment"
        '
        'lblPayType
        '
        Me.lblPayType.AutoSize = True
        Me.lblPayType.Location = New System.Drawing.Point(12, 28)
        Me.lblPayType.Name = "lblPayType"
        Me.lblPayType.Size = New System.Drawing.Size(31, 13)
        Me.lblPayType.TabIndex = 0
        Me.lblPayType.Text = "T&ype"
        '
        'cboPayType
        '
        Me.cboPayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPayType.Location = New System.Drawing.Point(80, 25)
        Me.cboPayType.Name = "cboPayType"
        Me.cboPayType.Size = New System.Drawing.Size(120, 21)
        Me.cboPayType.TabIndex = 1
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.Location = New System.Drawing.Point(212, 28)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.Size = New System.Drawing.Size(43, 13)
        Me.lblAmount.TabIndex = 2
        Me.lblAmount.Text = "A&mount"
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(270, 25)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(100, 20)
        Me.txtAmount.TabIndex = 3
        Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblPayee
        '
        Me.lblPayee.AutoSize = True
        Me.lblPayee.Location = New System.Drawing.Point(388, 28)
        Me.lblPayee.Name = "lblPayee"
        Me.lblPayee.Size = New System.Drawing.Size(36, 13)
        Me.lblPayee.TabIndex = 4
        Me.lblPayee.Text = "Pa&yee"
        '
        'txtPayee
        '
        Me.txtPayee.Location = New System.Drawing.Point(430, 25)
        Me.txtPayee.MaxLength = 120
        Me.txtPayee.Name = "txtPayee"
        Me.txtPayee.Size = New System.Drawing.Size(200, 20)
        Me.txtPayee.TabIndex = 5
        '
        'lblPayRef
        '
        Me.lblPayRef.AutoSize = True
        Me.lblPayRef.Location = New System.Drawing.Point(644, 28)
        Me.lblPayRef.Name = "lblPayRef"
        Me.lblPayRef.Size = New System.Drawing.Size(50, 13)
        Me.lblPayRef.TabIndex = 6
        Me.lblPayRef.Text = "Re&ference"
        '
        'txtPayRef
        '
        Me.txtPayRef.Location = New System.Drawing.Point(700, 25)
        Me.txtPayRef.MaxLength = 50
        Me.txtPayRef.Name = "txtPayRef"
        Me.txtPayRef.Size = New System.Drawing.Size(160, 20)
        Me.txtPayRef.TabIndex = 7
        '
        'lblPaidAt
        '
        Me.lblPaidAt.AutoSize = True
        Me.lblPaidAt.Location = New System.Drawing.Point(12, 62)
        Me.lblPaidAt.Name = "lblPaidAt"
        Me.lblPaidAt.Size = New System.Drawing.Size(45, 13)
        Me.lblPaidAt.TabIndex = 8
        Me.lblPaidAt.Text = "Paid &on"
        '
        'dtpPaidAt
        '
        Me.dtpPaidAt.CustomFormat = "dd/MM/yyyy"
        Me.dtpPaidAt.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpPaidAt.Location = New System.Drawing.Point(80, 59)
        Me.dtpPaidAt.Name = "dtpPaidAt"
        Me.dtpPaidAt.Size = New System.Drawing.Size(120, 20)
        Me.dtpPaidAt.TabIndex = 9
        '
        'btnAddPayment
        '
        Me.btnAddPayment.Location = New System.Drawing.Point(700, 58)
        Me.btnAddPayment.Name = "btnAddPayment"
        Me.btnAddPayment.Size = New System.Drawing.Size(160, 23)
        Me.btnAddPayment.TabIndex = 10
        Me.btnAddPayment.Text = "Record &payment"
        '
        'lblPaymentTotals
        '
        Me.lblPaymentTotals.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPaymentTotals.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblPaymentTotals.Location = New System.Drawing.Point(12, 545)
        Me.lblPaymentTotals.Name = "lblPaymentTotals"
        Me.lblPaymentTotals.Size = New System.Drawing.Size(700, 16)
        Me.lblPaymentTotals.TabIndex = 2
        '
        'tabHistory
        '
        Me.tabHistory.Controls.Add(Me.lblHistoryCaption)
        Me.tabHistory.Controls.Add(Me.dgvHistory)
        Me.tabHistory.Controls.Add(Me.lblAuditCaption)
        Me.tabHistory.Controls.Add(Me.dgvAudit)
        Me.tabHistory.Location = New System.Drawing.Point(4, 22)
        Me.tabHistory.Name = "tabHistory"
        Me.tabHistory.Padding = New System.Windows.Forms.Padding(3)
        Me.tabHistory.Size = New System.Drawing.Size(900, 587)
        Me.tabHistory.TabIndex = 5
        Me.tabHistory.Text = "History"
        Me.tabHistory.UseVisualStyleBackColor = True
        '
        'lblHistoryCaption
        '
        Me.lblHistoryCaption.AutoSize = True
        Me.lblHistoryCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblHistoryCaption.Location = New System.Drawing.Point(12, 12)
        Me.lblHistoryCaption.Name = "lblHistoryCaption"
        Me.lblHistoryCaption.Size = New System.Drawing.Size(90, 13)
        Me.lblHistoryCaption.TabIndex = 0
        Me.lblHistoryCaption.Text = "Status history"
        '
        'dgvHistory
        '
        Me.dgvHistory.Anchor = CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.dgvHistory.Location = New System.Drawing.Point(12, 30)
        Me.dgvHistory.Name = "dgvHistory"
        Me.dgvHistory.Size = New System.Drawing.Size(876, 240)
        Me.dgvHistory.TabIndex = 1
        '
        'lblAuditCaption
        '
        Me.lblAuditCaption.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAuditCaption.AutoSize = True
        Me.lblAuditCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblAuditCaption.Location = New System.Drawing.Point(12, 282)
        Me.lblAuditCaption.Name = "lblAuditCaption"
        Me.lblAuditCaption.Size = New System.Drawing.Size(70, 13)
        Me.lblAuditCaption.TabIndex = 2
        Me.lblAuditCaption.Text = "Audit trail"
        '
        'dgvAudit
        '
        Me.dgvAudit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.dgvAudit.Location = New System.Drawing.Point(12, 300)
        Me.dgvAudit.Name = "dgvAudit"
        Me.dgvAudit.Size = New System.Drawing.Size(876, 274)
        Me.dgvAudit.TabIndex = 3
        '
        'frmClaim
        '
        Me.ClientSize = New System.Drawing.Size(908, 661)
        Me.Controls.Add(Me.tabClaim)
        Me.Controls.Add(Me.pnlHeader)
        Me.MinimumSize = New System.Drawing.Size(760, 520)
        Me.Name = "frmClaim"
        Me.Text = "Claim"
        Me.pnlHeader.ResumeLayout(False)
        Me.tabClaim.ResumeLayout(False)
        Me.tabDetails.ResumeLayout(False)
        Me.tabCorrespondence.ResumeLayout(False)
        Me.tabDocuments.ResumeLayout(False)
        Me.tabNotes.ResumeLayout(False)
        Me.tabPayments.ResumeLayout(False)
        Me.grpAddPayment.ResumeLayout(False)
        Me.tabHistory.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblRefCaption As System.Windows.Forms.Label
    Friend WithEvents lblRef As System.Windows.Forms.Label
    Friend WithEvents lblStatusCaption As System.Windows.Forms.Label
    Friend WithEvents lblStatusValue As System.Windows.Forms.Label
    Friend WithEvents btnAction As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents mnuActions As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tabClaim As System.Windows.Forms.TabControl
    Friend WithEvents tabDetails As System.Windows.Forms.TabPage
    Friend WithEvents lblPolicyNo As System.Windows.Forms.Label
    Friend WithEvents txtPolicyNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblInsuredName As System.Windows.Forms.Label
    Friend WithEvents txtInsuredName As System.Windows.Forms.TextBox
    Friend WithEvents lblPostcode As System.Windows.Forms.Label
    Friend WithEvents txtPostcode As System.Windows.Forms.TextBox
    Friend WithEvents lblPhone As System.Windows.Forms.Label
    Friend WithEvents txtPhone As System.Windows.Forms.TextBox
    Friend WithEvents lblEmail As System.Windows.Forms.Label
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents lblDateOfLoss As System.Windows.Forms.Label
    Friend WithEvents dtpDateOfLoss As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkLossDateKnown As System.Windows.Forms.CheckBox
    Friend WithEvents lblDateNotified As System.Windows.Forms.Label
    Friend WithEvents dtpDateNotified As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblPeril As System.Windows.Forms.Label
    Friend WithEvents cboPeril As System.Windows.Forms.ComboBox
    Friend WithEvents lblEstimate As System.Windows.Forms.Label
    Friend WithEvents txtEstimatedValue As System.Windows.Forms.TextBox
    Friend WithEvents lblReserve As System.Windows.Forms.Label
    Friend WithEvents txtReserve As System.Windows.Forms.TextBox
    Friend WithEvents lblSettled As System.Windows.Forms.Label
    Friend WithEvents txtSettled As System.Windows.Forms.TextBox
    Friend WithEvents lblHandler As System.Windows.Forms.Label
    Friend WithEvents cboHandler As System.Windows.Forms.ComboBox
    Friend WithEvents lblAdjuster As System.Windows.Forms.Label
    Friend WithEvents cboAdjuster As System.Windows.Forms.ComboBox
    Friend WithEvents lblUnderwriter As System.Windows.Forms.Label
    Friend WithEvents cboUnderwriter As System.Windows.Forms.ComboBox
    Friend WithEvents lblLossDescription As System.Windows.Forms.Label
    Friend WithEvents txtLossDescription As System.Windows.Forms.TextBox
    Friend WithEvents tabCorrespondence As System.Windows.Forms.TabPage
    Friend WithEvents dgvCorrespondence As System.Windows.Forms.DataGridView
    Friend WithEvents txtCorrPreview As System.Windows.Forms.TextBox
    Friend WithEvents lvCorrDocs As System.Windows.Forms.ListView
    Friend WithEvents colCorrDocFile As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCorrDocType As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnOpenCorrDoc As System.Windows.Forms.Button
    Friend WithEvents btnRecordInbound As System.Windows.Forms.Button
    Friend WithEvents tabDocuments As System.Windows.Forms.TabPage
    Friend WithEvents dgvDocs As System.Windows.Forms.DataGridView
    Friend WithEvents lblAddType As System.Windows.Forms.Label
    Friend WithEvents cboAddDocType As System.Windows.Forms.ComboBox
    Friend WithEvents btnAddDoc As System.Windows.Forms.Button
    Friend WithEvents btnOpenDoc As System.Windows.Forms.Button
    Friend WithEvents btnRemoveDoc As System.Windows.Forms.Button
    Friend WithEvents lblDocsNote As System.Windows.Forms.Label
    Friend WithEvents tabNotes As System.Windows.Forms.TabPage
    Friend WithEvents dgvNotes As System.Windows.Forms.DataGridView
    Friend WithEvents lblNewNote As System.Windows.Forms.Label
    Friend WithEvents txtNewNote As System.Windows.Forms.TextBox
    Friend WithEvents chkInternal As System.Windows.Forms.CheckBox
    Friend WithEvents btnAddNote As System.Windows.Forms.Button
    Friend WithEvents tabPayments As System.Windows.Forms.TabPage
    Friend WithEvents dgvPayments As System.Windows.Forms.DataGridView
    Friend WithEvents grpAddPayment As System.Windows.Forms.GroupBox
    Friend WithEvents lblPayType As System.Windows.Forms.Label
    Friend WithEvents cboPayType As System.Windows.Forms.ComboBox
    Friend WithEvents lblAmount As System.Windows.Forms.Label
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents lblPayee As System.Windows.Forms.Label
    Friend WithEvents txtPayee As System.Windows.Forms.TextBox
    Friend WithEvents lblPayRef As System.Windows.Forms.Label
    Friend WithEvents txtPayRef As System.Windows.Forms.TextBox
    Friend WithEvents lblPaidAt As System.Windows.Forms.Label
    Friend WithEvents dtpPaidAt As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnAddPayment As System.Windows.Forms.Button
    Friend WithEvents lblPaymentTotals As System.Windows.Forms.Label
    Friend WithEvents tabHistory As System.Windows.Forms.TabPage
    Friend WithEvents lblHistoryCaption As System.Windows.Forms.Label
    Friend WithEvents dgvHistory As System.Windows.Forms.DataGridView
    Friend WithEvents lblAuditCaption As System.Windows.Forms.Label
    Friend WithEvents dgvAudit As System.Windows.Forms.DataGridView
End Class
