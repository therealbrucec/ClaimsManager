Option Strict On
Imports System.Data
Imports System.Globalization
Imports System.Windows.Forms

''' <summary>
''' One claim, all of it. The header carries the reference, the current status and the only
''' route to changing that status; the tabs hold the detail. Nothing here decides what a
''' claim may do next - it asks ClaimWorkflow and offers what comes back.
''' </summary>
Public Class frmClaim

    Private _claim As Claim
    Private ReadOnly _prefill As Correspondence
    Private _loading As Boolean

    Public ReadOnly Property ClaimId As Integer
        Get
            If _claim Is Nothing Then Return 0
            Return _claim.ClaimId
        End Get
    End Property

    ''' <summary>Set by the match screen: create the claim, then hand the id straight back.</summary>
    Public Property CloseOnFirstSave As Boolean

    Public Sub New()
        Me.New(CType(Nothing, Correspondence))
    End Sub

    ''' <summary>Opens an existing claim.</summary>
    Public Sub New(claimId As Integer)
        InitializeComponent()
        UiStyle.ApplyTo(Me)
        StyleGrids()

        _claim = ClaimRepository.GetById(claimId)
        If _claim Is Nothing Then
            Throw New ApplicationException("That claim could not be loaded. It may have been deleted.")
        End If
    End Sub

    ''' <summary>Starts a new claim, optionally pre-filled from an inbound item.</summary>
    Public Sub New(prefillFrom As Correspondence)
        InitializeComponent()
        UiStyle.ApplyTo(Me)
        StyleGrids()

        _claim = New Claim()
        _prefill = prefillFrom
    End Sub

    Private Sub StyleGrids()
        UiStyle.StyleGrid(dgvCorrespondence)
        UiStyle.StyleGrid(dgvDocs)
        UiStyle.StyleGrid(dgvNotes)
        UiStyle.StyleGrid(dgvPayments)
        UiStyle.StyleGrid(dgvHistory)
        UiStyle.StyleGrid(dgvAudit)
        txtCorrPreview.Font = UiStyle.MonoFont
    End Sub

