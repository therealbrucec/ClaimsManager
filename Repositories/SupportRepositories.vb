Option Strict On
Imports System.Data

Public Module NoteRepository

    Public Function GetByClaim(claimId As Integer) As DataTable
        Return Db.GetTable(
            "SELECT [CreatedAt] AS [When], [CreatedBy] AS [Who], " &
            "IIF([IsInternal], 'Internal', 'Shared') AS [Visibility], " &
            "[NoteText] AS [Note] FROM [ClaimNotes] WHERE [ClaimId] = ? " &
            "ORDER BY [CreatedAt] DESC", claimId)
    End Function

    Public Function Insert(claimId As Integer, text As String, isInternal As Boolean,
                           userName As String) As Integer
        Dim id As Integer = Db.InsertAndGetId(
            "INSERT INTO [ClaimNotes] ([ClaimId],[NoteText],[IsInternal],[CreatedAt],[CreatedBy]) " &
            "VALUES (?,?,?,?,?)",
            claimId, text, isInternal, Date.Now, userName)

        AuditRepository.Write("Claim", claimId, "Note added",
                              If(isInternal, "Internal note", "Shared note"), userName)
        Return id
    End Function

End Module

Public Module PaymentRepository

    Public Function GetByClaim(claimId As Integer) As DataTable
        Return Db.GetTable(
            "SELECT [PaymentId] AS [Id], [PaidAt] AS [Paid], [PaymentKind] AS [Type], " &
            "[Amount] AS [Amount], [Payee] AS [Payee], [Reference] AS [Reference], " &
            "[CreatedBy] AS [Entered By] FROM [Payments] WHERE [ClaimId] = ? " &
            "ORDER BY [PaidAt] DESC", claimId)
    End Function

    ''' <summary>Recoveries reduce the net paid figure, everything else adds to it.</summary>
    Public Function TotalForClaim(claimId As Integer) As Decimal
        Dim paid As Decimal = Db.AsDecimal(Db.Scalar(
            "SELECT SUM([Amount]) FROM [Payments] WHERE [ClaimId] = ? AND [PaymentKind] <> ?",
            claimId, PaymentType.Recovery))
        Dim recovered As Decimal = Db.AsDecimal(Db.Scalar(
            "SELECT SUM([Amount]) FROM [Payments] WHERE [ClaimId] = ? AND [PaymentKind] = ?",
            claimId, PaymentType.Recovery))
        Return paid - recovered
    End Function

    Public Function Insert(item As Payment, userName As String) As Integer
        item.CreatedBy = userName

        Dim id As Integer = Db.InsertAndGetId(
            "INSERT INTO [Payments] ([ClaimId],[PaymentKind],[Amount],[Payee],[Reference]," &
            "[PaidAt],[CreatedBy]) VALUES (?,?,?,?,?,?,?)",
            item.ClaimId, item.PaymentKind, item.Amount, item.Payee,
            item.Reference, item.PaidAt, item.CreatedBy)

        item.PaymentId = id
        ClaimRepository.RecalculateSettled(item.ClaimId, userName)
        AuditRepository.Write("Claim", item.ClaimId, "Payment recorded",
                              item.PaymentKind & " " & item.Amount.ToString("C") &
                              " to " & item.Payee, userName)
        Return id
    End Function

    Public Sub Delete(paymentId As Integer, claimId As Integer, userName As String)
        Db.Execute("DELETE FROM [Payments] WHERE [PaymentId] = ?", paymentId)
        ClaimRepository.RecalculateSettled(claimId, userName)
        AuditRepository.Write("Claim", claimId, "Payment removed",
                              "Payment " & paymentId.ToString() & " deleted", userName)
    End Sub

End Module

Public Module StatusHistoryRepository

    Public Function GetByClaim(claimId As Integer) As DataTable
        Return Db.GetTable(
            "SELECT [ChangedAt] AS [When], [ChangedBy] AS [Who], [FromStatus] AS [From], " &
            "[ToStatus] AS [To], [Rationale] AS [Reason] FROM [ClaimStatusHistory] " &
            "WHERE [ClaimId] = ? ORDER BY [ChangedAt] DESC", claimId)
    End Function

    Public Sub Insert(claimId As Integer, fromStatus As String, toStatus As String,
                      rationale As String, userName As String)
        Db.Execute(
            "INSERT INTO [ClaimStatusHistory] ([ClaimId],[FromStatus],[ToStatus]," &
            "[Rationale],[ChangedAt],[ChangedBy]) VALUES (?,?,?,?,?,?)",
            claimId, fromStatus, toStatus, rationale, Date.Now, userName)
    End Sub

End Module

Public Module AuditRepository

    ''' <summary>
    ''' Append only. Nothing in this application updates or deletes an audit row, which is
    ''' the whole point of having one.
    ''' </summary>
    Public Sub Write(entityType As String, entityId As Integer, action As String,
                     detail As String, userName As String)
        Db.Execute(
            "INSERT INTO [AuditLog] ([EntityType],[EntityId],[Action],[Detail]," &
            "[PerformedAt],[PerformedBy]) VALUES (?,?,?,?,?,?)",
            entityType, entityId, action, detail, Date.Now, userName)
    End Sub

    Public Function GetForClaim(claimId As Integer) As DataTable
        Return Db.GetTable(
            "SELECT [PerformedAt] AS [When], [PerformedBy] AS [Who], [Action] AS [Action], " &
            "[Detail] AS [Detail] FROM [AuditLog] WHERE [EntityType] = 'Claim' AND [EntityId] = ? " &
            "ORDER BY [PerformedAt] DESC", claimId)
    End Function

End Module

Public Module UserRepository

    Public Function GetActive() As List(Of AppUser)
        Dim dt As DataTable = Db.GetTable(
            "SELECT * FROM [AppUsers] WHERE [IsActive] = True ORDER BY [Role], [FullName]")
        Return (From row As DataRow In dt.Rows.Cast(Of DataRow)() Select FromRow(row)).ToList()
    End Function

    Public Function GetByRole(role As String) As List(Of AppUser)
        Dim dt As DataTable = Db.GetTable(
            "SELECT * FROM [AppUsers] WHERE [IsActive] = True AND [Role] = ? ORDER BY [FullName]",
            role)
        Return (From row As DataRow In dt.Rows.Cast(Of DataRow)() Select FromRow(row)).ToList()
    End Function

    Public Function FindIdByUserName(userName As String) As Integer
        Return Db.AsInt(Db.Scalar(
            "SELECT [UserId] FROM [AppUsers] WHERE [UserName] = ?", userName))
    End Function

    ''' <summary>Looks up a typed user name. Jet compares text case-insensitively, so
    ''' "JBaker" finds jbaker.</summary>
    Public Function FindByUserName(userName As String) As AppUser
        If String.IsNullOrWhiteSpace(userName) Then Return Nothing

        Dim dt As DataTable = Db.GetTable(
            "SELECT * FROM [AppUsers] WHERE [UserName] = ?", userName.Trim())

        If dt.Rows.Count = 0 Then Return Nothing
        Return FromRow(dt.Rows(0))
    End Function

    Private Function FromRow(row As DataRow) As AppUser
        Return New AppUser With {
            .UserId = Db.AsInt(row("UserId")),
            .UserName = Db.AsString(row("UserName")),
            .FullName = Db.AsString(row("FullName")),
            .Role = Db.AsString(row("Role")),
            .IsActive = Db.AsBool(row("IsActive"))
        }
    End Function

End Module

Public Module UnderwriterRepository

    Public Function GetAll() As List(Of Underwriter)
        Dim dt As DataTable = Db.GetTable("SELECT * FROM [Underwriters] ORDER BY [Name]")
        Return (From row As DataRow In dt.Rows.Cast(Of DataRow)()
                Select New Underwriter With {
                    .UnderwriterId = Db.AsInt(row("UnderwriterId")),
                    .Name = Db.AsString(row("Name")),
                    .SchemeRef = Db.AsString(row("SchemeRef")),
                    .ContactEmail = Db.AsString(row("ContactEmail")),
                    .FaxNumber = Db.AsString(row("FaxNumber"))
                }).ToList()
    End Function

End Module
