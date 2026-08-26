Option Strict On

Public Class AppUser
    Public Property UserId As Integer
    Public Property UserName As String = String.Empty
    Public Property FullName As String = String.Empty
    Public Property Role As String = UserRole.Handler
    Public Property IsActive As Boolean = True

    ''' <summary>Used as the combo box display text.</summary>
    Public Overrides Function ToString() As String
        Return FullName & " (" & Role & ")"
    End Function
End Class

Public Class Underwriter
    Public Property UnderwriterId As Integer
    Public Property Name As String = String.Empty
    Public Property SchemeRef As String = String.Empty
    Public Property ContactEmail As String = String.Empty
    Public Property FaxNumber As String = String.Empty

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Class Claim
    Public Property ClaimId As Integer
    Public Property ClaimRef As String = String.Empty
    Public Property PolicyNumber As String = String.Empty
    Public Property InsuredName As String = String.Empty
    Public Property InsuredPostcode As String = String.Empty
    Public Property ContactPhone As String = String.Empty
    Public Property ContactEmail As String = String.Empty
    Public Property DateOfLoss As Date? = Nothing
    Public Property DateNotified As Date = Date.Today
    Public Property Peril As String = String.Empty
    Public Property LossDescription As String = String.Empty
    Public Property EstimatedValue As Decimal
    Public Property ReserveAmount As Decimal
    Public Property SettledAmount As Decimal
    Public Property Status As String = ClaimStatus.Notified
    Public Property HandlerUserId As Integer?
    Public Property AdjusterUserId As Integer?
    Public Property UnderwriterId As Integer?
    Public Property CreatedAt As Date
    Public Property CreatedBy As String = String.Empty
    Public Property UpdatedAt As Date
    Public Property UpdatedBy As String = String.Empty

    Public ReadOnly Property IsNew As Boolean
        Get
            Return ClaimId = 0
        End Get
    End Property

    ''' <summary>Surname is what people actually search on, so derive it once here.</summary>
    Public ReadOnly Property InsuredSurname As String
        Get
            If String.IsNullOrWhiteSpace(InsuredName) Then Return String.Empty
            Dim parts As String() = InsuredName.Trim().Split(" "c)
            Return parts(parts.Length - 1)
        End Get
    End Property
End Class

Public Class Correspondence
    Public Property CorrespondenceId As Integer
    Public Property ClaimId As Integer?
    Public Property Direction As String = CorrespondenceDirection.Inbound
    Public Property Source As String = CorrespondenceSource.Email
    Public Property ReceivedAt As Date = Date.Now
    Public Property FromName As String = String.Empty
    Public Property FromAddress As String = String.Empty
    Public Property Subject As String = String.Empty
    Public Property Body As String = String.Empty
    Public Property CreatedAt As Date
    Public Property CreatedBy As String = String.Empty

    Public ReadOnly Property IsMatched As Boolean
        Get
            Return ClaimId.HasValue
        End Get
    End Property
End Class

Public Class Attachment
    Public Property AttachmentId As Integer
    Public Property ClaimId As Integer?
    Public Property CorrespondenceId As Integer?
    Public Property FileName As String = String.Empty
    Public Property StoredPath As String = String.Empty
    Public Property DocumentType As String = DocType.Other
    Public Property FileSize As Long
    Public Property AddedAt As Date
    Public Property AddedBy As String = String.Empty
End Class

Public Class ClaimNote
    Public Property NoteId As Integer
    Public Property ClaimId As Integer
    Public Property NoteText As String = String.Empty
    Public Property IsInternal As Boolean
    Public Property CreatedAt As Date
    Public Property CreatedBy As String = String.Empty
End Class

Public Class Payment
    Public Property PaymentId As Integer
    Public Property ClaimId As Integer
    Public Property PaymentKind As String = PaymentType.Interim
    Public Property Amount As Decimal
    Public Property Payee As String = String.Empty
    Public Property Reference As String = String.Empty
    Public Property PaidAt As Date = Date.Today
    Public Property CreatedBy As String = String.Empty
End Class
