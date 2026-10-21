Option Strict On
Imports System.Windows.Forms

Public Class frmMain

    ''' <summary>The running shell, so child forms can ask for a claim to be opened
    ''' without holding their own reference to it.</summary>
    Public Shared Property Instance As frmMain

    Public Sub New()
        InitializeComponent()
        UiStyle.ApplyTo(Me)
        AppHelp.Attach(Me, AppHelp.MainWindow)
        Instance = Me
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblStatusUser.Text = Session.FullName
        lblStatusRole.Text = Session.Role
        Text = "Northbridge Assurance - Claims Manager - " & Session.FullName & " (" & Session.Role & ")"

        ' An adjuster has no business creating claims or filing post.
        Dim isHandlerSide As Boolean = Session.IsInRole(UserRole.Handler, UserRole.Supervisor)
        mnuNewClaim.Enabled = isHandlerSide
        mnuRecordInbound.Enabled = isHandlerSide
        tsNewClaim.Enabled = isHandlerSide
        tsRecordInbound.Enabled = isHandlerSide

        RefreshIntakeCount()

        If Session.IsInRole(UserRole.Adjuster) Then
            ShowAdjusterQueue()
        Else
            ShowMyClaims()
        End If
    End Sub

    ''' <summary>Called by the intake screens whenever the queue changes.</summary>
    Public Sub RefreshIntakeCount()
        Try
            Dim waiting As Integer = CorrespondenceRepository.CountUnmatched()
            lblStatusUnmatched.Text = If(waiting = 0,
                "Intake queue clear",
                waiting.ToString() & " item(s) waiting to be matched")
        Catch ex As Exception
            lblStatusUnmatched.Text = "Intake count unavailable"
        End Try
    End Sub

#Region "Window management"

    Private Function FindOpen(Of T As Form)(Optional key As String = "") As T
        For Each child As Form In MdiChildren
            If TypeOf child Is T Then
                Dim childKey As String = If(child.Tag Is Nothing, String.Empty, child.Tag.ToString())
                If key.Length = 0 OrElse childKey = key Then
                    Return DirectCast(child, T)
                End If
            End If
        Next
        Return Nothing
    End Function

    Private Sub ShowChild(child As Form, Optional key As String = "")
        child.MdiParent = Me
        child.Tag = key
        child.Show()
        child.BringToFront()
    End Sub

    ''' <summary>Opens a claim, or brings it forward if it is already on screen.</summary>
    Public Sub OpenClaim(claimId As Integer)
        Dim key As String = claimId.ToString()
        Dim existing As frmClaim = FindOpen(Of frmClaim)(key)
        If existing IsNot Nothing Then
            existing.Activate()
            Return
        End If

        Try
            ShowChild(New frmClaim(claimId), key)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Public Sub OpenNewClaim(Optional prefillFrom As Correspondence = Nothing)
        ShowChild(New frmClaim(prefillFrom), "new-" & Guid.NewGuid().ToString("N"))
    End Sub

    Public Sub ShowUnmatchedQueue()
        Dim existing As frmUnmatched = FindOpen(Of frmUnmatched)()
        If existing IsNot Nothing Then
            existing.Activate()
            existing.LoadQueue()
            Return
        End If
        ShowChild(New frmUnmatched())
    End Sub

    Private Sub ShowMyClaims()
        Dim mine As Integer? = Nothing
        If Session.CurrentUser IsNot Nothing Then mine = Session.CurrentUser.UserId
        ShowChild(New frmClaimList("My claims", ClaimListMode.MineAsHandler, mine), "list-mine")
    End Sub

    Private Sub ShowAdjusterQueue()
        Dim mine As Integer? = Nothing
        If Session.IsInRole(UserRole.Adjuster) AndAlso Session.CurrentUser IsNot Nothing Then
            mine = Session.CurrentUser.UserId
        End If
        ShowChild(New frmClaimList("Adjuster review queue", ClaimListMode.AdjusterQueue, mine),
                  "list-adjuster")
    End Sub

#End Region

