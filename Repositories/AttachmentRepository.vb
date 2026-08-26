Option Strict On
Imports System.Data
Imports System.IO

Public Module AttachmentRepository

    Private Const GridSelect As String =
        "SELECT [AttachmentId] AS [Id], [FileName] AS [File], [DocumentType] AS [Type], " &
        "[FileSize] AS [Bytes], [AddedAt] AS [Added], [AddedBy] AS [Added By], " &
        "[StoredPath] AS [StoredPath] FROM [Attachments] "

    Public Function GetByClaim(claimId As Integer) As DataTable
        Return Db.GetTable(GridSelect & "WHERE [ClaimId] = ? ORDER BY [AddedAt] DESC", claimId)
    End Function

    Public Function GetByCorrespondence(correspondenceId As Integer) As DataTable
        Return Db.GetTable(GridSelect & "WHERE [CorrespondenceId] = ? ORDER BY [AddedAt]",
                           correspondenceId)
    End Function

    Public Function GetById(attachmentId As Integer) As Attachment
        Dim dt As DataTable = Db.GetTable(
            "SELECT * FROM [Attachments] WHERE [AttachmentId] = ?", attachmentId)
        If dt.Rows.Count = 0 Then Return Nothing
        Return FromRow(dt.Rows(0))
    End Function

    Public Function CountForClaim(claimId As Integer) As Integer
        Return Db.AsInt(Db.Scalar("SELECT COUNT(*) FROM [Attachments] WHERE [ClaimId] = ?", claimId))
    End Function

    ''' <summary>
    ''' Copies the file into the document store, then records it. Documents captured during
    ''' intake have no claim yet, so they land in an "Unfiled" folder and are moved later.
    ''' </summary>
    Public Function Import(sourceFile As String, documentType As String,
                           claimId As Integer?, claimRef As String,
                           correspondenceId As Integer?, userName As String) As Attachment
        Dim folder As String = If(String.IsNullOrWhiteSpace(claimRef), "Unfiled", claimRef)
        Dim relativePath As String = DocumentStore.Import(sourceFile, folder)

        Dim item As New Attachment With {
            .ClaimId = claimId,
            .CorrespondenceId = correspondenceId,
            .FileName = Path.GetFileName(sourceFile),
            .StoredPath = relativePath,
            .DocumentType = documentType,
            .FileSize = DocumentStore.SizeOf(sourceFile),
            .AddedAt = Date.Now,
            .AddedBy = userName
        }

        Const sql As String =
            "INSERT INTO [Attachments] ([ClaimId],[CorrespondenceId],[FileName],[StoredPath]," &
            "[DocumentType],[FileSize],[AddedAt],[AddedBy]) VALUES (?,?,?,?,?,?,?,?)"

        item.AttachmentId = Db.InsertAndGetId(sql,
            IdParam(item.ClaimId), IdParam(item.CorrespondenceId),
            item.FileName, item.StoredPath, item.DocumentType, item.FileSize,
            item.AddedAt, item.AddedBy)

        If claimId.HasValue Then
            AuditRepository.Write("Claim", claimId.Value, "Document added",
                                  item.DocumentType & ": " & item.FileName, userName)
        End If

        Return item
    End Function

    ''' <summary>Moves documents that arrived with an inbound item onto a claim.</summary>
    Public Sub AssignToClaim(correspondenceId As Integer, claim As Claim, userName As String)
        Dim dt As DataTable = Db.GetTable(
            "SELECT [AttachmentId],[StoredPath],[FileName] FROM [Attachments] " &
            "WHERE [CorrespondenceId] = ?", correspondenceId)

        For Each row As DataRow In dt.Rows
            Dim attachmentId As Integer = Db.AsInt(row("AttachmentId"))
            Dim oldPath As String = Db.AsString(row("StoredPath"))
            Dim newPath As String = oldPath

            Try
                newPath = DocumentStore.Relocate(oldPath, claim.ClaimRef)
            Catch ex As IOException
                ' Leave the file where it is rather than losing the database link to it.
                newPath = oldPath
            End Try

            Db.Execute("UPDATE [Attachments] SET [ClaimId] = ?, [StoredPath] = ? " &
                       "WHERE [AttachmentId] = ?", claim.ClaimId, newPath, attachmentId)
        Next

        If dt.Rows.Count > 0 Then
            AuditRepository.Write("Claim", claim.ClaimId, "Documents filed",
                                  dt.Rows.Count.ToString() & " document(s) filed from intake",
                                  userName)
        End If
    End Sub

    Public Sub Delete(attachmentId As Integer, userName As String)
        Dim item As Attachment = GetById(attachmentId)
        If item Is Nothing Then Return

        Db.Execute("DELETE FROM [Attachments] WHERE [AttachmentId] = ?", attachmentId)

        ' The database row goes; the file stays on disk so nothing is ever destroyed
        ' by a mis-click. Housekeeping of the store is a deliberate, separate job.
        If item.ClaimId.HasValue Then
            AuditRepository.Write("Claim", item.ClaimId.Value, "Document removed",
                                  item.FileName & " (file retained in store)", userName)
        End If
    End Sub

    Public Function FromRow(row As DataRow) As Attachment
        Return New Attachment With {
            .AttachmentId = Db.AsInt(row("AttachmentId")),
            .ClaimId = Db.AsNullableInt(row("ClaimId")),
            .CorrespondenceId = Db.AsNullableInt(row("CorrespondenceId")),
            .FileName = Db.AsString(row("FileName")),
            .StoredPath = Db.AsString(row("StoredPath")),
            .DocumentType = Db.AsString(row("DocumentType")),
            .FileSize = Db.AsLong(row("FileSize")),
            .AddedAt = Db.AsDate(row("AddedAt")),
            .AddedBy = Db.AsString(row("AddedBy"))
        }
    End Function

    Private Function IdParam(value As Integer?) As Object
        If Not value.HasValue Then Return Db.NullInteger()
        Return value.Value
    End Function

End Module
