Option Strict On

Public Class WorkflowTransition
    Public Property ToStatus As String = String.Empty
    Public Property Caption As String = String.Empty
    Public Property Roles As String() = New String() {}
    Public Property RequiresRationale As Boolean

    Public Overrides Function ToString() As String
        Return Caption
    End Function
End Class

''' <summary>
''' The single place that decides what may happen to a claim next. Forms ask this module
''' what to offer; they never move a claim on their own.
''' </summary>
Public Module ClaimWorkflow

    Private ReadOnly Map As Dictionary(Of String, List(Of WorkflowTransition)) = BuildMap()

    Private Function BuildMap() As Dictionary(Of String, List(Of WorkflowTransition))
        Dim m As New Dictionary(Of String, List(Of WorkflowTransition))(StringComparer.OrdinalIgnoreCase)

        Dim handlerOrSupervisor As String() = {UserRole.Handler, UserRole.Supervisor}
        Dim adjusterOrSupervisor As String() = {UserRole.Adjuster, UserRole.Supervisor}
        Dim supervisorOnly As String() = {UserRole.Supervisor}

        Add(m, ClaimStatus.Notified, ClaimStatus.DocumentsPending,
            "Request further documents", handlerOrSupervisor, True)
        Add(m, ClaimStatus.Notified, ClaimStatus.WithAdjuster,
            "Refer to loss adjuster", handlerOrSupervisor, False)
        Add(m, ClaimStatus.Notified, ClaimStatus.Withdrawn,
            "Withdraw claim", handlerOrSupervisor, True)

        Add(m, ClaimStatus.DocumentsPending, ClaimStatus.WithAdjuster,
            "Refer to loss adjuster", handlerOrSupervisor, False)
        Add(m, ClaimStatus.DocumentsPending, ClaimStatus.Withdrawn,
            "Withdraw claim", handlerOrSupervisor, True)

        Add(m, ClaimStatus.WithAdjuster, ClaimStatus.AdjusterApproved,
            "Approve claim", adjusterOrSupervisor, True)
        Add(m, ClaimStatus.WithAdjuster, ClaimStatus.AdjusterRejected,
            "Reject claim", adjusterOrSupervisor, True)
        Add(m, ClaimStatus.WithAdjuster, ClaimStatus.ReferredForInfo,
            "Refer back for more information", adjusterOrSupervisor, True)

        Add(m, ClaimStatus.ReferredForInfo, ClaimStatus.WithAdjuster,
            "Return to adjuster", handlerOrSupervisor, False)
        Add(m, ClaimStatus.ReferredForInfo, ClaimStatus.Withdrawn,
            "Withdraw claim", handlerOrSupervisor, True)

        Add(m, ClaimStatus.AdjusterApproved, ClaimStatus.SentToUnderwriter,
            "Send to underwriter", handlerOrSupervisor, False)
        Add(m, ClaimStatus.AdjusterApproved, ClaimStatus.WithAdjuster,
            "Send back to adjuster", supervisorOnly, True)

        Add(m, ClaimStatus.AdjusterRejected, ClaimStatus.Closed,
            "Close claim", handlerOrSupervisor, True)
        Add(m, ClaimStatus.AdjusterRejected, ClaimStatus.WithAdjuster,
            "Reopen with adjuster", supervisorOnly, True)

        Add(m, ClaimStatus.SentToUnderwriter, ClaimStatus.UnderwriterAgreed,
            "Record underwriter agreement", handlerOrSupervisor, False)
        Add(m, ClaimStatus.SentToUnderwriter, ClaimStatus.UnderwriterDeclined,
            "Record underwriter decline", handlerOrSupervisor, True)

        Add(m, ClaimStatus.UnderwriterAgreed, ClaimStatus.Settled,
            "Record settlement", handlerOrSupervisor, False)

        Add(m, ClaimStatus.UnderwriterDeclined, ClaimStatus.Closed,
            "Close claim", handlerOrSupervisor, False)
        Add(m, ClaimStatus.UnderwriterDeclined, ClaimStatus.WithAdjuster,
            "Reopen with adjuster", supervisorOnly, True)

        Add(m, ClaimStatus.Settled, ClaimStatus.Closed,
            "Close claim", handlerOrSupervisor, False)

        Return m
    End Function

    Private Sub Add(m As Dictionary(Of String, List(Of WorkflowTransition)),
                    fromStatus As String, toStatus As String, caption As String,
                    roles As String(), requiresRationale As Boolean)
        If Not m.ContainsKey(fromStatus) Then
            m(fromStatus) = New List(Of WorkflowTransition)()
        End If
        m(fromStatus).Add(New WorkflowTransition With {
            .ToStatus = toStatus,
            .Caption = caption,
            .Roles = roles,
            .RequiresRationale = requiresRationale
        })
    End Sub

    ''' <summary>Every transition out of a status, regardless of who is signed in.</summary>
    Public Function TransitionsFrom(status As String) As List(Of WorkflowTransition)
        If String.IsNullOrEmpty(status) OrElse Not Map.ContainsKey(status) Then
            Return New List(Of WorkflowTransition)()
        End If
        Return Map(status)
    End Function

    ''' <summary>The transitions this role is allowed to perform.</summary>
    Public Function AvailableFor(status As String, role As String) As List(Of WorkflowTransition)
        Return TransitionsFrom(status).Where(
            Function(t) t.Roles.Any(
                Function(r) String.Equals(r, role, StringComparison.OrdinalIgnoreCase))).ToList()
    End Function

    ''' <summary>
    ''' Business rules that must hold before a claim can move on. Returns an empty string
    ''' when the move is allowed, otherwise the reason it is blocked.
    ''' </summary>
    Public Function Validate(claim As Claim, toStatus As String) As String
        Select Case toStatus

            Case ClaimStatus.WithAdjuster
                If Not claim.DateOfLoss.HasValue Then
                    Return "Set a date of loss on the Details tab before referring this to an adjuster."
                End If
                If String.IsNullOrWhiteSpace(claim.Peril) Then
                    Return "Choose a peril on the Details tab before referring this to an adjuster."
                End If
                If claim.EstimatedValue <= 0D Then
                    Return "Enter an estimated value on the Details tab before referring this to an adjuster."
                End If
                If Not claim.AdjusterUserId.HasValue Then
                    Return "Assign a loss adjuster on the Details tab before referring this claim."
                End If
                If AttachmentRepository.CountForClaim(claim.ClaimId) = 0 AndAlso
                   CorrespondenceRepository.CountForClaim(claim.ClaimId) = 0 Then
                    Return "An adjuster needs something to review. Attach at least one document " &
                           "or record the inbound correspondence first."
                End If

            Case ClaimStatus.SentToUnderwriter
                If Not claim.UnderwriterId.HasValue Then
                    Return "Choose an underwriter on the Details tab before sending this claim out."
                End If
                If claim.ReserveAmount <= 0D Then
                    Return "Set a reserve on the Details tab before sending this claim to the underwriter."
                End If

            Case ClaimStatus.Settled
                If PaymentRepository.TotalForClaim(claim.ClaimId) <= 0D Then
                    Return "Record the payment on the Payments tab before marking this claim settled."
                End If

        End Select

        Return String.Empty
    End Function

    ''' <summary>
    ''' Moves the claim, writes the status history row and the audit entry. Call Validate
    ''' first; this method assumes the move has already been checked and permitted.
    ''' </summary>
    Public Sub Apply(claim As Claim, transition As WorkflowTransition,
                     rationale As String, userName As String)
        Dim fromStatus As String = claim.Status

        claim.Status = transition.ToStatus
        If transition.ToStatus = ClaimStatus.Settled Then
            claim.SettledAmount = PaymentRepository.TotalForClaim(claim.ClaimId)
        End If
        ClaimRepository.Update(claim, userName)

        StatusHistoryRepository.Insert(claim.ClaimId, fromStatus,
                                      transition.ToStatus, rationale, userName)
        AuditRepository.Write("Claim", claim.ClaimId, "Status changed",
                              fromStatus & " -> " & transition.ToStatus, userName)
    End Sub

End Module
