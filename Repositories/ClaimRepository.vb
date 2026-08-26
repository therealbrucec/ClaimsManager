Option Strict On
Imports System.Data
Imports System.Text

Public Module ClaimRepository

    Private Const GridSelect As String =
        "SELECT c.[ClaimId] AS [Id], c.[ClaimRef] AS [Claim Ref], c.[PolicyNumber] AS [Policy No], " &
        "c.[InsuredName] AS [Insured], c.[DateOfLoss] AS [Date of Loss], " &
        "c.[DateNotified] AS [Notified], c.[Peril] AS [Peril], c.[Status] AS [Status], " &
        "c.[EstimatedValue] AS [Estimate], c.[ReserveAmount] AS [Reserve], " &
        "h.[FullName] AS [Handler], a.[FullName] AS [Adjuster] " &
        "FROM (([Claims] AS c " &
        "LEFT JOIN [AppUsers] AS h ON c.[HandlerUserId] = h.[UserId]) " &
        "LEFT JOIN [AppUsers] AS a ON c.[AdjusterUserId] = a.[UserId]) "

#Region "Reads"

    Public Function GetById(claimId As Integer) As Claim
        Dim dt As DataTable = Db.GetTable("SELECT * FROM [Claims] WHERE [ClaimId] = ?", claimId)
        If dt.Rows.Count = 0 Then Return Nothing
        Return FromRow(dt.Rows(0))
    End Function

    Public Function GetByRef(claimRef As String) As Claim
        Dim dt As DataTable = Db.GetTable("SELECT * FROM [Claims] WHERE [ClaimRef] = ?", claimRef)
        If dt.Rows.Count = 0 Then Return Nothing
        Return FromRow(dt.Rows(0))
    End Function

    ''' <summary>Grid data for the claim list. Any argument may be left blank.</summary>
    Public Function Search(searchText As String, status As String,
                           openOnly As Boolean, handlerUserId As Integer?,
                           adjusterUserId As Integer?) As DataTable
        Dim sql As New StringBuilder(GridSelect)
        Dim where As New List(Of String)()
        Dim args As New List(Of Object)()

        If Not String.IsNullOrWhiteSpace(searchText) Then
            Dim like_ As String = "%" & searchText.Trim() & "%"
            where.Add("(c.[ClaimRef] LIKE ? OR c.[PolicyNumber] LIKE ? OR c.[InsuredName] LIKE ?)")
            args.Add(like_)
            args.Add(like_)
            args.Add(like_)
        End If

        If Not String.IsNullOrWhiteSpace(status) Then
            where.Add("c.[Status] = ?")
            args.Add(status)
        ElseIf openOnly Then
            where.Add("c.[Status] IN (" & Quoted(ClaimStatus.Open) & ")")
        End If

        If handlerUserId.HasValue Then
            where.Add("c.[HandlerUserId] = ?")
            args.Add(handlerUserId.Value)
        End If

        If adjusterUserId.HasValue Then
            where.Add("c.[AdjusterUserId] = ?")
            args.Add(adjusterUserId.Value)
        End If

        If where.Count > 0 Then
            sql.Append("WHERE ").Append(String.Join(" AND ", where)).Append(" ")
        End If
        sql.Append("ORDER BY c.[DateNotified] DESC, c.[ClaimRef] DESC")

        Return Db.GetTable(sql.ToString(), args.ToArray())
    End Function

    ''' <summary>Claims sitting with adjusters, for the review queue.</summary>
    Public Function AdjusterQueue(adjusterUserId As Integer?) As DataTable
        Dim sql As New StringBuilder(GridSelect)
        Dim args As New List(Of Object)()

        sql.Append("WHERE c.[Status] IN ('" & ClaimStatus.WithAdjuster & "') ")
        If adjusterUserId.HasValue Then
            sql.Append("AND c.[AdjusterUserId] = ? ")
            args.Add(adjusterUserId.Value)
        End If
        sql.Append("ORDER BY c.[DateNotified]")

        Return Db.GetTable(sql.ToString(), args.ToArray())
    End Function

    ''' <summary>
    ''' Candidate existing claims for an inbound item. Deliberately loose: it is better to
    ''' show a handler four possibles than to silently miss the right one and open a duplicate.
    ''' </summary>
    Public Function FindPotentialDuplicates(policyNumber As String, insuredText As String,
                                            dateOfLoss As Date?, windowDays As Integer) As DataTable
        Dim clauses As New List(Of String)()
        Dim args As New List(Of Object)()

        If Not String.IsNullOrWhiteSpace(policyNumber) Then
            clauses.Add("c.[PolicyNumber] LIKE ?")
            args.Add("%" & policyNumber.Trim() & "%")

            ' Someone types NGC884213 when the policy is filed as NGC/884213. The digits run
            ' together either way, so matching on those alone bridges the two formats. Doing
            ' it this way avoids REPLACE(), which Jet does not expose through OLEDB.
            Dim digits As String = LongestDigitRun(policyNumber)
            If digits.Length >= 5 Then
                clauses.Add("c.[PolicyNumber] LIKE ?")
                args.Add("%" & digits & "%")
            End If
        End If

        If Not String.IsNullOrWhiteSpace(insuredText) Then
            clauses.Add("c.[InsuredName] LIKE ?")
            args.Add("%" & insuredText.Trim() & "%")
        End If

        If dateOfLoss.HasValue Then
            clauses.Add("(c.[DateOfLoss] BETWEEN ? AND ?)")
            args.Add(dateOfLoss.Value.AddDays(-windowDays))
            args.Add(dateOfLoss.Value.AddDays(windowDays))
        End If

        If clauses.Count = 0 Then Return Db.GetTable(GridSelect & "WHERE 1 = 0")

        Dim sql As String = GridSelect & "WHERE " & String.Join(" OR ", clauses) &
                            " ORDER BY c.[DateNotified] DESC"
        Return Db.GetTable(sql, args.ToArray())
    End Function

