Option Strict On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMatchClaim
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
        Me.grpItem = New System.Windows.Forms.GroupBox()
        Me.lblFromCaption = New System.Windows.Forms.Label()
        Me.lblItemFrom = New System.Windows.Forms.Label()
        Me.lblReceivedCaption = New System.Windows.Forms.Label()
        Me.lblItemReceived = New System.Windows.Forms.Label()
        Me.lblSubjectCaption = New System.Windows.Forms.Label()
        Me.lblItemSubject = New System.Windows.Forms.Label()
        Me.txtItemBody = New System.Windows.Forms.TextBox()
        Me.grpSearch = New System.Windows.Forms.GroupBox()
        Me.lblPolicy = New System.Windows.Forms.Label()
        Me.txtPolicy = New System.Windows.Forms.TextBox()
        Me.lblInsured = New System.Windows.Forms.Label()
        Me.txtInsured = New System.Windows.Forms.TextBox()
        Me.chkUseDate = New System.Windows.Forms.CheckBox()
        Me.dtpLoss = New System.Windows.Forms.DateTimePicker()
        Me.lblWindow = New System.Windows.Forms.Label()
        Me.numWindow = New System.Windows.Forms.NumericUpDown()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.lblHints = New System.Windows.Forms.Label()
        Me.dgvCandidates = New System.Windows.Forms.DataGridView()
        Me.btnAttach = New System.Windows.Forms.Button()
        Me.btnCreateNew = New System.Windows.Forms.Button()
        Me.lblResult = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grpItem.SuspendLayout()
        Me.grpSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpItem
        '
        Me.grpItem.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.grpItem.Controls.Add(Me.lblFromCaption)
        Me.grpItem.Controls.Add(Me.lblItemFrom)
        Me.grpItem.Controls.Add(Me.lblReceivedCaption)
        Me.grpItem.Controls.Add(Me.lblItemReceived)
        Me.grpItem.Controls.Add(Me.lblSubjectCaption)
        Me.grpItem.Controls.Add(Me.lblItemSubject)
        Me.grpItem.Controls.Add(Me.txtItemBody)
        Me.grpItem.Location = New System.Drawing.Point(12, 12)
        Me.grpItem.Name = "grpItem"
        Me.grpItem.Size = New System.Drawing.Size(784, 150)
        Me.grpItem.TabIndex = 0
        Me.grpItem.TabStop = False
        Me.grpItem.Text = "The item you are filing"
        '
        'lblFromCaption
        '
        Me.lblFromCaption.AutoSize = True
        Me.lblFromCaption.Location = New System.Drawing.Point(12, 24)
        Me.lblFromCaption.Name = "lblFromCaption"
        Me.lblFromCaption.Size = New System.Drawing.Size(30, 13)
        Me.lblFromCaption.TabIndex = 0
        Me.lblFromCaption.Text = "From"
        '
        'lblItemFrom
        '
        Me.lblItemFrom.AutoEllipsis = True
        Me.lblItemFrom.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblItemFrom.Location = New System.Drawing.Point(90, 24)
        Me.lblItemFrom.Name = "lblItemFrom"
        Me.lblItemFrom.Size = New System.Drawing.Size(300, 14)
        Me.lblItemFrom.TabIndex = 1
        '
        'lblReceivedCaption
        '
        Me.lblReceivedCaption.AutoSize = True
        Me.lblReceivedCaption.Location = New System.Drawing.Point(410, 24)
        Me.lblReceivedCaption.Name = "lblReceivedCaption"
        Me.lblReceivedCaption.Size = New System.Drawing.Size(52, 13)
        Me.lblReceivedCaption.TabIndex = 2
        Me.lblReceivedCaption.Text = "Received"
        '
        'lblItemReceived
        '
        Me.lblItemReceived.AutoSize = True
        Me.lblItemReceived.Location = New System.Drawing.Point(478, 24)
        Me.lblItemReceived.Name = "lblItemReceived"
        Me.lblItemReceived.Size = New System.Drawing.Size(0, 13)
        Me.lblItemReceived.TabIndex = 3
        '
        'lblSubjectCaption
        '
        Me.lblSubjectCaption.AutoSize = True
        Me.lblSubjectCaption.Location = New System.Drawing.Point(12, 46)
        Me.lblSubjectCaption.Name = "lblSubjectCaption"
        Me.lblSubjectCaption.Size = New System.Drawing.Size(43, 13)
        Me.lblSubjectCaption.TabIndex = 4
        Me.lblSubjectCaption.Text = "Subject"
        '
        'lblItemSubject
        '
        Me.lblItemSubject.AutoEllipsis = True
        Me.lblItemSubject.Location = New System.Drawing.Point(90, 46)
        Me.lblItemSubject.Name = "lblItemSubject"
        Me.lblItemSubject.Size = New System.Drawing.Size(680, 14)
        Me.lblItemSubject.TabIndex = 5
        '
        'txtItemBody
        '
        Me.txtItemBody.BackColor = System.Drawing.SystemColors.Control
        Me.txtItemBody.Location = New System.Drawing.Point(12, 68)
        Me.txtItemBody.Multiline = True
        Me.txtItemBody.Name = "txtItemBody"
        Me.txtItemBody.ReadOnly = True
        Me.txtItemBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtItemBody.Size = New System.Drawing.Size(758, 72)
        Me.txtItemBody.TabIndex = 6
        '
        'grpSearch
        '
        Me.grpSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.grpSearch.Controls.Add(Me.lblPolicy)
        Me.grpSearch.Controls.Add(Me.txtPolicy)
        Me.grpSearch.Controls.Add(Me.lblInsured)
        Me.grpSearch.Controls.Add(Me.txtInsured)
        Me.grpSearch.Controls.Add(Me.chkUseDate)
        Me.grpSearch.Controls.Add(Me.dtpLoss)
        Me.grpSearch.Controls.Add(Me.lblWindow)
        Me.grpSearch.Controls.Add(Me.numWindow)
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Location = New System.Drawing.Point(12, 168)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(784, 80)
        Me.grpSearch.TabIndex = 1
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "Look for an existing claim"
        '
        'lblPolicy
        '
        Me.lblPolicy.AutoSize = True
        Me.lblPolicy.Location = New System.Drawing.Point(12, 28)
        Me.lblPolicy.Name = "lblPolicy"
        Me.lblPolicy.Size = New System.Drawing.Size(55, 13)
        Me.lblPolicy.TabIndex = 0
        Me.lblPolicy.Text = "&Policy no"
        '
        'txtPolicy
        '
        Me.txtPolicy.Location = New System.Drawing.Point(90, 25)
        Me.txtPolicy.Name = "txtPolicy"
        Me.txtPolicy.Size = New System.Drawing.Size(160, 20)
        Me.txtPolicy.TabIndex = 1
        '
        'lblInsured
        '
        Me.lblInsured.AutoSize = True
        Me.lblInsured.Location = New System.Drawing.Point(266, 28)
        Me.lblInsured.Name = "lblInsured"
        Me.lblInsured.Size = New System.Drawing.Size(43, 13)
        Me.lblInsured.TabIndex = 2
        Me.lblInsured.Text = "&Insured"
        '
        'txtInsured
        '
        Me.txtInsured.Location = New System.Drawing.Point(340, 25)
        Me.txtInsured.Name = "txtInsured"
        Me.txtInsured.Size = New System.Drawing.Size(200, 20)
        Me.txtInsured.TabIndex = 3
        '
        'chkUseDate
        '
        Me.chkUseDate.AutoSize = True
        Me.chkUseDate.Location = New System.Drawing.Point(556, 27)
        Me.chkUseDate.Name = "chkUseDate"
        Me.chkUseDate.Size = New System.Drawing.Size(88, 17)
        Me.chkUseDate.TabIndex = 4
        Me.chkUseDate.Text = "&Date of loss"
        '
        'dtpLoss
        '
        Me.dtpLoss.CustomFormat = "dd/MM/yyyy"
        Me.dtpLoss.Enabled = False
        Me.dtpLoss.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpLoss.Location = New System.Drawing.Point(556, 48)
        Me.dtpLoss.Name = "dtpLoss"
        Me.dtpLoss.Size = New System.Drawing.Size(110, 20)
        Me.dtpLoss.TabIndex = 5
        '
        'lblWindow
        '
        Me.lblWindow.AutoSize = True
        Me.lblWindow.Location = New System.Drawing.Point(674, 51)
        Me.lblWindow.Name = "lblWindow"
        Me.lblWindow.Size = New System.Drawing.Size(43, 13)
        Me.lblWindow.TabIndex = 6
        Me.lblWindow.Text = "+/- days"
        '
        'numWindow
        '
        Me.numWindow.Location = New System.Drawing.Point(723, 48)
        Me.numWindow.Maximum = New Decimal(New Integer() {90, 0, 0, 0})
        Me.numWindow.Name = "numWindow"
        Me.numWindow.Size = New System.Drawing.Size(48, 20)
        Me.numWindow.TabIndex = 7
        Me.numWindow.Value = New Decimal(New Integer() {7, 0, 0, 0})
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(671, 21)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 23)
        Me.btnSearch.TabIndex = 8
        Me.btnSearch.Text = "Search"
        '
        'lblHints
        '
        Me.lblHints.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.lblHints.AutoEllipsis = True
        Me.lblHints.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblHints.Location = New System.Drawing.Point(14, 253)
        Me.lblHints.Name = "lblHints"
        Me.lblHints.Size = New System.Drawing.Size(782, 15)
        Me.lblHints.TabIndex = 2
        '
        'dgvCandidates
        '
        Me.dgvCandidates.Anchor = CType(((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.dgvCandidates.Location = New System.Drawing.Point(12, 272)
        Me.dgvCandidates.Name = "dgvCandidates"
        Me.dgvCandidates.Size = New System.Drawing.Size(784, 282)
        Me.dgvCandidates.TabIndex = 3
        '
        'btnAttach
        '
        Me.btnAttach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAttach.Location = New System.Drawing.Point(12, 564)
        Me.btnAttach.Name = "btnAttach"
        Me.btnAttach.Size = New System.Drawing.Size(180, 23)
        Me.btnAttach.TabIndex = 4
        Me.btnAttach.Text = "Attach to selected claim"
        '
        'btnCreateNew
        '
        Me.btnCreateNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCreateNew.Location = New System.Drawing.Point(198, 564)
        Me.btnCreateNew.Name = "btnCreateNew"
        Me.btnCreateNew.Size = New System.Drawing.Size(190, 23)
        Me.btnCreateNew.TabIndex = 5
        Me.btnCreateNew.Text = "Create new claim from this item"
        '
        'lblResult
        '
        Me.lblResult.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblResult.AutoEllipsis = True
        Me.lblResult.Location = New System.Drawing.Point(396, 569)
        Me.lblResult.Name = "lblResult"
        Me.lblResult.Size = New System.Drawing.Size(300, 15)
        Me.lblResult.TabIndex = 6
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(706, 564)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Close"
        '
        'frmMatchClaim
        '
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(808, 599)
        Me.Controls.Add(Me.grpItem)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.lblHints)
        Me.Controls.Add(Me.dgvCandidates)
        Me.Controls.Add(Me.btnAttach)
        Me.Controls.Add(Me.btnCreateNew)
        Me.Controls.Add(Me.lblResult)
        Me.Controls.Add(Me.btnCancel)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(824, 560)
        Me.Name = "frmMatchClaim"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "New claim or existing claim?"
        Me.grpItem.ResumeLayout(False)
        Me.grpSearch.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents grpItem As System.Windows.Forms.GroupBox
    Friend WithEvents lblFromCaption As System.Windows.Forms.Label
    Friend WithEvents lblItemFrom As System.Windows.Forms.Label
    Friend WithEvents lblReceivedCaption As System.Windows.Forms.Label
    Friend WithEvents lblItemReceived As System.Windows.Forms.Label
    Friend WithEvents lblSubjectCaption As System.Windows.Forms.Label
    Friend WithEvents lblItemSubject As System.Windows.Forms.Label
    Friend WithEvents txtItemBody As System.Windows.Forms.TextBox
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents lblPolicy As System.Windows.Forms.Label
    Friend WithEvents txtPolicy As System.Windows.Forms.TextBox
    Friend WithEvents lblInsured As System.Windows.Forms.Label
    Friend WithEvents txtInsured As System.Windows.Forms.TextBox
    Friend WithEvents chkUseDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpLoss As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblWindow As System.Windows.Forms.Label
    Friend WithEvents numWindow As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lblHints As System.Windows.Forms.Label
    Friend WithEvents dgvCandidates As System.Windows.Forms.DataGridView
    Friend WithEvents btnAttach As System.Windows.Forms.Button
    Friend WithEvents btnCreateNew As System.Windows.Forms.Button
    Friend WithEvents lblResult As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
