Option Strict On
Imports System.Data

Public Module CorrespondenceRepository

    Private Const GridSelect As String =
        "SELECT c.[CorrespondenceId] AS [Id], c.[ReceivedAt] AS [Received], " &
        "c.[Direction] AS [Direction], c.[Source] AS [Source], c.[FromName] AS [From], " &
        "c.[FromAddress] AS [Address], c.[Subject] AS [Subject], " &
        "(SELECT COUNT(*) FROM [Attachments] AS a " &
        " WHERE a.[CorrespondenceId] = c.[CorrespondenceId]) AS [Docs] " &
        "FROM [Correspondence] AS c "

    Public Function GetById(correspondenceId As Integer) As Correspondence
        Dim dt As DataTable = Db.GetTable(
            "SELECT * FROM [Correspondence] WHERE [CorrespondenceId] = ?", correspondenceId)
        If dt.Rows.Count = 0 Then Return Nothing
        Return FromRow(dt.Rows(0))
    End Function

    Public Function GetByClaim(claimId As Integer) As DataTable
        Return Db.GetTable(GridSelect & "WHERE c.[ClaimId] = ? ORDER BY c.[ReceivedAt] DESC", claimId)
    End Function

    ''' <summary>The intake queue: everything received that is not yet against a claim.</summary>
    Public Function GetUnmatched() As DataTable
        Return Db.GetTable(GridSelect & "WHERE c.[ClaimId] IS NULL ORDER BY c.[ReceivedAt]")
    End Function

    Public Function CountForClaim(claimId As Integer) As Integer
        Return Db.AsInt(Db.Scalar(
            "SELECT COUNT(*) FROM [Correspondence] WHERE [ClaimId] = ?", claimId))
    End Function

    Public Function CountUnmatched() As Integer
        Return Db.AsInt(Db.Scalar("SELECT COUNT(*) FROM [Correspondence] WHERE [ClaimId] IS NULL"))
    End Function

    Public Function Insert(item As Correspondence, userName As String) As Integer
        item.CreatedAt = Date.Now
        item.CreatedBy = userName

        Const sql As String =
            "INSERT INTO [Correspondence] ([ClaimId],[Direction],[Source],[ReceivedAt]," &
            "[FromName],[FromAddress],[Subject],[Body],[CreatedAt],[CreatedBy]) " &
            "VALUES (?,?,?,?,?,?,?,?,?,?)"

        Dim claimIdParam As Object = If(item.ClaimId.HasValue,
                                        CType(item.ClaimId.Value, Object), Db.NullInteger())

        item.CorrespondenceId = Db.InsertAndGetId(sql,
            claimIdParam, item.Direction, item.Source, item.ReceivedAt,
            item.FromName, item.FromAddress, item.Subject, item.Body,
            item.CreatedAt, item.CreatedBy)

        AuditRepository.Write("Correspondence", item.CorrespondenceId, "Recorded",
                              item.Source & " from " & item.FromName, userName)
        Return item.CorrespondenceId
    End Function

    Public Sub Update(item As Correspondence, userName As String)
        Const sql As String =
            "UPDATE [Correspondence] SET [Direction]=?,[Source]=?,[ReceivedAt]=?," &
            "[FromName]=?,[FromAddress]=?,[Subject]=?,[Body]=? WHERE [CorrespondenceId]=?"

        Db.Execute(sql, item.Direction, item.Source, item.ReceivedAt,
                   item.FromName, item.FromAddress, item.Subject, item.Body,
                   item.CorrespondenceId)

        AuditRepository.Write("Correspondence", item.CorrespondenceId, "Amended",
                              "Details edited", userName)
    End Sub

    ''' <summary>
    ''' Files an inbound item against a claim and moves any documents that came with it
    ''' into that claim's folder. This is the step that turns a pile of email into a case.
    ''' </summary>
    Public Sub MatchToClaim(correspondenceId As Integer, claimId As Integer, userName As String)
        Dim claim As Claim = ClaimRepository.GetById(claimId)
        If claim Is Nothing Then
            Throw New ApplicationException("That claim no longer exists.")
        End If

        Db.Execute("UPDATE [Correspondence] SET [ClaimId] = ? WHERE [CorrespondenceId] = ?",
                   claimId, correspondenceId)

        AttachmentRepository.AssignToClaim(correspondenceId, claim, userName)

        AuditRepository.Write("Correspondence", correspondenceId, "Matched to claim",
                              "Filed against " & claim.ClaimRef, userName)
        AuditRepository.Write("Claim", claimId, "Correspondence filed",
                              "Inbound item " & correspondenceId.ToString() & " attached", userName)
    End Sub

    Public Sub Unmatch(correspondenceId As Integer, userName As String)
        Db.Execute("UPDATE [Correspondence] SET [ClaimId] = ? WHERE [CorrespondenceId] = ?",
                   Db.NullInteger(), correspondenceId)
        AuditRepository.Write("Correspondence", correspondenceId, "Returned to intake",
                              "Unfiled from claim", userName)
    End Sub

    Public Function FromRow(row As DataRow) As Correspondence
        Return New Correspondence With {
            .CorrespondenceId = Db.AsInt(row("CorrespondenceId")),
            .ClaimId = Db.AsNullableInt(row("ClaimId")),
            .Direction = Db.AsString(row("Direction")),
            .Source = Db.AsString(row("Source")),
            .ReceivedAt = Db.AsDate(row("ReceivedAt")),
            .FromName = Db.AsString(row("FromName")),
            .FromAddress = Db.AsString(row("FromAddress")),
            .Subject = Db.AsString(row("Subject")),
            .Body = Db.AsString(row("Body")),
            .CreatedAt = Db.AsDate(row("CreatedAt")),
            .CreatedBy = Db.AsString(row("CreatedBy"))
        }
    End Function

End Module