#End Region

#Region "Writes"

    Public Function Insert(claim As Claim, userName As String) As Integer
        If String.IsNullOrWhiteSpace(claim.ClaimRef) Then
            claim.ClaimRef = NextClaimRef()
        End If
        claim.CreatedAt = Date.Now
        claim.CreatedBy = userName
        claim.UpdatedAt = Date.Now
        claim.UpdatedBy = userName

        Const sql As String =
            "INSERT INTO [Claims] ([ClaimRef],[PolicyNumber],[InsuredName],[InsuredPostcode]," &
            "[ContactPhone],[ContactEmail],[DateOfLoss],[DateNotified],[Peril],[LossDescription]," &
            "[EstimatedValue],[ReserveAmount],[SettledAmount],[Status],[HandlerUserId]," &
            "[AdjusterUserId],[UnderwriterId],[CreatedAt],[CreatedBy],[UpdatedAt],[UpdatedBy]) " &
            "VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

        claim.ClaimId = Db.InsertAndGetId(sql,
            claim.ClaimRef, claim.PolicyNumber, claim.InsuredName, claim.InsuredPostcode,
            claim.ContactPhone, claim.ContactEmail, NullableDate(claim.DateOfLoss),
            claim.DateNotified, claim.Peril, claim.LossDescription,
            claim.EstimatedValue, claim.ReserveAmount, claim.SettledAmount, claim.Status,
            NullableInt(claim.HandlerUserId), NullableInt(claim.AdjusterUserId),
            NullableInt(claim.UnderwriterId), claim.CreatedAt, claim.CreatedBy,
            claim.UpdatedAt, claim.UpdatedBy)

        StatusHistoryRepository.Insert(claim.ClaimId, String.Empty, claim.Status,
                                       "Claim created", userName)
        AuditRepository.Write("Claim", claim.ClaimId, "Created",
                              "Claim " & claim.ClaimRef & " created", userName)
        Return claim.ClaimId
    End Function

    Public Sub Update(claim As Claim, userName As String)
        claim.UpdatedAt = Date.Now
        claim.UpdatedBy = userName

        Const sql As String =
            "UPDATE [Claims] SET [PolicyNumber]=?,[InsuredName]=?,[InsuredPostcode]=?," &
            "[ContactPhone]=?,[ContactEmail]=?,[DateOfLoss]=?,[DateNotified]=?,[Peril]=?," &
            "[LossDescription]=?,[EstimatedValue]=?,[ReserveAmount]=?,[SettledAmount]=?," &
            "[Status]=?,[HandlerUserId]=?,[AdjusterUserId]=?,[UnderwriterId]=?," &
            "[UpdatedAt]=?,[UpdatedBy]=? WHERE [ClaimId]=?"

        Db.Execute(sql,
            claim.PolicyNumber, claim.InsuredName, claim.InsuredPostcode,
            claim.ContactPhone, claim.ContactEmail, NullableDate(claim.DateOfLoss),
            claim.DateNotified, claim.Peril, claim.LossDescription,
            claim.EstimatedValue, claim.ReserveAmount, claim.SettledAmount, claim.Status,
            NullableInt(claim.HandlerUserId), NullableInt(claim.AdjusterUserId),
            NullableInt(claim.UnderwriterId), claim.UpdatedAt, claim.UpdatedBy, claim.ClaimId)
    End Sub

    Public Sub RecalculateSettled(claimId As Integer, userName As String)
        Dim total As Decimal = PaymentRepository.TotalForClaim(claimId)
        Db.Execute("UPDATE [Claims] SET [SettledAmount]=?,[UpdatedAt]=?,[UpdatedBy]=? WHERE [ClaimId]=?",
                   total, Date.Now, userName, claimId)
    End Sub

    ''' <summary>Hands out the next reference from the Counters table, e.g. CLM-2026-00007.</summary>
    Private Function NextClaimRef() As String
        Dim current As Integer = Db.AsInt(
            Db.Scalar("SELECT [NextValue] FROM [Counters] WHERE [CounterName] = ?", "ClaimRef"))
        If current <= 0 Then current = 1

        Db.Execute("UPDATE [Counters] SET [NextValue] = ? WHERE [CounterName] = ?",
                   current + 1, "ClaimRef")

        Return "CLM-" & Date.Today.Year.ToString() & "-" & current.ToString("00000")
    End Function

#End Region

#Region "Helpers"

    Public Function FromRow(row As DataRow) As Claim
        Return New Claim With {
            .ClaimId = Db.AsInt(row("ClaimId")),
            .ClaimRef = Db.AsString(row("ClaimRef")),
            .PolicyNumber = Db.AsString(row("PolicyNumber")),
            .InsuredName = Db.AsString(row("InsuredName")),
            .InsuredPostcode = Db.AsString(row("InsuredPostcode")),
            .ContactPhone = Db.AsString(row("ContactPhone")),
            .ContactEmail = Db.AsString(row("ContactEmail")),
            .DateOfLoss = Db.AsNullableDate(row("DateOfLoss")),
            .DateNotified = Db.AsDate(row("DateNotified")),
            .Peril = Db.AsString(row("Peril")),
            .LossDescription = Db.AsString(row("LossDescription")),
            .EstimatedValue = Db.AsDecimal(row("EstimatedValue")),
            .ReserveAmount = Db.AsDecimal(row("ReserveAmount")),
            .SettledAmount = Db.AsDecimal(row("SettledAmount")),
            .Status = Db.AsString(row("Status")),
            .HandlerUserId = Db.AsNullableInt(row("HandlerUserId")),
            .AdjusterUserId = Db.AsNullableInt(row("AdjusterUserId")),
            .UnderwriterId = Db.AsNullableInt(row("UnderwriterId")),
            .CreatedAt = Db.AsDate(row("CreatedAt")),
            .CreatedBy = Db.AsString(row("CreatedBy")),
            .UpdatedAt = Db.AsDate(row("UpdatedAt")),
            .UpdatedBy = Db.AsString(row("UpdatedBy"))
        }
    End Function

    Private Function NullableDate(value As Date?) As Object
        If Not value.HasValue Then Return Db.NullDate()
        Return value.Value
    End Function

    Private Function NullableInt(value As Integer?) As Object
        If Not value.HasValue Then Return Db.NullInteger()
        Return value.Value
    End Function

    Private Function Quoted(values As String()) As String
        Return String.Join(",", values.Select(Function(v) "'" & v.Replace("'", "''") & "'"))
    End Function

    ''' <summary>The longest run of digits in a policy number: NGC/884213 gives 884213.</summary>
    Private Function LongestDigitRun(text As String) As String
        Dim best As String = String.Empty
        Dim current As String = String.Empty

        For Each ch As Char In text
            If Char.IsDigit(ch) Then
                current &= ch
                If current.Length > best.Length Then best = current
            Else
                current = String.Empty
            End If
        Next

        Return best
    End Function

#End Region

End Module
