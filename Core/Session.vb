Option Strict On

''' <summary>Who is signed in. Set once by the sign-in dialog and read everywhere else.</summary>
Public Module Session

    ''' <summary>
    ''' Demoware: one password for every account, held in the source and compared as plain
    ''' text. No hashing, no per-user credential, no lockout. There is nothing behind this
    ''' worth protecting, and a real deployment would not do any of it this way.
    ''' </summary>
    Public Const DemoPassword As String = "Pa55w0rd"

    Public Function PasswordIsCorrect(entered As String) As Boolean
        Return String.Equals(entered, DemoPassword, StringComparison.Ordinal)
    End Function

    Public Property CurrentUser As AppUser

    Public ReadOnly Property IsSignedIn As Boolean
        Get
            Return CurrentUser IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property UserName As String
        Get
            If CurrentUser Is Nothing Then Return "unknown"
            Return CurrentUser.UserName
        End Get
    End Property

    Public ReadOnly Property FullName As String
        Get
            If CurrentUser Is Nothing Then Return "Not signed in"
            Return CurrentUser.FullName
        End Get
    End Property

    Public ReadOnly Property Role As String
        Get
            If CurrentUser Is Nothing Then Return String.Empty
            Return CurrentUser.Role
        End Get
    End Property

    Public Function IsInRole(ParamArray roles As String()) As Boolean
        For Each r As String In roles
            If String.Equals(r, Role, StringComparison.OrdinalIgnoreCase) Then Return True
        Next
        Return False
    End Function

End Module
