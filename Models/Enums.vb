Option Strict On

''' <summary>Claim lifecycle states. Stored as text in the database so the values
''' remain readable when someone inevitably opens the .mdb in Access.</summary>
Public NotInheritable Class ClaimStatus
    Private Sub New()
    End Sub

    Public Const Notified As String = "Notified"
    Public Const DocumentsPending As String = "Documents Pending"
    Public Const WithAdjuster As String = "With Adjuster"
    Public Const ReferredForInfo As String = "Referred For Info"
    Public Const AdjusterApproved As String = "Adjuster Approved"
    Public Const AdjusterRejected As String = "Adjuster Rejected"
    Public Const SentToUnderwriter As String = "Sent To Underwriter"
    Public Const UnderwriterAgreed As String = "Underwriter Agreed"
    Public Const UnderwriterDeclined As String = "Underwriter Declined"
    Public Const Settled As String = "Settled"
    Public Const Closed As String = "Closed"
    Public Const Withdrawn As String = "Withdrawn"

    Public Shared ReadOnly All As String() = {
        Notified, DocumentsPending, WithAdjuster, ReferredForInfo,
        AdjusterApproved, AdjusterRejected, SentToUnderwriter,
        UnderwriterAgreed, UnderwriterDeclined, Settled, Closed, Withdrawn
    }

    ''' <summary>Statuses that still need someone to do something.</summary>
    Public Shared ReadOnly Open As String() = {
        Notified, DocumentsPending, WithAdjuster, ReferredForInfo,
        AdjusterApproved, SentToUnderwriter, UnderwriterAgreed
    }
End Class

Public NotInheritable Class UserRole
    Private Sub New()
    End Sub

    Public Const Handler As String = "Handler"
    Public Const Adjuster As String = "Adjuster"
    Public Const Supervisor As String = "Supervisor"

    Public Shared ReadOnly All As String() = {Handler, Adjuster, Supervisor}
End Class

Public NotInheritable Class CorrespondenceSource
    Private Sub New()
    End Sub

    Public Const Email As String = "Email"
    Public Const Fax As String = "Fax"
    Public Const Post As String = "Post"
    Public Const Phone As String = "Phone"

    Public Shared ReadOnly All As String() = {Email, Fax, Post, Phone}
End Class

Public NotInheritable Class CorrespondenceDirection
    Private Sub New()
    End Sub

    Public Const Inbound As String = "Inbound"
    Public Const Outbound As String = "Outbound"

    Public Shared ReadOnly All As String() = {Inbound, Outbound}
End Class

Public NotInheritable Class DocType
    Private Sub New()
    End Sub

    Public Const Photo As String = "Photo"
    Public Const Receipt As String = "Receipt"
    Public Const Invoice As String = "Invoice"
    Public Const Estimate As String = "Estimate"
    Public Const Report As String = "Report"
    Public Const Correspondence As String = "Correspondence"
    Public Const PolicyDocument As String = "Policy Document"
    Public Const Other As String = "Other"

    Public Shared ReadOnly All As String() = {
        Photo, Receipt, Invoice, Estimate, Report,
        Correspondence, PolicyDocument, Other
    }
End Class

Public NotInheritable Class Peril
    Private Sub New()
    End Sub

    Public Shared ReadOnly All As String() = {
        "Escape of Water", "Fire", "Storm", "Flood", "Theft",
        "Malicious Damage", "Accidental Damage", "Impact",
        "Subsidence", "Public Liability", "Employers Liability",
        "Business Interruption", "Other"
    }
End Class

Public NotInheritable Class PaymentType
    Private Sub New()
    End Sub

    Public Const Interim As String = "Interim"
    Public Const Final As String = "Final"
    Public Const Fees As String = "Fees"
    Public Const Recovery As String = "Recovery"

    Public Shared ReadOnly All As String() = {Interim, Final, Fees, Recovery}
End Class
