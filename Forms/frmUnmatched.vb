Option Strict On
Imports System.Data
Imports System.Windows.Forms

Public Class frmUnmatched

    Public Sub New()
        InitializeComponent()
        UiStyle.ApplyTo(Me)
        AppHelp.Attach(Me, AppHelp.IntakeQueue)
        UiStyle.StyleGrid(dgvQueue)
        txtPreview.Font = UiStyle.MonoFont
    End Sub

    Private Sub frmUnmatched_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim isHandlerSide As Boolean = Session.IsInRole(UserRole.Handler, UserRole.Supervisor)
        btnRecordNew.Enabled = isHandlerSide
        btnFindClaim.Enabled = isHandlerSide

        LoadQueue()
    End Sub

    Public Sub LoadQueue()
        Try
            Dim data As DataTable = CorrespondenceRepository.GetUnmatched()
            dgvQueue.DataSource = data
            UiStyle.HideColumn(dgvQueue, "Id")
            UiStyle.FormatDateColumn(dgvQueue, "Received", True)

            lblQueueCount.Text = If(data.Rows.Count = 0,
                "Nothing waiting. Anything new will appear here once it is recorded.",
                data.Rows.Count.ToString() & " item(s) waiting to be matched")

            ShowSelectedItem()
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Function SelectedCorrespondenceId() As Integer
        If dgvQueue.CurrentRow Is Nothing Then Return 0
        Return Db.AsInt(dgvQueue.CurrentRow.Cells("Id").Value)
    End Function

    Private Sub ShowSelectedItem()
        Dim id As Integer = SelectedCorrespondenceId()
        lvDocs.Items.Clear()

        If id = 0 Then
            txtPreview.Clear()
            Return
        End If

        Try
            Dim item As Correspondence = CorrespondenceRepository.GetById(id)
            If item Is Nothing Then
                txtPreview.Clear()
                Return
            End If

            txtPreview.Text =
                "Arrived by : " & item.Source & vbCrLf &
                "Received   : " & item.ReceivedAt.ToString("dd/MM/yyyy HH:mm") & vbCrLf &
                "From       : " & item.FromName & vbCrLf &
                "Contact    : " & item.FromAddress & vbCrLf &
                "Subject    : " & item.Subject & vbCrLf &
                New String("-"c, 60) & vbCrLf & vbCrLf &
                item.Body

            Dim docs As DataTable = AttachmentRepository.GetByCorrespondence(id)
            For Each row As DataRow In docs.Rows
                Dim entry As New ListViewItem(Db.AsString(row("File")))
                entry.SubItems.Add(Db.AsString(row("Type")))
                entry.Tag = Db.AsString(row("StoredPath"))
                lvDocs.Items.Add(entry)
            Next
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub dgvQueue_SelectionChanged(sender As Object, e As EventArgs) _
        Handles dgvQueue.SelectionChanged
        ShowSelectedItem()
    End Sub

    Private Sub FindClaimForSelected()
        Dim id As Integer = SelectedCorrespondenceId()
        If id = 0 Then
            UiStyle.ShowProblem(Me, "Select an item in the queue first.")
            Return
        End If

        Using matcher As New frmMatchClaim(id)
            matcher.ShowDialog(Me)
        End Using

        LoadQueue()
        If frmMain.Instance IsNot Nothing Then frmMain.Instance.RefreshIntakeCount()
    End Sub

    Private Sub btnFindClaim_Click(sender As Object, e As EventArgs) Handles btnFindClaim.Click
        FindClaimForSelected()
    End Sub

    Private Sub dgvQueue_DoubleClick(sender As Object, e As EventArgs) Handles dgvQueue.DoubleClick
        FindClaimForSelected()
    End Sub

    Private Sub btnRecordNew_Click(sender As Object, e As EventArgs) Handles btnRecordNew.Click
        Using intake As New frmIntake()
            intake.ShowDialog(Me)
        End Using
        LoadQueue()
        If frmMain.Instance IsNot Nothing Then frmMain.Instance.RefreshIntakeCount()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadQueue()
    End Sub

    Private Sub btnOpenDoc_Click(sender As Object, e As EventArgs) Handles btnOpenDoc.Click
        If lvDocs.SelectedItems.Count = 0 Then
            UiStyle.ShowProblem(Me, "Select an attachment to open it.")
            Return
        End If

        Try
            DocumentStore.OpenDocument(Convert.ToString(lvDocs.SelectedItems(0).Tag))
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub lvDocs_DoubleClick(sender As Object, e As EventArgs) Handles lvDocs.DoubleClick
        btnOpenDoc_Click(sender, e)
    End Sub

End Class