#Region "Menu and toolbar"

    Private Sub mnuNewClaim_Click(sender As Object, e As EventArgs) _
        Handles mnuNewClaim.Click, tsNewClaim.Click
        OpenNewClaim()
    End Sub

    Private Sub mnuRecordInbound_Click(sender As Object, e As EventArgs) _
        Handles mnuRecordInbound.Click, tsRecordInbound.Click
        Using intake As New frmIntake()
            intake.ShowDialog(Me)
        End Using
        RefreshIntakeCount()
        ShowUnmatchedQueue()
    End Sub

    Private Sub mnuUnmatchedQueue_Click(sender As Object, e As EventArgs) _
        Handles mnuUnmatchedQueue.Click, tsUnmatched.Click
        ShowUnmatchedQueue()
    End Sub

    Private Sub mnuMyClaims_Click(sender As Object, e As EventArgs) _
        Handles mnuMyClaims.Click, tsMyClaims.Click
        Dim existing As frmClaimList = FindOpen(Of frmClaimList)("list-mine")
        If existing IsNot Nothing Then
            existing.Activate()
            existing.LoadClaims()
            Return
        End If
        ShowMyClaims()
    End Sub

    Private Sub mnuAllClaims_Click(sender As Object, e As EventArgs) Handles mnuAllClaims.Click
        Dim existing As frmClaimList = FindOpen(Of frmClaimList)("list-all")
        If existing IsNot Nothing Then
            existing.Activate()
            existing.LoadClaims()
            Return
        End If
        ShowChild(New frmClaimList("All claims", ClaimListMode.All, Nothing), "list-all")
    End Sub

    Private Sub mnuAdjusterQueue_Click(sender As Object, e As EventArgs) _
        Handles mnuAdjusterQueue.Click, tsAdjusterQueue.Click
        Dim existing As frmClaimList = FindOpen(Of frmClaimList)("list-adjuster")
        If existing IsNot Nothing Then
            existing.Activate()
            existing.LoadClaims()
            Return
        End If
        ShowAdjusterQueue()
    End Sub

    Private Sub mnuOpenByRef_Click(sender As Object, e As EventArgs) Handles mnuOpenByRef.Click
        Dim entered As String = InputBox("Claim reference, for example CLM-2026-00001:",
                                         "Open claim").Trim()
        If entered.Length = 0 Then Return

        Try
            Dim claim As Claim = ClaimRepository.GetByRef(entered.ToUpperInvariant())
            If claim Is Nothing Then
                UiStyle.ShowProblem(Me, "No claim is filed under " & entered & ". " &
                                        "Check the reference, or search the claim list.")
                Return
            End If
            OpenClaim(claim.ClaimId)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub mnuExportReports_Click(sender As Object, e As EventArgs) _
        Handles mnuExportReports.Click
        Using reports As New frmReports()
            reports.ShowDialog(Me)
        End Using
    End Sub

    Private Sub mnuCloseAll_Click(sender As Object, e As EventArgs) Handles mnuCloseAll.Click
        For Each child As Form In MdiChildren.ToArray()
            child.Close()
        Next
    End Sub

    Private Sub mnuSignOut_Click(sender As Object, e As EventArgs) Handles mnuSignOut.Click
        If MessageBox.Show(Me, "Sign out and return to the sign-in screen?", "Claims Manager",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Application.Restart()
        End If
    End Sub

    Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click
        Close()
    End Sub

    Private Sub mnuHelpContents_Click(sender As Object, e As EventArgs) Handles mnuHelpContents.Click
        AppHelp.ShowContents(Me)
    End Sub

    Private Sub mnuHelpIndex_Click(sender As Object, e As EventArgs) Handles mnuHelpIndex.Click
        AppHelp.ShowIndex(Me)
    End Sub

    Private Sub mnuAbout_Click(sender As Object, e As EventArgs) Handles mnuAbout.Click
        Dim detail As String =
            "Northbridge Assurance" & vbCrLf &
            "Claims Manager - broker claims handling desk" & vbCrLf & vbCrLf &
            "Database: " & Config.DatabasePath & vbCrLf &
            "Documents: " & Config.DocumentRoot & vbCrLf &
            "Sign-in details: " & Config.AccountsFilePath & vbCrLf &
            "Help: " & Config.HelpFilePath

        detail &= vbCrLf & vbCrLf &
                  "All of these sit in the program's own folder. Nothing is written to your profile."

        MessageBox.Show(Me, detail,
            "About Northbridge Assurance Claims Manager",
            MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#End Region

End Class
