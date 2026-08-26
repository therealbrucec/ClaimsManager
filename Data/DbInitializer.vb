' Option Strict is off in this file only, because ADOX is called through late binding.
' That avoids shipping a COM interop reference just to create an empty .mdb.
Option Strict Off
Imports System.IO
Imports System.Runtime.InteropServices

Public Module DbInitializer

    ''' <summary>
    ''' Creates the database on first run. Safe to call on every start-up. The optional
    ''' callback lets the splash screen report what is happening; building a year of demo
    ''' claims takes long enough that silence would look like a hang.
    ''' </summary>
    Public Sub EnsureDatabase(Optional progress As Action(Of String, Integer) = Nothing)
        Config.EnsureFolders()

        If Not File.Exists(Config.DatabasePath) Then
            Report(progress, "Creating the claims database...", 4)
            CreateEmptyDatabase()

            Dim statements As String() = SchemaStatements()
            For i As Integer = 0 To statements.Length - 1
                Db.Execute(statements(i))
                Report(progress, "Building tables and indexes...",
                       5 + CInt(15 * (i + 1) / statements.Length))
            Next

            Report(progress, "Adding users and underwriters...", 22)
            SeedReferenceData()

            DemoData.Seed(progress)
        Else
            Report(progress, "Opening the claims database...", 70)
        End If

        ' Outside the block above on purpose, so deleting the file brings it back.
        Report(progress, "Checking sign-in details...", 96)
        EnsureAccountsFile()

        Report(progress, "Ready", 100)
    End Sub

    Private Sub Report(progress As Action(Of String, Integer), message As String, percent As Integer)
        If progress IsNot Nothing Then progress(message, percent)
    End Sub

    ''' <summary>
    ''' Writes the demo sign-in details next to the database, together with what each role
    ''' is permitted to do. The user name is typed at sign-in rather than chosen from a list,
    ''' so without this there would be nothing to tell you what the accounts are called.
    '''
    ''' Demo build only: it is a plain text file containing a password, which no real system
    ''' should ever produce.
    ''' </summary>
    Private Sub EnsureAccountsFile()
        If File.Exists(Config.AccountsFilePath) Then Return

        Try
            Dim labels As String() = RolePermissions.Labels()

            Dim header As New List(Of String)()
            header.Add("UserName")
            header.Add("FullName")
            header.Add("Role")
            header.Add("Password")
            For Each label As String In labels
                header.Add(CsvField(label))
            Next
            header.Add("Role summary")

            Dim lines As New List(Of String)()
            lines.Add(String.Join(",", header.ToArray()))

            For Each account As AppUser In UserRepository.GetActive()
                Dim allowed As Boolean() = RolePermissions.ForRole(account.Role)

                Dim row As New List(Of String)()
                row.Add(CsvField(account.UserName))
                row.Add(CsvField(account.FullName))
                row.Add(CsvField(account.Role))
                row.Add(CsvField(Session.DemoPassword))
                For i As Integer = 0 To labels.Length - 1
                    row.Add(If(i < allowed.Length AndAlso allowed(i), "Yes", "No"))
                Next
                row.Add(CsvField(RolePermissions.SummaryFor(account.Role)))

                lines.Add(String.Join(",", row.ToArray()))
            Next

            File.WriteAllLines(Config.AccountsFilePath, lines.ToArray())
        Catch ex As Exception
            ' A missing convenience file is not a reason to stop the application starting.
        End Try
    End Sub

    Private Function CsvField(value As String) As String
        Dim text As String = If(value, String.Empty)
        If text.IndexOf(""""c) >= 0 Then text = text.Replace("""", """""")
        If text.IndexOfAny(New Char() {","c, """"c, ControlChars.Cr, ControlChars.Lf}) >= 0 Then
            Return """" & text & """"
        End If
        Return text
    End Function

    Private Sub CreateEmptyDatabase()
        Dim catalog As Object = Nothing
        Try
            catalog = CreateObject("ADOX.Catalog")
            catalog.Create(Config.ConnectionString)
        Catch ex As Exception
            Throw New ApplicationException(
                "Could not create the claims database at " & Config.DatabasePath & "." & vbCrLf & vbCrLf &
                "This needs the 32-bit Jet provider and ADOX, both of which ship with Windows. " &
                "Build the project for x86 and run again.", ex)
        Finally
            If catalog IsNot Nothing Then
                Try
                    If catalog.ActiveConnection IsNot Nothing Then catalog.ActiveConnection.Close()
                Catch
                    ' Nothing useful to do if the ADOX connection is already gone.
                End Try
                Marshal.FinalReleaseComObject(catalog)
            End If
        End Try

        ' ADOX holds the file open until its COM object is collected. Force that now,
        ' otherwise the first OleDb command below fails with a sharing violation.
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Function SchemaStatements() As String()
        Return New String() {
            "CREATE TABLE [AppUsers] (" &
            " [UserId] AUTOINCREMENT PRIMARY KEY," &
            " [UserName] TEXT(50) NOT NULL," &
            " [FullName] TEXT(100) NOT NULL," &
            " [Role] TEXT(20) NOT NULL," &
            " [IsActive] YESNO NOT NULL)",
            "CREATE UNIQUE INDEX [idxAppUserName] ON [AppUsers] ([UserName])",
            "CREATE TABLE [Underwriters] (" &
            " [UnderwriterId] AUTOINCREMENT PRIMARY KEY," &
            " [Name] TEXT(100) NOT NULL," &
            " [SchemeRef] TEXT(30)," &
            " [ContactEmail] TEXT(120)," &
            " [FaxNumber] TEXT(30))",
            "CREATE TABLE [Counters] (" &
            " [CounterName] TEXT(30) NOT NULL PRIMARY KEY," &
            " [NextValue] LONG NOT NULL)",
            "CREATE TABLE [Claims] (" &
            " [ClaimId] AUTOINCREMENT PRIMARY KEY," &
            " [ClaimRef] TEXT(20) NOT NULL," &
            " [PolicyNumber] TEXT(30)," &
            " [InsuredName] TEXT(100)," &
            " [InsuredPostcode] TEXT(12)," &
            " [ContactPhone] TEXT(30)," &
            " [ContactEmail] TEXT(120)," &
            " [DateOfLoss] DATETIME," &
            " [DateNotified] DATETIME," &
            " [Peril] TEXT(40)," &
            " [LossDescription] MEMO," &
            " [EstimatedValue] CURRENCY," &
            " [ReserveAmount] CURRENCY," &
            " [SettledAmount] CURRENCY," &
            " [Status] TEXT(30) NOT NULL," &
            " [HandlerUserId] LONG," &
            " [AdjusterUserId] LONG," &
            " [UnderwriterId] LONG," &
            " [CreatedAt] DATETIME," &
            " [CreatedBy] TEXT(50)," &
            " [UpdatedAt] DATETIME," &
            " [UpdatedBy] TEXT(50))",
            "CREATE UNIQUE INDEX [idxClaimRef] ON [Claims] ([ClaimRef])",
            "CREATE INDEX [idxClaimPolicy] ON [Claims] ([PolicyNumber])",
            "CREATE INDEX [idxClaimStatus] ON [Claims] ([Status])",
            "CREATE INDEX [idxClaimInsured] ON [Claims] ([InsuredName])",
            "CREATE TABLE [Correspondence] (" &
            " [CorrespondenceId] AUTOINCREMENT PRIMARY KEY," &
            " [ClaimId] LONG," &
            " [Direction] TEXT(10) NOT NULL," &
            " [Source] TEXT(10) NOT NULL," &
            " [ReceivedAt] DATETIME," &
            " [FromName] TEXT(150)," &
            " [FromAddress] TEXT(150)," &
            " [Subject] TEXT(255)," &
            " [Body] MEMO," &
            " [CreatedAt] DATETIME," &
            " [CreatedBy] TEXT(50)," &
            " CONSTRAINT [fkCorrespondenceClaim] FOREIGN KEY ([ClaimId])" &
            " REFERENCES [Claims] ([ClaimId]))",
            "CREATE INDEX [idxCorrespondenceClaim] ON [Correspondence] ([ClaimId])",
            "CREATE TABLE [Attachments] (" &
            " [AttachmentId] AUTOINCREMENT PRIMARY KEY," &
            " [ClaimId] LONG," &
            " [CorrespondenceId] LONG," &
            " [FileName] TEXT(255) NOT NULL," &
            " [StoredPath] TEXT(255) NOT NULL," &
            " [DocumentType] TEXT(30)," &
            " [FileSize] LONG," &
            " [AddedAt] DATETIME," &
            " [AddedBy] TEXT(50))",
            "CREATE INDEX [idxAttachmentClaim] ON [Attachments] ([ClaimId])",
            "CREATE INDEX [idxAttachmentCorrespondence] ON [Attachments] ([CorrespondenceId])",
            "CREATE TABLE [ClaimNotes] (" &
            " [NoteId] AUTOINCREMENT PRIMARY KEY," &
            " [ClaimId] LONG NOT NULL," &
            " [NoteText] MEMO NOT NULL," &
            " [IsInternal] YESNO NOT NULL," &
            " [CreatedAt] DATETIME," &
            " [CreatedBy] TEXT(50)," &
            " CONSTRAINT [fkNoteClaim] FOREIGN KEY ([ClaimId])" &
            " REFERENCES [Claims] ([ClaimId]))",
            "CREATE INDEX [idxNoteClaim] ON [ClaimNotes] ([ClaimId])",
            "CREATE TABLE [Payments] (" &
            " [PaymentId] AUTOINCREMENT PRIMARY KEY," &
            " [ClaimId] LONG NOT NULL," &
            " [PaymentKind] TEXT(20) NOT NULL," &
            " [Amount] CURRENCY NOT NULL," &
            " [Payee] TEXT(120)," &
            " [Reference] TEXT(50)," &
            " [PaidAt] DATETIME," &
            " [CreatedBy] TEXT(50)," &
            " CONSTRAINT [fkPaymentClaim] FOREIGN KEY ([ClaimId])" &
            " REFERENCES [Claims] ([ClaimId]))",
            "CREATE INDEX [idxPaymentClaim] ON [Payments] ([ClaimId])",
            "CREATE TABLE [ClaimStatusHistory] (" &
            " [HistoryId] AUTOINCREMENT PRIMARY KEY," &
            " [ClaimId] LONG NOT NULL," &
            " [FromStatus] TEXT(30)," &
            " [ToStatus] TEXT(30) NOT NULL," &
            " [Rationale] MEMO," &
            " [ChangedAt] DATETIME," &
            " [ChangedBy] TEXT(50)," &
            " CONSTRAINT [fkHistoryClaim] FOREIGN KEY ([ClaimId])" &
            " REFERENCES [Claims] ([ClaimId]))",
            "CREATE INDEX [idxHistoryClaim] ON [ClaimStatusHistory] ([ClaimId])",
            "CREATE TABLE [AuditLog] (" &
            " [AuditId] AUTOINCREMENT PRIMARY KEY," &
            " [EntityType] TEXT(30) NOT NULL," &
            " [EntityId] LONG," &
            " [Action] TEXT(40) NOT NULL," &
            " [Detail] MEMO," &
            " [PerformedAt] DATETIME," &
            " [PerformedBy] TEXT(50))",
            "CREATE INDEX [idxAuditEntity] ON [AuditLog] ([EntityType], [EntityId])"
        }
    End Function

    Private Sub SeedReferenceData()
        Db.Execute("INSERT INTO [Counters] ([CounterName],[NextValue]) VALUES (?,?)",
                   "ClaimRef", 1)

        AddUser("jbaker", "Jane Baker", UserRole.Handler)
        AddUser("mokafor", "Michael Okafor", UserRole.Handler)
        AddUser("rpatel", "Rita Patel", UserRole.Adjuster)
        AddUser("dcampbell", "Douglas Campbell", UserRole.Adjuster)
        AddUser("shollis", "Susan Hollis", UserRole.Supervisor)

        AddUnderwriter("Northgate Syndicate", "NG-COM-04",
                       "claims@northgate-syn.example", "020 7000 1234")
        AddUnderwriter("Ellesmere Mutual", "EM-PROP-11",
                       "newclaims@ellesmere.example", "0161 000 5678")
        AddUnderwriter("Caledonian Re", "CR-LIAB-02",
                       "liability@caledonian-re.example", "0141 000 9012")
    End Sub

    Private Sub AddUser(userName As String, fullName As String, role As String)
        Db.Execute("INSERT INTO [AppUsers] ([UserName],[FullName],[Role],[IsActive]) VALUES (?,?,?,?)",
                   userName, fullName, role, True)
    End Sub

    Private Sub AddUnderwriter(name As String, schemeRef As String, email As String, fax As String)
        Db.Execute("INSERT INTO [Underwriters] ([Name],[SchemeRef],[ContactEmail],[FaxNumber]) VALUES (?,?,?,?)",
                   name, schemeRef, email, fax)
    End Sub

End Module