#Region "Load and bind"

    Private Sub frmClaim_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            PopulateLookups()
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try

        If _claim.IsNew AndAlso _prefill IsNot Nothing Then ApplyPrefill()

        BindClaim()
        ApplyPermissions()

        If Not _claim.IsNew Then LoadAllTabs()
    End Sub

    Private Sub PopulateLookups()
        cboPeril.Items.Add("(not set)")
        cboPeril.Items.AddRange(Peril.All)

        Dim handlers As New List(Of AppUser)()
        handlers.Add(New AppUser With {.UserId = 0, .FullName = "(not assigned)"})
        handlers.AddRange(UserRepository.GetByRole(UserRole.Handler))
        handlers.AddRange(UserRepository.GetByRole(UserRole.Supervisor))
        cboHandler.DisplayMember = "FullName"
        cboHandler.ValueMember = "UserId"
        cboHandler.DataSource = handlers

        Dim adjusters As New List(Of AppUser)()
        adjusters.Add(New AppUser With {.UserId = 0, .FullName = "(not assigned)"})
        adjusters.AddRange(UserRepository.GetByRole(UserRole.Adjuster))
        cboAdjuster.DisplayMember = "FullName"
        cboAdjuster.ValueMember = "UserId"
        cboAdjuster.DataSource = adjusters

        Dim underwriters As New List(Of Underwriter)()
        underwriters.Add(New Underwriter With {.UnderwriterId = 0, .Name = "(not chosen)"})
        underwriters.AddRange(UnderwriterRepository.GetAll())
        cboUnderwriter.DisplayMember = "Name"
        cboUnderwriter.ValueMember = "UnderwriterId"
        cboUnderwriter.DataSource = underwriters

        cboAddDocType.Items.AddRange(DocType.All)
        cboAddDocType.SelectedItem = DocType.Photo

        cboPayType.Items.AddRange(PaymentType.All)
        cboPayType.SelectedItem = PaymentType.Interim
        dtpPaidAt.Value = Date.Today
    End Sub

    ''' <summary>Carries across whatever the inbound item already told us.</summary>
    Private Sub ApplyPrefill()
        _claim.InsuredName = _prefill.FromName
        _claim.DateNotified = _prefill.ReceivedAt.Date
        _claim.LossDescription = _prefill.Body

        If _prefill.Source = CorrespondenceSource.Email Then
            _claim.ContactEmail = _prefill.FromAddress
        ElseIf _prefill.Source = CorrespondenceSource.Phone OrElse
               _prefill.Source = CorrespondenceSource.Fax Then
            _claim.ContactPhone = _prefill.FromAddress
        End If

        Dim hints As TextHints = TextHints.Extract(_prefill.Subject, _prefill.Body)
        If hints.FirstPolicyNumber.Length > 0 Then _claim.PolicyNumber = hints.FirstPolicyNumber
        If hints.EarliestDate.HasValue Then _claim.DateOfLoss = hints.EarliestDate

        ' Whoever is filing it owns it until someone says otherwise.
        If Session.CurrentUser IsNot Nothing AndAlso
           Session.IsInRole(UserRole.Handler, UserRole.Supervisor) Then
            _claim.HandlerUserId = Session.CurrentUser.UserId
        End If
    End Sub

    Private Sub BindClaim()
        _loading = True
        Try
            txtPolicyNumber.Text = _claim.PolicyNumber
            txtInsuredName.Text = _claim.InsuredName
            txtPostcode.Text = _claim.InsuredPostcode
            txtPhone.Text = _claim.ContactPhone
            txtEmail.Text = _claim.ContactEmail

            chkLossDateKnown.Checked = _claim.DateOfLoss.HasValue
            dtpDateOfLoss.Value = If(_claim.DateOfLoss.HasValue, _claim.DateOfLoss.Value, Date.Today)
            dtpDateOfLoss.Enabled = chkLossDateKnown.Checked

            dtpDateNotified.Value = If(_claim.DateNotified = Date.MinValue,
                                       Date.Today, _claim.DateNotified)

            cboPeril.SelectedItem = _claim.Peril
            If cboPeril.SelectedIndex < 0 Then cboPeril.SelectedIndex = 0

            txtEstimatedValue.Text = Money(_claim.EstimatedValue)
            txtReserve.Text = Money(_claim.ReserveAmount)
            txtSettled.Text = Money(_claim.SettledAmount)

            SelectValue(cboHandler, _claim.HandlerUserId)
            SelectValue(cboAdjuster, _claim.AdjusterUserId)
            SelectValue(cboUnderwriter, _claim.UnderwriterId)

            txtLossDescription.Text = _claim.LossDescription

            RefreshHeader()
        Finally
            _loading = False
        End Try
    End Sub

    Private Sub RefreshHeader()
        If _claim.IsNew Then
            lblRef.Text = "New claim"
            lblStatusValue.Text = "Not saved yet"
            lblStatusValue.ForeColor = SystemColors.GrayText
            Text = "New claim"
        Else
            lblRef.Text = _claim.ClaimRef
            lblStatusValue.Text = _claim.Status
            lblStatusValue.ForeColor = UiStyle.ColourFor(_claim.Status)
            Text = _claim.ClaimRef & " - " & _claim.InsuredName
        End If

        BuildActionMenu()

        ' Nothing can be attached to a claim that has no reference yet.
        Dim saved As Boolean = Not _claim.IsNew
        tabCorrespondence.Enabled = saved
        tabDocuments.Enabled = saved
        tabNotes.Enabled = saved
        tabPayments.Enabled = saved
        tabHistory.Enabled = saved
    End Sub

    Private Sub ApplyPermissions()
        ' An adjuster reviews; they do not rewrite the file.
        Dim canEdit As Boolean = Session.IsInRole(UserRole.Handler, UserRole.Supervisor)

        For Each c As Control In New Control() {txtPolicyNumber, txtInsuredName, txtPostcode,
                                                txtPhone, txtEmail, txtEstimatedValue,
                                                txtReserve, txtLossDescription}
            DirectCast(c, TextBox).ReadOnly = Not canEdit
        Next

        cboPeril.Enabled = canEdit
        cboHandler.Enabled = canEdit
        cboAdjuster.Enabled = canEdit
        cboUnderwriter.Enabled = canEdit
        dtpDateNotified.Enabled = canEdit
        chkLossDateKnown.Enabled = canEdit
        If Not canEdit Then dtpDateOfLoss.Enabled = False

        btnSave.Enabled = canEdit
        btnRecordInbound.Enabled = canEdit
        btnAddDoc.Enabled = canEdit
        btnRemoveDoc.Enabled = canEdit
        grpAddPayment.Enabled = canEdit
    End Sub

    Private Sub LoadAllTabs()
        LoadCorrespondence()
        LoadDocuments()
        LoadNotes()
        LoadPayments()
        LoadHistory()
    End Sub

#End Region

#Region "Saving"

    Private Function CollectFromForm() As String
        If String.IsNullOrWhiteSpace(txtInsuredName.Text) Then
            Return "Enter the name of the insured."
        End If

        Dim estimate As Decimal
        If Not TryMoney(txtEstimatedValue.Text, estimate) Then
            Return "The estimated value is not a number. Enter it as digits, for example 8400.00."
        End If

        Dim reserve As Decimal
        If Not TryMoney(txtReserve.Text, reserve) Then
            Return "The reserve is not a number. Enter it as digits, for example 9000.00."
        End If

        If chkLossDateKnown.Checked AndAlso dtpDateOfLoss.Value.Date > Date.Today Then
            Return "The date of loss is in the future. Correct it before saving."
        End If

        _claim.PolicyNumber = txtPolicyNumber.Text.Trim()
        _claim.InsuredName = txtInsuredName.Text.Trim()
        _claim.InsuredPostcode = txtPostcode.Text.Trim()
        _claim.ContactPhone = txtPhone.Text.Trim()
        _claim.ContactEmail = txtEmail.Text.Trim()
        _claim.DateOfLoss = If(chkLossDateKnown.Checked,
                               CType(dtpDateOfLoss.Value.Date, Date?), Nothing)
        _claim.DateNotified = dtpDateNotified.Value.Date
        _claim.Peril = If(cboPeril.SelectedIndex > 0,
                          Convert.ToString(cboPeril.SelectedItem), String.Empty)
        _claim.LossDescription = txtLossDescription.Text
        _claim.EstimatedValue = estimate
        _claim.ReserveAmount = reserve
        _claim.HandlerUserId = ValueOrNothing(cboHandler)
        _claim.AdjusterUserId = ValueOrNothing(cboAdjuster)
        _claim.UnderwriterId = ValueOrNothing(cboUnderwriter)

        Return String.Empty
    End Function

    Private Function SaveClaim() As Boolean
        Dim problem As String = CollectFromForm()
        If problem.Length > 0 Then
            UiStyle.ShowProblem(Me, problem)
            Return False
        End If

        Try
            If _claim.IsNew Then
                ClaimRepository.Insert(_claim, Session.UserName)
                If frmMain.Instance IsNot Nothing Then Tag = _claim.ClaimId.ToString()
            Else
                ClaimRepository.Update(_claim, Session.UserName)
            End If
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
            Return False
        End Try

        RefreshHeader()
        Return True
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim wasNew As Boolean = _claim.IsNew
        If Not SaveClaim() Then Return

        If wasNew AndAlso CloseOnFirstSave Then
            DialogResult = DialogResult.OK
            Close()
            Return
        End If

        If wasNew Then LoadAllTabs()

        MessageBox.Show(Me, "Saved as " & _claim.ClaimRef & ".", "Claims Manager",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

#End Region

#Region "Workflow"

    Private Sub BuildActionMenu()
        mnuActions.Items.Clear()

        If _claim.IsNew Then
            btnAction.Enabled = False
            Return
        End If

        Dim available As List(Of WorkflowTransition) =
            ClaimWorkflow.AvailableFor(_claim.Status, Session.Role)

        If available.Count = 0 Then
            btnAction.Enabled = False
            btnAction.Text = "No action available"
            Return
        End If

        btnAction.Enabled = True
        btnAction.Text = "Move claim on..."

        For Each transition As WorkflowTransition In available
            Dim item As New ToolStripMenuItem(transition.Caption)
            item.Tag = transition
            AddHandler item.Click, AddressOf OnTransitionChosen
            mnuActions.Items.Add(item)
        Next
    End Sub

    Private Sub btnAction_Click(sender As Object, e As EventArgs) Handles btnAction.Click
        If mnuActions.Items.Count = 0 Then Return
        mnuActions.Show(btnAction, New Point(0, btnAction.Height))
    End Sub

    Private Sub OnTransitionChosen(sender As Object, e As EventArgs)
        Dim item As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        If item Is Nothing Then Return

        Dim transition As WorkflowTransition = TryCast(item.Tag, WorkflowTransition)
        If transition Is Nothing Then Return

        ' Unsaved edits on the details tab would otherwise be lost by the reload below.
        If CollectFromForm().Length = 0 Then
            Try
                ClaimRepository.Update(_claim, Session.UserName)
            Catch ex As Exception
                UiStyle.ShowError(Me, ex)
                Return
            End Try
        End If

        Dim blocked As String = ClaimWorkflow.Validate(_claim, transition.ToStatus)
        If blocked.Length > 0 Then
            UiStyle.ShowProblem(Me, blocked)
            Return
        End If

        Dim rationale As String
        Using dialog As New frmTransition(_claim.Status, transition)
            If dialog.ShowDialog(Me) <> DialogResult.OK Then Return
            rationale = dialog.Rationale
        End Using

        Try
            ClaimWorkflow.Apply(_claim, transition, rationale, Session.UserName)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
            Return
        End Try

        BindClaim()
        LoadHistory()
        LoadPayments()
    End Sub

#End Region

#Region "Correspondence tab"

    Private Sub LoadCorrespondence()
        Try
            dgvCorrespondence.DataSource = CorrespondenceRepository.GetByClaim(_claim.ClaimId)
            UiStyle.HideColumn(dgvCorrespondence, "Id")
            UiStyle.FormatDateColumn(dgvCorrespondence, "Received", True)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub dgvCorrespondence_SelectionChanged(sender As Object, e As EventArgs) _
        Handles dgvCorrespondence.SelectionChanged
        lvCorrDocs.Items.Clear()
        txtCorrPreview.Clear()

        If dgvCorrespondence.CurrentRow Is Nothing Then Return
        Dim id As Integer = Db.AsInt(dgvCorrespondence.CurrentRow.Cells("Id").Value)
        If id = 0 Then Return

        Try
            Dim item As Correspondence = CorrespondenceRepository.GetById(id)
            If item Is Nothing Then Return

            txtCorrPreview.Text =
                "Arrived by : " & item.Source & vbCrLf &
                "Received   : " & item.ReceivedAt.ToString("dd/MM/yyyy HH:mm") & vbCrLf &
                "From       : " & item.FromName & "  " & item.FromAddress & vbCrLf &
                "Subject    : " & item.Subject & vbCrLf &
                New String("-"c, 60) & vbCrLf & vbCrLf & item.Body

            For Each row As DataRow In AttachmentRepository.GetByCorrespondence(id).Rows
                Dim entry As New ListViewItem(Db.AsString(row("File")))
                entry.SubItems.Add(Db.AsString(row("Type")))
                entry.Tag = Db.AsString(row("StoredPath"))
                lvCorrDocs.Items.Add(entry)
            Next
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub btnRecordInbound_Click(sender As Object, e As EventArgs) Handles btnRecordInbound.Click
        Using intake As New frmIntake(_claim.ClaimId, _claim.ClaimRef)
            intake.ShowDialog(Me)
        End Using
        LoadCorrespondence()
        LoadDocuments()
    End Sub

    Private Sub btnOpenCorrDoc_Click(sender As Object, e As EventArgs) Handles btnOpenCorrDoc.Click
        If lvCorrDocs.SelectedItems.Count = 0 Then
            UiStyle.ShowProblem(Me, "Select an attachment to open it.")
            Return
        End If

        Try
            DocumentStore.OpenDocument(Convert.ToString(lvCorrDocs.SelectedItems(0).Tag))
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub lvCorrDocs_DoubleClick(sender As Object, e As EventArgs) Handles lvCorrDocs.DoubleClick
        btnOpenCorrDoc_Click(sender, e)
    End Sub

#End Region

#Region "Documents tab"

    Private Sub LoadDocuments()
        Try
            dgvDocs.DataSource = AttachmentRepository.GetByClaim(_claim.ClaimId)
            UiStyle.HideColumn(dgvDocs, "Id")
            UiStyle.HideColumn(dgvDocs, "StoredPath")
            UiStyle.FormatDateColumn(dgvDocs, "Added", True)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub btnAddDoc_Click(sender As Object, e As EventArgs) Handles btnAddDoc.Click
        Dim documentType As String = Convert.ToString(cboAddDocType.SelectedItem)
        If String.IsNullOrEmpty(documentType) Then documentType = DocType.Other

        Using picker As New OpenFileDialog()
            picker.Title = "Add documents to " & _claim.ClaimRef
            picker.Multiselect = True
            picker.Filter = "All files (*.*)|*.*"

            If picker.ShowDialog(Me) <> DialogResult.OK Then Return

            Dim failed As New List(Of String)()
            For Each chosen As String In picker.FileNames
                Try
                    AttachmentRepository.Import(chosen, documentType, _claim.ClaimId,
                                                _claim.ClaimRef, Nothing, Session.UserName)
                Catch ex As Exception
                    failed.Add(ex.Message)
                End Try
            Next

            If failed.Count > 0 Then
                UiStyle.ShowProblem(Me, "Some files were not added:" & vbCrLf & vbCrLf &
                                        String.Join(vbCrLf, failed))
            End If
        End Using

        LoadDocuments()
    End Sub

    Private Function SelectedDocumentPath() As String
        If dgvDocs.CurrentRow Is Nothing Then Return String.Empty
        Return Db.AsString(dgvDocs.CurrentRow.Cells("StoredPath").Value)
    End Function

    Private Sub btnOpenDoc_Click(sender As Object, e As EventArgs) Handles btnOpenDoc.Click
        Dim relativePath As String = SelectedDocumentPath()
        If relativePath.Length = 0 Then
            UiStyle.ShowProblem(Me, "Select a document in the list to open it.")
            Return
        End If

        Try
            DocumentStore.OpenDocument(relativePath)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub dgvDocs_DoubleClick(sender As Object, e As EventArgs) Handles dgvDocs.DoubleClick
        btnOpenDoc_Click(sender, e)
    End Sub

    Private Sub btnRemoveDoc_Click(sender As Object, e As EventArgs) Handles btnRemoveDoc.Click
        If dgvDocs.CurrentRow Is Nothing Then
            UiStyle.ShowProblem(Me, "Select a document to take off this claim.")
            Return
        End If

        Dim attachmentId As Integer = Db.AsInt(dgvDocs.CurrentRow.Cells("Id").Value)
        Dim fileName As String = Db.AsString(dgvDocs.CurrentRow.Cells("File").Value)

        If MessageBox.Show(Me,
            "Take " & fileName & " off this claim?" & vbCrLf & vbCrLf &
            "The file itself stays in the document store.",
            "Remove document", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            AttachmentRepository.Delete(attachmentId, Session.UserName)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try

        LoadDocuments()
    End Sub

#End Region

#Region "Notes tab"

    Private Sub LoadNotes()
        Try
            dgvNotes.DataSource = NoteRepository.GetByClaim(_claim.ClaimId)
            UiStyle.FormatDateColumn(dgvNotes, "When", True)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub btnAddNote_Click(sender As Object, e As EventArgs) Handles btnAddNote.Click
        If txtNewNote.TextLength = 0 Then
            UiStyle.ShowProblem(Me, "Type the note before adding it.")
            Return
        End If

        Try
            NoteRepository.Insert(_claim.ClaimId, txtNewNote.Text.Trim(),
                                  chkInternal.Checked, Session.UserName)
            txtNewNote.Clear()
            LoadNotes()
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

#End Region

#Region "Payments tab"

    Private Sub LoadPayments()
        Try
            dgvPayments.DataSource = PaymentRepository.GetByClaim(_claim.ClaimId)
            UiStyle.HideColumn(dgvPayments, "Id")
            UiStyle.FormatDateColumn(dgvPayments, "Paid")
            UiStyle.FormatCurrencyColumn(dgvPayments, "Amount")

            Dim total As Decimal = PaymentRepository.TotalForClaim(_claim.ClaimId)
            txtSettled.Text = Money(total)
            lblPaymentTotals.Text = "Net paid " & Money(total) &
                                    "   Reserve " & Money(_claim.ReserveAmount) &
                                    "   Outstanding " & Money(_claim.ReserveAmount - total)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub btnAddPayment_Click(sender As Object, e As EventArgs) Handles btnAddPayment.Click
        Dim amount As Decimal
        If Not TryMoney(txtAmount.Text, amount) OrElse amount <= 0D Then
            UiStyle.ShowProblem(Me, "Enter the payment amount as digits, for example 1250.00.")
            txtAmount.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtPayee.Text) Then
            UiStyle.ShowProblem(Me, "Enter who the payment goes to.")
            txtPayee.Focus()
            Return
        End If

        Dim payment As New Payment With {
            .ClaimId = _claim.ClaimId,
            .PaymentKind = Convert.ToString(cboPayType.SelectedItem),
            .Amount = amount,
            .Payee = txtPayee.Text.Trim(),
            .Reference = txtPayRef.Text.Trim(),
            .PaidAt = dtpPaidAt.Value.Date
        }

        Try
            PaymentRepository.Insert(payment, Session.UserName)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
            Return
        End Try

        txtAmount.Clear()
        txtPayee.Clear()
        txtPayRef.Clear()

        _claim.SettledAmount = PaymentRepository.TotalForClaim(_claim.ClaimId)
        LoadPayments()
    End Sub

#End Region

#Region "History tab"

    Private Sub LoadHistory()
        Try
            dgvHistory.DataSource = StatusHistoryRepository.GetByClaim(_claim.ClaimId)
            UiStyle.FormatDateColumn(dgvHistory, "When", True)

            dgvAudit.DataSource = AuditRepository.GetForClaim(_claim.ClaimId)
            UiStyle.FormatDateColumn(dgvAudit, "When", True)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

#End Region

#Region "Small helpers"

    Private Sub chkLossDateKnown_CheckedChanged(sender As Object, e As EventArgs) _
        Handles chkLossDateKnown.CheckedChanged
        If _loading Then Return
        dtpDateOfLoss.Enabled = chkLossDateKnown.Checked AndAlso
                                Session.IsInRole(UserRole.Handler, UserRole.Supervisor)
    End Sub

    Private Shared Function Money(value As Decimal) As String
        Return value.ToString("N2", CultureInfo.CurrentCulture)
    End Function

    ''' <summary>Accepts 8400, 8,400.00 and £8400 alike. Blank counts as zero.</summary>
    Private Shared Function TryMoney(text As String, ByRef value As Decimal) As Boolean
        If String.IsNullOrWhiteSpace(text) Then
            value = 0D
            Return True
        End If

        Return Decimal.TryParse(text.Trim(),
                                NumberStyles.Currency Or NumberStyles.AllowThousands,
                                CultureInfo.CurrentCulture, value)
    End Function

    Private Shared Sub SelectValue(combo As ComboBox, id As Integer?)
        combo.SelectedValue = If(id.HasValue, id.Value, 0)
        If combo.SelectedIndex < 0 AndAlso combo.Items.Count > 0 Then combo.SelectedIndex = 0
    End Sub

    Private Shared Function ValueOrNothing(combo As ComboBox) As Integer?
        If combo.SelectedValue Is Nothing Then Return Nothing
        Dim id As Integer = Db.AsInt(combo.SelectedValue)
        If id = 0 Then Return Nothing
        Return id
    End Function

#End Region

End Class
