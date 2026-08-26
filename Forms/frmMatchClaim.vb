Option Strict On
Imports System.Data
Imports System.Windows.Forms

''' <summary>
''' The decision the whole desk turns on: is this a new loss, or more paper for a claim we
''' already hold? The search is pre-filled from whatever could be scraped out of the item,
''' but a person makes the call.
''' </summary>
Public Class frmMatchClaim

    Private ReadOnly _correspondenceId As Integer
    Private _item As Correspondence

    Public Property MatchedClaimId As Integer

    Public Sub New()
        Me.New(0)
    End Sub

    Public Sub New(correspondenceId As Integer)
        InitializeComponent()
        UiStyle.ApplyTo(Me)
        UiStyle.StyleGrid(dgvCandidates)
        _correspondenceId = correspondenceId
    End Sub

    Private Sub frmMatchClaim_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            _item = CorrespondenceRepository.GetById(_correspondenceId)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try

        If _item Is Nothing Then
            UiStyle.ShowProblem(Me, "That inbound item could not be loaded. It may have been deleted.")
            btnAttach.Enabled = False
            btnCreateNew.Enabled = False
            Return
        End If

        ShowItem()
        PrefillFromItem()
        RunSearch()
    End Sub

    Private Sub ShowItem()
        lblItemFrom.Text = _item.FromName
        If _item.FromAddress.Length > 0 Then
            lblItemFrom.Text &= "  <" & _item.FromAddress & ">"
        End If
        lblItemReceived.Text = _item.Source & ", " & _item.ReceivedAt.ToString("dd/MM/yyyy HH:mm")
        lblItemSubject.Text = _item.Subject
        txtItemBody.Text = _item.Body
    End Sub

    ''' <summary>Fills the search boxes from identifiers found in the text.</summary>
    Private Sub PrefillFromItem()
        Dim hints As TextHints = TextHints.Extract(_item.Subject, _item.Body)
        Dim found As New List(Of String)()

        If hints.ClaimRefs.Count > 0 Then
            ' A claim reference in the text is as close to certain as this gets.
            txtPolicy.Text = String.Empty
            txtInsured.Text = String.Empty
            found.Add("claim reference " & hints.ClaimRefs(0))

            Try
                Dim direct As Claim = ClaimRepository.GetByRef(hints.ClaimRefs(0))
                If direct IsNot Nothing Then
                    txtPolicy.Text = direct.PolicyNumber
                End If
            Catch ex As Exception
                ' A failed lookup here is not worth interrupting the handler for.
            End Try
        End If

        If txtPolicy.TextLength = 0 AndAlso hints.FirstPolicyNumber.Length > 0 Then
            txtPolicy.Text = hints.FirstPolicyNumber
            found.Add("policy number " & hints.FirstPolicyNumber)
        End If

        Dim surname As String = TextHints.SurnameFrom(_item.FromName)
        If surname.Length > 2 Then
            txtInsured.Text = surname
            found.Add("sender name " & surname)
        End If

        Dim earliest As Date? = hints.EarliestDate
        If earliest.HasValue Then
            chkUseDate.Checked = True
            dtpLoss.Value = earliest.Value
            found.Add("date " & earliest.Value.ToString("dd/MM/yyyy"))
        End If

        lblHints.Text = If(found.Count = 0,
            "Nothing identifiable was found in the text. Search on whatever you can see above.",
            "Pre-filled from the item: " & String.Join(", ", found) & ". Adjust and search again if needed.")
    End Sub

    Private Sub RunSearch()
        Try
            Dim lossDate As Date? = Nothing
            If chkUseDate.Checked Then lossDate = dtpLoss.Value.Date

            Dim data As DataTable = ClaimRepository.FindPotentialDuplicates(
                txtPolicy.Text, txtInsured.Text, lossDate, CInt(numWindow.Value))

            dgvCandidates.DataSource = data
            UiStyle.HideColumn(dgvCandidates, "Id")
            UiStyle.FormatDateColumn(dgvCandidates, "Date of Loss")
            UiStyle.FormatDateColumn(dgvCandidates, "Notified")
            UiStyle.FormatCurrencyColumn(dgvCandidates, "Estimate")
            UiStyle.FormatCurrencyColumn(dgvCandidates, "Reserve")

            Select Case data.Rows.Count
                Case 0
                    lblResult.Text = "No existing claim matches. This looks like a new loss."
                Case 1
                    lblResult.Text = "One possible match. Check it before attaching."
                Case Else
                    lblResult.Text = data.Rows.Count.ToString() & " possible matches. Check each one."
            End Select
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Function SelectedClaimId() As Integer
        If dgvCandidates.CurrentRow Is Nothing Then Return 0
        Return Db.AsInt(dgvCandidates.CurrentRow.Cells("Id").Value)
    End Function

