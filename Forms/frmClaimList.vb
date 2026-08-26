Option Strict On
Imports System.Data
Imports System.Windows.Forms

Public Enum ClaimListMode
    All = 0
    MineAsHandler = 1
    AdjusterQueue = 2
End Enum

Public Class frmClaimList

    Private ReadOnly _mode As ClaimListMode
    Private ReadOnly _userId As Integer?

    Public Sub New()
        Me.New("All claims", ClaimListMode.All, Nothing)
    End Sub

    Public Sub New(title As String, mode As ClaimListMode, userId As Integer?)
        InitializeComponent()
        UiStyle.ApplyTo(Me)
        UiStyle.StyleGrid(dgvClaims)

        _mode = mode
        _userId = userId
        Text = title
    End Sub

    Private Sub frmClaimList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboStatus.Items.Add("(any status)")
        cboStatus.Items.AddRange(ClaimStatus.All)
        cboStatus.SelectedIndex = 0

        ' The adjuster queue is defined by status, so the filters would only confuse.
        If _mode = ClaimListMode.AdjusterQueue Then
            cboStatus.Enabled = False
            chkOpenOnly.Enabled = False
        End If

        btnNew.Enabled = Session.IsInRole(UserRole.Handler, UserRole.Supervisor)

        LoadClaims()
    End Sub

    Public Sub LoadClaims()
        Try
            Dim data As DataTable

            If _mode = ClaimListMode.AdjusterQueue Then
                data = ClaimRepository.AdjusterQueue(_userId)
            Else
                Dim status As String = String.Empty
                If cboStatus.SelectedIndex > 0 Then status = Convert.ToString(cboStatus.SelectedItem)

                Dim handlerId As Integer? = Nothing
                If _mode = ClaimListMode.MineAsHandler Then handlerId = _userId

                data = ClaimRepository.Search(txtSearch.Text, status,
                                              chkOpenOnly.Checked, handlerId, Nothing)
            End If

            dgvClaims.DataSource = data
            UiStyle.HideColumn(dgvClaims, "Id")
            UiStyle.FormatDateColumn(dgvClaims, "Date of Loss")
            UiStyle.FormatDateColumn(dgvClaims, "Notified")
            UiStyle.FormatCurrencyColumn(dgvClaims, "Estimate")
            UiStyle.FormatCurrencyColumn(dgvClaims, "Reserve")

            lblCount.Text = If(data.Rows.Count = 0,
                "Nothing matches those filters.",
                data.Rows.Count.ToString() & " claim(s)")

            ReportHiddenClaims(data.Rows.Count)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Says how many claims the "open only" tick is holding back. Most of a year-old book is
    ''' settled or closed, so without this the list looks far emptier than the database is.
    ''' </summary>
    Private Sub ReportHiddenClaims(shownCount As Integer)
        If _mode = ClaimListMode.AdjusterQueue Then Return
        If Not chkOpenOnly.Checked Then Return
        If cboStatus.SelectedIndex > 0 Then Return

        Try
            Dim handlerId As Integer? = Nothing
            If _mode = ClaimListMode.MineAsHandler Then handlerId = _userId

            Dim everything As DataTable = ClaimRepository.Search(
                txtSearch.Text, String.Empty, False, handlerId, Nothing)

            Dim hidden As Integer = everything.Rows.Count - shownCount
            If hidden <= 0 Then Return

            lblCount.Text = shownCount.ToString() & " open claim(s), " &
                            hidden.ToString() & " concluded and hidden. " &
                            "Untick 'Open claims only' to include them."
        Catch ex As Exception
            ' The count is a convenience; losing it must not break the list.
        End Try
    End Sub

    ''' <summary>Tints the status cell so a full queue can be triaged without reading it.</summary>
    Private Sub dgvClaims_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) _
        Handles dgvClaims.CellFormatting
        If e.RowIndex < 0 Then Return
        If dgvClaims.Columns(e.ColumnIndex).Name <> "Status" Then Return

        e.CellStyle.ForeColor = UiStyle.ColourFor(Convert.ToString(e.Value))
        e.CellStyle.Font = UiStyle.HeadingFont
    End Sub

    Private Function SelectedClaimId() As Integer
        If dgvClaims.CurrentRow Is Nothing Then Return 0
        Return Db.AsInt(dgvClaims.CurrentRow.Cells("Id").Value)
    End Function

    Private Sub OpenSelected()
        Dim claimId As Integer = SelectedClaimId()
        If claimId = 0 Then
            UiStyle.ShowProblem(Me, "Select a claim in the list first.")
            Return
        End If
        If frmMain.Instance IsNot Nothing Then frmMain.Instance.OpenClaim(claimId)
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        OpenSelected()
    End Sub

    Private Sub dgvClaims_DoubleClick(sender As Object, e As EventArgs) Handles dgvClaims.DoubleClick
        OpenSelected()
    End Sub

    Private Sub dgvClaims_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvClaims.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            OpenSelected()
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If frmMain.Instance IsNot Nothing Then frmMain.Instance.OpenNewClaim()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadClaims()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtSearch.Clear()
        cboStatus.SelectedIndex = 0
        chkOpenOnly.Checked = True
        LoadClaims()
    End Sub

    Private Sub txtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            LoadClaims()
        End If
    End Sub

    Private Sub cboStatus_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles cboStatus.SelectedIndexChanged
        If Visible Then LoadClaims()
    End Sub

    Private Sub chkOpenOnly_CheckedChanged(sender As Object, e As EventArgs) _
        Handles chkOpenOnly.CheckedChanged
        If Visible Then LoadClaims()
    End Sub

End Class
