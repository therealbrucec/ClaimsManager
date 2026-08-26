Option Strict On

''' <summary>
''' What each role may do, as a single ordered list so the demo accounts file and any
''' documentation say the same thing.
'''
''' The workflow rights are not retyped here: they are asked of ClaimWorkflow, which is what
''' the application actually enforces, so this cannot drift from the real behaviour. Only the
''' screen-level rights - recording post, creating and editing claims - are stated directly,
''' because those live in the forms as Session.IsInRole checks rather than in the state machine.
''' </summary>
Public Module RolePermissions

    ''' <summary>Column headings, in the order ForRole returns its answers.</summary>
    Public Function Labels() As String()
        Return New String() {
            "Record inbound item",
            "Match item to claim",
            "Create claim",
            "Edit claim details",
            "Refer to adjuster",
            "Approve, reject or refer back",
            "Send to underwriter",
            "Reopen or reverse a decision",
            "Add notes"
        }
    End Function

    ''' <summary>True or False for each label above, for the role given.</summary>
    Public Function ForRole(role As String) As Boolean()
        ' The handler's side of the desk: intake, and ownership of the claim record.
        Dim deskSide As Boolean = IsRole(role, UserRole.Handler, UserRole.Supervisor)

        Return New Boolean() {
            deskSide,
            deskSide,
            deskSide,
            deskSide,
            CanMove(role, ClaimStatus.Notified, ClaimStatus.WithAdjuster),
            CanDecideAsAdjuster(role),
            CanMove(role, ClaimStatus.AdjusterApproved, ClaimStatus.SentToUnderwriter),
            CanMove(role, ClaimStatus.AdjusterRejected, ClaimStatus.WithAdjuster),
            True
        }
    End Function

    ''' <summary>One line describing the role, for the same file.</summary>
    Public Function SummaryFor(role As String) As String
        Select Case role
            Case UserRole.Handler
                Return "Owns the file from first contact to closure. Captures post, decides " &
                       "new or existing, chases documents, submits to the underwriter and pays."
            Case UserRole.Adjuster
                Return "Reviews the evidence and makes the technical call: approve, reject or " &
                       "refer back. Reads the file rather than writing it."
            Case UserRole.Supervisor
                Return "Everything a handler can do, plus the only route back: returning an " &
                       "approved claim to the adjuster, or reopening one already rejected."
            Case Else
                Return "Unrecognised role."
        End Select
    End Function

    ''' <summary>Can this role make that particular move, according to the state machine?</summary>
    Private Function CanMove(role As String, fromStatus As String, toStatus As String) As Boolean
        Return ClaimWorkflow.AvailableFor(fromStatus, role).Any(
            Function(t) String.Equals(t.ToStatus, toStatus, StringComparison.OrdinalIgnoreCase))
    End Function

    ''' <summary>Any of the three decisions available out of With Adjuster.</summary>
    Private Function CanDecideAsAdjuster(role As String) As Boolean
        Return ClaimWorkflow.AvailableFor(ClaimStatus.WithAdjuster, role).Count > 0
    End Function

    Private Function IsRole(role As String, ParamArray candidates As String()) As Boolean
        For Each candidate As String In candidates
            If String.Equals(candidate, role, StringComparison.OrdinalIgnoreCase) Then Return True
        Next
        Return False
    End Function

End Module
