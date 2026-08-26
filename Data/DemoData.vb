Option Strict On
Imports System.Data

''' <summary>
''' Builds a year's worth of plausible claims so the reports have something to say.
''' The generator is seeded with a fixed number, so every fresh database comes out
''' identical - useful when you are comparing two runs of a report.
''' </summary>
Public Module DemoData

    Private ReadOnly Rng As New Random(20260801)

    Private Const ClaimCount As Integer = 140

#Region "Vocabulary"

    Private ReadOnly BusinessPlace As String() = {
        "Harbour Lane", "Ashcroft", "Bellingham", "Cravendale", "Dunmore", "Eastgate",
        "Fenwick", "Granby", "Hollowell", "Ivybridge", "Kelsall", "Langton",
        "Marchwood", "Netherby", "Oakfield", "Pennington", "Quarrydale", "Redgrave",
        "Stanmore", "Thornbury", "Uppingham", "Westmill", "Yardley", "Calderbank"}

    Private ReadOnly BusinessTrade As String() = {
        "Bakery", "Joinery", "Scaffolding Services", "Motor Factors", "Print",
        "Catering", "Plant Hire", "Fabrications", "Garden Centre", "Dairy",
        "Upholstery", "Electrical", "Roofing", "Convenience Store", "Dry Cleaners",
        "Veterinary Practice", "Care Services", "Haulage", "Engineering",
        "Interiors", "Brewing Co", "Cold Storage", "Tyres", "Glazing"}

    Private ReadOnly BusinessSuffix As String() = {
        "Ltd", "Ltd", "Ltd", "Limited", "LLP", "& Sons Ltd", ""}

    Private ReadOnly FirstNames As String() = {
        "Gordon", "Alison", "Raymond", "Priya", "Callum", "Deborah", "Ian", "Bridget",
        "Malcolm", "Yvonne", "Trevor", "Nadia", "Stuart", "Fiona", "Dermot", "Joanne",
        "Clive", "Rosalind", "Aled", "Marta", "Kenneth", "Shirin", "Owen", "Bernadette"}

    Private ReadOnly Surnames As String() = {
        "Wraith", "Hollis", "Okafor", "Baxter", "Nesbitt", "Fairclough", "Lindsay",
        "Kowalski", "Attwood", "Sharma", "Prendergast", "Mullins", "Eastwood",
        "Garrity", "Tobin", "Villiers", "Rennie", "Amos", "Cadwallader", "Duffy"}

    Private ReadOnly PostcodeAreas As String() = {
        "BS", "M", "NE", "LS", "B", "CF", "EH", "G", "L", "NG",
        "S", "CV", "PL", "SO", "DE", "HU", "IP", "LN", "PR", "TF"}

    Private ReadOnly PhoneAreas As String() = {
        "0117", "0161", "0191", "0113", "0121", "029", "0131", "0141",
        "0151", "0115", "0114", "024", "01752", "023"}

    Private ReadOnly Letters As String = "ABDEFGHJLNPRSTUWXYZ"

    ' Weighted to something like a real UK commercial book: water damage dominates
    ' by frequency, fire and liability by value.
    Private ReadOnly Perils As String() = {
        "Escape of Water", "Theft", "Storm", "Accidental Damage", "Fire", "Impact",
        "Public Liability", "Malicious Damage", "Flood", "Employers Liability",
        "Subsidence", "Business Interruption", "Other"}

    Private ReadOnly PerilWeights As Integer() = {
        22, 15, 12, 10, 8, 8, 7, 5, 4, 4, 2, 2, 1}

    Private Function ValueBand(peril As String) As Integer()
        Select Case peril
            Case "Escape of Water" : Return New Integer() {1500, 25000}
            Case "Fire" : Return New Integer() {8000, 250000}
            Case "Storm" : Return New Integer() {1200, 40000}
            Case "Flood" : Return New Integer() {5000, 120000}
            Case "Theft" : Return New Integer() {800, 30000}
            Case "Malicious Damage" : Return New Integer() {500, 12000}
            Case "Accidental Damage" : Return New Integer() {400, 15000}
            Case "Impact" : Return New Integer() {1000, 35000}
            Case "Subsidence" : Return New Integer() {15000, 180000}
            Case "Public Liability" : Return New Integer() {2500, 150000}
            Case "Employers Liability" : Return New Integer() {5000, 200000}
            Case "Business Interruption" : Return New Integer() {10000, 300000}
            Case Else : Return New Integer() {500, 20000}
        End Select
    End Function

    Private Function LossNarrative(peril As String) As String
        Dim options As String()

        Select Case peril
            Case "Escape of Water"
                options = New String() {
                    "Failed joint on the mains feed above the ceiling void. Water tracked " &
                    "down two walls overnight and into the ground floor.",
                    "Washroom supply pipe split during the cold spell. Discovered on opening " &
                    "Monday morning, standing water throughout the rear store.",
                    "Heating circuit leaked behind a stud wall over an extended period. " &
                    "Plaster and flooring affected, some stock written off."}
            Case "Fire"
                options = New String() {
                    "Fire originated in a distribution board in the plant room. Smoke damage " &
                    "throughout, fire brigade attended and confirmed electrical origin.",
                    "Discarded smoking materials ignited waste packaging at the rear of the " &
                    "premises. Spread to the loading bay before being extinguished.",
                    "Overheated extraction unit above the cooking range. Localised fire " &
                    "damage with extensive smoke contamination."}
            Case "Storm"
                options = New String() {
                    "Named storm lifted roof sheets and flashing. Water ingress to the first " &
                    "floor followed while the roof was open.",
                    "Wind brought down a boundary fence and damaged two roller shutters.",
                    "Gable end coping stones dislodged in high winds, striking a parked " &
                    "vehicle below."}
            Case "Flood"
                options = New String() {
                    "Surface water entered from the road after prolonged rainfall. " &
                    "Depth around 300mm across the shop floor.",
                    "Watercourse burst its banks upstream. Ground floor inundated, " &
                    "plant and stock at low level affected."}
            Case "Theft"
                options = New String() {
                    "Forced entry through the rear fire door. Tools and two laptops taken, " &
                    "damage to the frame and lock.",
                    "Break-in overnight via a roof light. Stock and till float removed, " &
                    "police attended and issued a crime reference.",
                    "Van broken into on site. Power tools taken from a locked cage."}
            Case "Malicious Damage"
                options = New String() {
                    "Graffiti and broken glazing to the frontage discovered on opening.",
                    "Deliberate damage to external units and signage overnight."}
            Case "Accidental Damage"
                options = New String() {
                    "Contractor put a screw through a concealed pipe while fitting shelving.",
                    "Forklift struck a racking upright, bringing down part of a bay.",
                    "Glazed partition broken while moving equipment through the unit."}
            Case "Impact"
                options = New String() {
                    "Third party vehicle reversed into the shopfront, damaging glazing " &
                    "and the frame.",
                    "Delivery lorry struck the canopy on entry to the yard.",
                    "Vehicle left the carriageway and came to rest against the boundary wall."}
            Case "Subsidence"
                options = New String() {
                    "Progressive cracking to the rear elevation. Monitoring in place and " &
                    "vegetation nearby suspected as the cause.",
                    "Movement to a single storey extension, cracking above door and " &
                    "window openings."}
            Case "Public Liability"
                options = New String() {
                    "Third party alleges injury after a slip in the customer area. " &
                    "Solicitors instructed and a letter of claim received.",
                    "Member of the public alleges injury from falling debris at the site " &
                    "entrance. Liability not admitted pending investigation.",
                    "Damage alleged to a neighbouring property during external works."}
            Case "Employers Liability"
                options = New String() {
                    "Employee alleges injury handling a load in the warehouse. " &
                    "Accident book entry and RIDDOR report on file.",
                    "Claim from a former employee regarding long term exposure. " &
                    "Records requested from the period concerned."}
            Case "Business Interruption"
                options = New String() {
                    "Loss of trade following the adjoining fire. Premises closed to " &
                    "customers while access was restricted.",
                    "Production halted after damage to the main line. Increased cost of " &
                    "working claimed alongside lost gross profit."}
            Case Else
                options = New String() {
                    "Circumstances reported by the insured and under investigation.",
                    "Details provided are limited pending the adjuster's inspection."}
        End Select

        Return options(Rng.Next(options.Length))
    End Function

#End Region

    Public Sub Seed(Optional progress As Action(Of String, Integer) = Nothing)
        Dim handlerIds As Integer() = {
            UserRepository.FindIdByUserName("jbaker"),
            UserRepository.FindIdByUserName("mokafor"),
            UserRepository.FindIdByUserName("shollis")}

        Dim adjusterIds As Integer() = {
            UserRepository.FindIdByUserName("rpatel"),
            UserRepository.FindIdByUserName("dcampbell")}

        Dim underwriterIds As New List(Of Integer)()
        For Each u As Underwriter In UnderwriterRepository.GetAll()
            underwriterIds.Add(u.UnderwriterId)
        Next
        If underwriterIds.Count = 0 Then underwriterIds.Add(1)

        ' A full year back from the day the database is created, through to that day.
        Dim windowEnd As Date = Date.Today
        Dim windowStart As Date = windowEnd.AddYears(-1)
        Dim spanDays As Integer = CInt((windowEnd - windowStart).TotalDays)

        ' Notification dates first, so references run in date order like a real ledger.
        Dim notifiedDates As New List(Of Date)()
        For i As Integer = 1 To ClaimCount
            notifiedDates.Add(windowStart.AddDays(Rng.Next(spanDays + 1)))
        Next
        notifiedDates.Sort()

        Dim reference As Integer = 1
        Dim policySeed As Integer = 100000
        Dim done As Integer = 0

        If progress IsNot Nothing Then
            progress("Generating twelve months of claims...", 25)
        End If

        For Each notified As Date In notifiedDates
            Dim peril As String = PickWeighted(Perils, PerilWeights)
            Dim isBusiness As Boolean = Rng.Next(100) < 72
            Dim insured As String = If(isBusiness, BusinessName(), PersonName())

            Dim band As Integer() = ValueBand(peril)
            Dim estimate As Decimal = SkewedAmount(band(0), band(1))
            Dim reserve As Decimal = RoundTo(estimate * CDec(1.05R + Rng.NextDouble() * 0.2R), 50)

            Dim ageMonths As Integer = MonthsBetween(notified, windowEnd)
            Dim status As String = StatusForAge(ageMonths)

            Dim claim As New Claim With {
                .ClaimRef = "CLM-" & notified.Year.ToString() & "-" & reference.ToString("00000"),
                .PolicyNumber = PolicyNumber(policySeed),
                .InsuredName = insured,
                .InsuredPostcode = Postcode(),
                .ContactPhone = Phone(),
                .ContactEmail = EmailFor(insured, isBusiness),
                .DateOfLoss = notified.AddDays(-Rng.Next(0, 11)),
                .DateNotified = notified,
                .Peril = peril,
                .LossDescription = LossNarrative(peril),
                .EstimatedValue = estimate,
                .ReserveAmount = reserve,
                .Status = status,
                .HandlerUserId = handlerIds(Rng.Next(handlerIds.Length))
            }

            ' An adjuster is only involved once the claim has been referred.
            If NeedsAdjuster(status) Then
                claim.AdjusterUserId = adjusterIds(Rng.Next(adjusterIds.Length))
            End If
            If NeedsUnderwriter(status) Then
                claim.UnderwriterId = underwriterIds(Rng.Next(underwriterIds.Count))
            End If

            Dim claimId As Integer = InsertBackdated(claim)

            ' Opening entry, dated when the claim actually came in.
            AddHistory(claimId, String.Empty, ClaimStatus.Notified,
                       "Claim created from " & IntakeRoute(), notified)

            Dim concludedOn As Date? = Nothing
            If IsConcluded(status) Then
                Dim lead As Integer = 12 + Rng.Next(70)
                Dim changed As Date = notified.AddDays(lead)
                If changed > windowEnd Then changed = windowEnd
                concludedOn = changed

                AddHistory(claimId, PriorStatusFor(status), status,
                           ConclusionReason(status), changed)
            End If

            ' Payments, for the ones that actually paid out.
            If status = ClaimStatus.Settled Then
                Dim paidOn As Date = If(concludedOn.HasValue, concludedOn.Value, notified.AddDays(30))
                Dim settledTotal As Decimal = RoundTo(
                    estimate * CDec(0.8R + Rng.NextDouble() * 0.2R), 10)

                If settledTotal > 6000D AndAlso Rng.Next(100) < 55 Then
                    Dim interim As Decimal = RoundTo(settledTotal * 0.4D, 10)
                    AddPayment(claimId, PaymentType.Interim, interim, insured,
                               paidOn.AddDays(-Rng.Next(10, 30)))
                    AddPayment(claimId, PaymentType.Final, settledTotal - interim, insured, paidOn)
                Else
                    AddPayment(claimId, PaymentType.Final, settledTotal, insured, paidOn)
                End If

                Db.Execute("UPDATE [Claims] SET [SettledAmount] = ? WHERE [ClaimId] = ?",
                           settledTotal, claimId)
            End If

            ' Correspondence on roughly half the file, so the tabs are not bare.
            If Rng.Next(100) < 55 Then
                AddInboundItem(claimId, claim, notified.AddDays(Rng.Next(0, 6)))
            End If

            reference += 1
            policySeed += 1 + Rng.Next(400)
            done += 1

            If progress IsNot Nothing AndAlso done Mod 10 = 0 Then
                progress("Generating claims, " & done.ToString() & " of " &
                         ClaimCount.ToString() & "...",
                         25 + CInt(63 * done / ClaimCount))
            End If
        Next

        ' Leave the counter where the generated references finished.
        Db.Execute("UPDATE [Counters] SET [NextValue] = ? WHERE [CounterName] = ?",
                   reference, "ClaimRef")

        If progress IsNot Nothing Then progress("Filling the intake queue...", 92)
        SeedIntakeQueue()
    End Sub

#Region "Writers"

    ''' <summary>
    ''' Inserts with the created and updated stamps backdated to the notification date.
    ''' ClaimRepository.Insert would stamp everything with today, which would make a
    ''' ten month old claim look like it was opened this morning.
    ''' </summary>
    Private Function InsertBackdated(claim As Claim) As Integer
        Const sql As String =
            "INSERT INTO [Claims] ([ClaimRef],[PolicyNumber],[InsuredName],[InsuredPostcode]," &
            "[ContactPhone],[ContactEmail],[DateOfLoss],[DateNotified],[Peril],[LossDescription]," &
            "[EstimatedValue],[ReserveAmount],[SettledAmount],[Status],[HandlerUserId]," &
            "[AdjusterUserId],[UnderwriterId],[CreatedAt],[CreatedBy],[UpdatedAt],[UpdatedBy]) " &
            "VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)"

        Dim stamp As Date = claim.DateNotified.AddHours(9 + Rng.Next(8)).AddMinutes(Rng.Next(60))

        Return Db.InsertAndGetId(sql,
            claim.ClaimRef, claim.PolicyNumber, claim.InsuredName, claim.InsuredPostcode,
            claim.ContactPhone, claim.ContactEmail,
            If(claim.DateOfLoss.HasValue, CType(claim.DateOfLoss.Value, Object), Db.NullDate()),
            claim.DateNotified, claim.Peril, claim.LossDescription,
            claim.EstimatedValue, claim.ReserveAmount, 0D, claim.Status,
            If(claim.HandlerUserId.HasValue, CType(claim.HandlerUserId.Value, Object), Db.NullInteger()),
            If(claim.AdjusterUserId.HasValue, CType(claim.AdjusterUserId.Value, Object), Db.NullInteger()),
            If(claim.UnderwriterId.HasValue, CType(claim.UnderwriterId.Value, Object), Db.NullInteger()),
            stamp, "system", stamp, "system")
    End Function

    ''' <summary>
    ''' Writes history directly so the timestamp is right first time. Inserting and then
    ''' correcting the date would need a subquery in an UPDATE, which Jet often refuses
    ''' with "operation must use an updateable query".
    ''' </summary>
    Private Sub AddHistory(claimId As Integer, fromStatus As String, toStatus As String,
                           rationale As String, changedAt As Date)
        Dim stamp As Date = changedAt.AddHours(9 + Rng.Next(8)).AddMinutes(Rng.Next(60))
        Db.Execute(
            "INSERT INTO [ClaimStatusHistory] ([ClaimId],[FromStatus],[ToStatus]," &
            "[Rationale],[ChangedAt],[ChangedBy]) VALUES (?,?,?,?,?,?)",
            claimId, fromStatus, toStatus, rationale, stamp, "system")
    End Sub

    Private Sub AddPayment(claimId As Integer, kind As String, amount As Decimal,
                           payee As String, paidOn As Date)
        Db.Execute(
            "INSERT INTO [Payments] ([ClaimId],[PaymentKind],[Amount],[Payee],[Reference]," &
            "[PaidAt],[CreatedBy]) VALUES (?,?,?,?,?,?,?)",
            claimId, kind, amount, payee,
            "BACS/" & Rng.Next(100000, 999999).ToString(), paidOn, "system")
    End Sub

    Private Sub AddInboundItem(claimId As Integer, claim As Claim, receivedOn As Date)
        Dim route As String = IntakeRoute()
        Dim contact As String = If(route = CorrespondenceSource.Fax,
                                   Phone(), claim.ContactEmail)

        Db.Execute(
            "INSERT INTO [Correspondence] ([ClaimId],[Direction],[Source],[ReceivedAt]," &
            "[FromName],[FromAddress],[Subject],[Body],[CreatedAt],[CreatedBy]) " &
            "VALUES (?,?,?,?,?,?,?,?,?,?)",
            claimId, CorrespondenceDirection.Inbound, route,
            receivedOn.AddHours(8 + Rng.Next(9)),
            claim.InsuredName, contact,
            "Policy " & claim.PolicyNumber & " - " & claim.Peril,
            "Further to the notification, please find the information requested." & vbCrLf & vbCrLf &
            "Date of loss was " &
            If(claim.DateOfLoss.HasValue, claim.DateOfLoss.Value.ToString("dd/MM/yyyy"), "as reported") &
            ". Policy number " & claim.PolicyNumber & "." & vbCrLf & vbCrLf &
            "Regards",
            receivedOn, "system")
    End Sub

    ''' <summary>
    ''' Items sitting unmatched in the intake queue. Two of them quote the policy number of
    ''' a real claim so the matching flow finds something; the rest look like new losses.
    ''' </summary>
    Private Sub SeedIntakeQueue()
        Dim existing As DataTable = Db.GetTable(
            "SELECT TOP 3 [PolicyNumber], [InsuredName], [DateOfLoss] FROM [Claims] " &
            "WHERE [Status] <> ? ORDER BY [DateNotified] DESC", ClaimStatus.Closed)

        For Each row As DataRow In existing.Rows
            Dim policy As String = Db.AsString(row("PolicyNumber"))
            Dim who As String = Db.AsString(row("InsuredName"))
            Dim loss As Date? = Db.AsNullableDate(row("DateOfLoss"))

            AddUnmatched(CorrespondenceSource.Email, who, EmailFor(who, True),
                "Documents for policy " & policy,
                "Please find attached the receipts and photographs you asked for." & vbCrLf & vbCrLf &
                "Policy number is " & policy & " and the date of loss was " &
                If(loss.HasValue, loss.Value.ToString("dd/MM/yyyy"), "last month") & "." & vbCrLf & vbCrLf &
                "Regards",
                Date.Today.AddDays(-Rng.Next(1, 6)))
        Next

        AddUnmatched(CorrespondenceSource.Fax, "Marsh & Cotter Loss Assessors", "0117 496 7788",
            "Inspection report - 4 pages",
            "Fax cover sheet plus 4 pages. Inspection carried out this week. Report concludes " &
            "damage consistent with a sudden failure rather than gradual seepage.",
            Date.Today.AddDays(-2))

        AddUnmatched(CorrespondenceSource.Email, "Netherby Interiors Ltd",
            "office@netherbyinteriors.example",
            "New claim - water damage to showroom",
            "We need to report a new claim. A pipe failed in the ceiling above our showroom " &
            "over the weekend and we have damage to flooring and to display stock." & vbCrLf & vbCrLf &
            "We have not had a claim before so I am not sure what you need from us.",
            Date.Today.AddDays(-1))

        AddUnmatched(CorrespondenceSource.Phone, "Bridget Tobin", "0161 496 2210",
            "Telephone notification - break in",
            "Insured telephoned to report a break in at the unit discovered this morning. " &
            "Rear door forced. Police called and a crime reference is being obtained. " &
            "Tools and a quantity of stock taken, value not yet known.",
            Date.Today)
    End Sub

    Private Sub AddUnmatched(source As String, fromName As String, fromAddress As String,
                             subject As String, body As String, receivedOn As Date)
        Db.Execute(
            "INSERT INTO [Correspondence] ([ClaimId],[Direction],[Source],[ReceivedAt]," &
            "[FromName],[FromAddress],[Subject],[Body],[CreatedAt],[CreatedBy]) " &
            "VALUES (?,?,?,?,?,?,?,?,?,?)",
            Db.NullInteger(), CorrespondenceDirection.Inbound, source,
            receivedOn.AddHours(8 + Rng.Next(9)).AddMinutes(Rng.Next(60)),
            fromName, fromAddress, subject, body, receivedOn, "system")
    End Sub

#End Region

#Region "Status shaping"

    ''' <summary>
    ''' Older claims have mostly finished; recent ones are still moving. Without this the
    ''' reports would show a flat, uniform spread that tells you nothing.
    ''' </summary>
    Private Function StatusForAge(ageMonths As Integer) As String
        If ageMonths >= 7 Then
            Return PickWeighted(
                New String() {ClaimStatus.Settled, ClaimStatus.Closed, ClaimStatus.Withdrawn,
                              ClaimStatus.AdjusterRejected, ClaimStatus.UnderwriterAgreed},
                New Integer() {48, 28, 9, 9, 6})
        ElseIf ageMonths >= 4 Then
            Return PickWeighted(
                New String() {ClaimStatus.Settled, ClaimStatus.UnderwriterAgreed,
                              ClaimStatus.SentToUnderwriter, ClaimStatus.AdjusterApproved,
                              ClaimStatus.Closed, ClaimStatus.WithAdjuster,
                              ClaimStatus.UnderwriterDeclined, ClaimStatus.Withdrawn},
                New Integer() {28, 14, 16, 11, 13, 9, 5, 4})
        ElseIf ageMonths >= 2 Then
            Return PickWeighted(
                New String() {ClaimStatus.WithAdjuster, ClaimStatus.AdjusterApproved,
                              ClaimStatus.SentToUnderwriter, ClaimStatus.DocumentsPending,
                              ClaimStatus.ReferredForInfo, ClaimStatus.UnderwriterAgreed,
                              ClaimStatus.AdjusterRejected},
                New Integer() {27, 19, 19, 14, 9, 7, 5})
        Else
            Return PickWeighted(
                New String() {ClaimStatus.Notified, ClaimStatus.DocumentsPending,
                              ClaimStatus.WithAdjuster, ClaimStatus.ReferredForInfo},
                New Integer() {38, 30, 26, 6})
        End If
    End Function

    Private Function NeedsAdjuster(status As String) As Boolean
        Select Case status
            Case ClaimStatus.Notified, ClaimStatus.DocumentsPending, ClaimStatus.Withdrawn
                Return Rng.Next(100) < 15
            Case Else
                Return True
        End Select
    End Function

    Private Function NeedsUnderwriter(status As String) As Boolean
        Select Case status
            Case ClaimStatus.SentToUnderwriter, ClaimStatus.UnderwriterAgreed,
                 ClaimStatus.UnderwriterDeclined, ClaimStatus.Settled, ClaimStatus.Closed
                Return True
            Case Else
                Return Rng.Next(100) < 20
        End Select
    End Function

    Private Function IsConcluded(status As String) As Boolean
        Select Case status
            Case ClaimStatus.Settled, ClaimStatus.Closed, ClaimStatus.Withdrawn,
                 ClaimStatus.AdjusterRejected, ClaimStatus.UnderwriterDeclined,
                 ClaimStatus.UnderwriterAgreed
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Function PriorStatusFor(status As String) As String
        Select Case status
            Case ClaimStatus.Settled : Return ClaimStatus.UnderwriterAgreed
            Case ClaimStatus.UnderwriterAgreed, ClaimStatus.UnderwriterDeclined
                Return ClaimStatus.SentToUnderwriter
            Case ClaimStatus.AdjusterRejected : Return ClaimStatus.WithAdjuster
            Case ClaimStatus.Withdrawn : Return ClaimStatus.DocumentsPending
            Case Else : Return ClaimStatus.SentToUnderwriter
        End Select
    End Function

    Private Function ConclusionReason(status As String) As String
        Select Case status
            Case ClaimStatus.Settled
                Return "Settlement agreed with the insured and payment released."
            Case ClaimStatus.Closed
                Return "Nothing further outstanding on the file."
            Case ClaimStatus.Withdrawn
                Return "Insured confirmed they no longer wish to pursue the claim."
            Case ClaimStatus.AdjusterRejected
                Return "Damage not consistent with an insured peril under the policy."
            Case ClaimStatus.UnderwriterDeclined
                Return "Underwriter declined, breach of a policy condition."
            Case ClaimStatus.UnderwriterAgreed
                Return "Underwriter agreed to deal with the claim as presented."
            Case Else
                Return "Status updated."
        End Select
    End Function

#End Region

#Region "Value and name helpers"

    Private Function PickWeighted(items As String(), weights As Integer()) As String
        Dim total As Integer = weights.Sum()
        Dim roll As Integer = Rng.Next(total)
        Dim running As Integer = 0

        For i As Integer = 0 To items.Length - 1
            running += weights(i)
            If roll < running Then Return items(i)
        Next

        Return items(items.Length - 1)
    End Function

    ''' <summary>Most claims are small and a few are very large, so the draw is skewed
    ''' towards the bottom of the band rather than spread evenly across it.</summary>
    Private Function SkewedAmount(low As Integer, high As Integer) As Decimal
        Dim skew As Double = Math.Pow(Rng.NextDouble(), 2.4R)
        Dim raw As Double = low + (high - low) * skew
        Return RoundTo(CDec(raw), 50)
    End Function

    Private Function RoundTo(value As Decimal, nearest As Integer) As Decimal
        If nearest <= 0 Then Return Decimal.Round(value, 2)
        Return CDec(Math.Round(value / nearest) * nearest)
    End Function

    Private Function MonthsBetween(earlier As Date, later As Date) As Integer
        Return (later.Year - earlier.Year) * 12 + (later.Month - earlier.Month)
    End Function

    Private Function BusinessName() As String
        Dim suffix As String = BusinessSuffix(Rng.Next(BusinessSuffix.Length))
        Dim name As String = BusinessPlace(Rng.Next(BusinessPlace.Length)) & " " &
                             BusinessTrade(Rng.Next(BusinessTrade.Length))
        If suffix.Length > 0 Then name &= " " & suffix
        Return name
    End Function

    Private Function PersonName() As String
        Return FirstNames(Rng.Next(FirstNames.Length)) & " " &
               Surnames(Rng.Next(Surnames.Length))
    End Function

    Private Function Postcode() As String
        Return PostcodeAreas(Rng.Next(PostcodeAreas.Length)) &
               Rng.Next(1, 40).ToString() & " " & Rng.Next(1, 10).ToString() &
               Letters(Rng.Next(Letters.Length)) & Letters(Rng.Next(Letters.Length))
    End Function

    Private Function Phone() As String
        Return PhoneAreas(Rng.Next(PhoneAreas.Length)) & " " &
               Rng.Next(100, 1000).ToString() & " " & Rng.Next(1000, 10000).ToString()
    End Function

    Private Function PolicyNumber(seed As Integer) As String
        Dim prefixes As String() = {"NGC", "EM", "CR", "NGP", "EMC"}
        Return prefixes(Rng.Next(prefixes.Length)) & "/" & seed.ToString("000000")
    End Function

    Private Function EmailFor(name As String, isBusiness As Boolean) As String
        Dim cleaned As String = String.Empty
        For Each ch As Char In name.ToLowerInvariant()
            If Char.IsLetterOrDigit(ch) Then cleaned &= ch
        Next
        If cleaned.Length > 22 Then cleaned = cleaned.Substring(0, 22)
        If cleaned.Length = 0 Then cleaned = "contact"

        If isBusiness Then Return "accounts@" & cleaned & ".example"
        Return cleaned & "@example.com"
    End Function

    Private Function IntakeRoute() As String
        Return PickWeighted(
            New String() {CorrespondenceSource.Email, CorrespondenceSource.Fax,
                          CorrespondenceSource.Phone, CorrespondenceSource.Post},
            New Integer() {58, 18, 18, 6})
    End Function

#End Region

End Module