#Region "Actions"

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        RunSearch()
    End Sub

    Private Sub chkUseDate_CheckedChanged(sender As Object, e As EventArgs) _
        Handles chkUseDate.CheckedChanged
        dtpLoss.Enabled = chkUseDate.Checked
    End Sub

    Private Sub btnAttach_Click(sender As Object, e As EventArgs) Handles btnAttach.Click
        Dim claimId As Integer = SelectedClaimId()
        If claimId = 0 Then
            UiStyle.ShowProblem(Me, "Select the claim to attach this item to.")
            Return
        End If

        Dim claimRef As String = Db.AsString(dgvCandidates.CurrentRow.Cells("Claim Ref").Value)
        If MessageBox.Show(Me,
            "File this item against " & claimRef & "?" & vbCrLf & vbCrLf &
            "Any attachments will be moved into that claim's folder.",
            "Attach to claim", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            CorrespondenceRepository.MatchToClaim(_correspondenceId, claimId, Session.UserName)
            MatchedClaimId = claimId
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
            Return
        End Try

        If frmMain.Instance IsNot Nothing Then frmMain.Instance.RefreshIntakeCount()

        If MessageBox.Show(Me, "Filed against " & claimRef & ". Open the claim now?",
                           "Claims Manager", MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question) = DialogResult.Yes Then
            If frmMain.Instance IsNot Nothing Then frmMain.Instance.OpenClaim(claimId)
        End If

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCreateNew_Click(sender As Object, e As EventArgs) Handles btnCreateNew.Click
        If Not Session.IsInRole(UserRole.Handler, UserRole.Supervisor) Then
            UiStyle.ShowProblem(Me, "Only a handler or supervisor can open a new claim.")
            Return
        End If

        If dgvCandidates.Rows.Count > 0 Then
            If MessageBox.Show(Me,
                "There " & If(dgvCandidates.Rows.Count = 1, "is 1 possible match", "are " &
                dgvCandidates.Rows.Count.ToString() & " possible matches") &
                " in the list." & vbCrLf & vbCrLf &
                "Open a new claim anyway?",
                "Check for duplicates", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) <> DialogResult.Yes Then
                Return
            End If
        End If

        Dim newClaimId As Integer = 0
        Using claimForm As New frmClaim(_item)
            claimForm.CloseOnFirstSave = True
            If claimForm.ShowDialog(Me) = DialogResult.OK Then
                newClaimId = claimForm.ClaimId
            End If
        End Using

        If newClaimId = 0 Then Return

        Try
            CorrespondenceRepository.MatchToClaim(_correspondenceId, newClaimId, Session.UserName)
            MatchedClaimId = newClaimId
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try

        If frmMain.Instance IsNot Nothing Then
            frmMain.Instance.RefreshIntakeCount()
            frmMain.Instance.OpenClaim(newClaimId)
        End If

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub dgvCandidates_DoubleClick(sender As Object, e As EventArgs) _
        Handles dgvCandidates.DoubleClick
        Dim claimId As Integer = SelectedClaimId()
        If claimId = 0 Then Return
        If frmMain.Instance IsNot Nothing Then frmMain.Instance.OpenClaim(claimId)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

#End Region

End Class
