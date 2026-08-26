Option Strict On
Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' Manual capture of one thing that arrived: an email, a fax, a letter or a phone call,
''' plus whatever came with it. Everything else in the app follows from getting this right.
''' </summary>
Public Class frmIntake

    Private Class StagedFile
        Public Property SourcePath As String = String.Empty
        Public Property DocumentType As String = DocType.Other
    End Class

    Private ReadOnly _claimId As Integer?
    Private ReadOnly _claimRef As String
    Private ReadOnly _staged As New List(Of StagedFile)()

    Public Property SavedCorrespondenceId As Integer

    Public Sub New()
        Me.New(Nothing, String.Empty)
    End Sub

    ''' <summary>Use this overload to file an item straight onto a known claim.</summary>
    Public Sub New(claimId As Integer?, claimRef As String)
        InitializeComponent()
        UiStyle.ApplyTo(Me)

        _claimId = claimId
        _claimRef = If(claimRef, String.Empty)
    End Sub

    Private Sub frmIntake_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboSource.Items.AddRange(CorrespondenceSource.All)
        cboSource.SelectedItem = CorrespondenceSource.Email

        cboDocType.Items.AddRange(DocType.All)
        cboDocType.SelectedItem = DocType.Photo

        dtpReceived.Value = Date.Now

        If _claimId.HasValue Then
            lblContext.Text = "Filing against claim " & _claimRef
            btnSaveAndMatch.Visible = False
            btnSave.Text = "Save to claim"
        Else
            lblContext.Text = "This item will sit in the intake queue until it is matched to a claim."
        End If
    End Sub

#Region "Attachments"

    Private Sub btnAddFile_Click(sender As Object, e As EventArgs) Handles btnAddFile.Click
        Using picker As New OpenFileDialog()
            picker.Title = "Choose files that arrived with this item"
            picker.Multiselect = True
            picker.Filter = "All files (*.*)|*.*|" &
                            "Images (*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.bmp)|*.jpg;*.jpeg;*.png;*.tif;*.tiff;*.bmp|" &
                            "Documents (*.pdf;*.doc;*.docx;*.txt;*.rtf)|*.pdf;*.doc;*.docx;*.txt;*.rtf"

            If picker.ShowDialog(Me) <> DialogResult.OK Then Return

            Dim chosenType As String = Convert.ToString(cboDocType.SelectedItem)
            If String.IsNullOrEmpty(chosenType) Then chosenType = DocType.Other

            For Each chosenPath As String In picker.FileNames
                AddStagedFile(chosenPath, chosenType)
            Next
        End Using
    End Sub

    Private Sub AddStagedFile(sourcePath As String, documentType As String)
        If _staged.Any(Function(f) String.Equals(f.SourcePath, sourcePath, StringComparison.OrdinalIgnoreCase)) Then
            Return
        End If

        Dim staged As New StagedFile With {.SourcePath = sourcePath, .DocumentType = documentType}
        _staged.Add(staged)

        Dim item As New ListViewItem(Path.GetFileName(sourcePath))
        item.SubItems.Add(documentType)
        item.Tag = staged
        item.ToolTipText = sourcePath
        lvFiles.Items.Add(item)
    End Sub

    Private Sub btnRemoveFile_Click(sender As Object, e As EventArgs) Handles btnRemoveFile.Click
        If lvFiles.SelectedItems.Count = 0 Then
            UiStyle.ShowProblem(Me, "Select a file in the list to remove it.")
            Return
        End If

        Dim item As ListViewItem = lvFiles.SelectedItems(0)
        Dim staged As StagedFile = TryCast(item.Tag, StagedFile)
        If staged IsNot Nothing Then _staged.Remove(staged)
        lvFiles.Items.Remove(item)
    End Sub

#End Region

#Region "Saving"

    Private Function ValidateItem() As String
        If cboSource.SelectedItem Is Nothing Then
            Return "Choose how this item arrived."
        End If
        If String.IsNullOrWhiteSpace(txtFromName.Text) AndAlso
           String.IsNullOrWhiteSpace(txtFromAddress.Text) Then
            Return "Record who this came from, either a name or an email or fax number."
        End If
        If String.IsNullOrWhiteSpace(txtSubject.Text) AndAlso
           String.IsNullOrWhiteSpace(txtBody.Text) Then
            Return "Enter a subject or paste the text of the item, so it can be identified later."
        End If
        If dtpReceived.Value > Date.Now.AddMinutes(5) Then
            Return "The received date is in the future. Correct it before saving."
        End If
        Return String.Empty
    End Function

    ''' <summary>Writes the item and its documents. Returns 0 if it could not be saved.</summary>
    Private Function SaveItem() As Integer
        Dim problem As String = ValidateItem()
        If problem.Length > 0 Then
            UiStyle.ShowProblem(Me, problem)
            Return 0
        End If

        Dim item As New Correspondence With {
            .ClaimId = _claimId,
            .Direction = CorrespondenceDirection.Inbound,
            .Source = Convert.ToString(cboSource.SelectedItem),
            .ReceivedAt = dtpReceived.Value,
            .FromName = txtFromName.Text.Trim(),
            .FromAddress = txtFromAddress.Text.Trim(),
            .Subject = txtSubject.Text.Trim(),
            .Body = txtBody.Text
        }

        Try
            CorrespondenceRepository.Insert(item, Session.UserName)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
            Return 0
        End Try

        Dim failed As New List(Of String)()
        For Each staged As StagedFile In _staged
            Try
                AttachmentRepository.Import(staged.SourcePath, staged.DocumentType,
                                            _claimId, _claimRef,
                                            item.CorrespondenceId, Session.UserName)
            Catch ex As Exception
                failed.Add(Path.GetFileName(staged.SourcePath) & ": " & ex.Message)
            End Try
        Next

        If failed.Count > 0 Then
            UiStyle.ShowProblem(Me, "The item was saved, but these files were not copied " &
                                    "into the document store:" & vbCrLf & vbCrLf &
                                    String.Join(vbCrLf, failed))
        End If

        SavedCorrespondenceId = item.CorrespondenceId
        Return item.CorrespondenceId
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If SaveItem() = 0 Then Return
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnSaveAndMatch_Click(sender As Object, e As EventArgs) Handles btnSaveAndMatch.Click
        Dim correspondenceId As Integer = SaveItem()
        If correspondenceId = 0 Then Return

        Using matcher As New frmMatchClaim(correspondenceId)
            matcher.ShowDialog(Me)
        End Using

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _staged.Count > 0 OrElse txtBody.TextLength > 0 Then
            If MessageBox.Show(Me, "Discard this item without saving?", "Claims Manager",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
                Return
            End If
        End If
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

#End Region

End Class
