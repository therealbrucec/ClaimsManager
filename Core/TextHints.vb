Option Strict On
Imports System.Globalization
Imports System.Text.RegularExpressions

''' <summary>
''' Everything arrives as prose. This scrapes the obvious identifiers out of a message
''' body so the match screen can pre-fill its search instead of making the handler
''' retype what is already on the page. It is a hint, never a decision.
''' </summary>
Public Class TextHints

    Public Property ClaimRefs As New List(Of String)
    Public Property PolicyNumbers As New List(Of String)
    Public Property Dates As New List(Of Date)

    Private Shared ReadOnly ClaimRefPattern As New Regex(
        "\bCLM[-/]\d{4}[-/]\d{3,6}\b", RegexOptions.IgnoreCase Or RegexOptions.Compiled)

    ' Two to five letters, an optional slash or dash, then five or more digits.
    ' Catches NGC/884213, EM/2211907, CR-551002 and NGC884213.
    Private Shared ReadOnly PolicyPattern As New Regex(
        "\b[A-Z]{2,5}[-/ ]?\d{5,10}\b", RegexOptions.IgnoreCase Or RegexOptions.Compiled)

    Private Shared ReadOnly DatePattern As New Regex(
        "\b(\d{1,2})[/.\-](\d{1,2})[/.\-](\d{2,4})\b", RegexOptions.Compiled)

    Private Shared ReadOnly DateFormats As String() = {
        "d/M/yyyy", "dd/MM/yyyy", "d/M/yy", "dd/MM/yy",
        "d-M-yyyy", "dd-MM-yyyy", "d.M.yyyy", "dd.MM.yyyy"
    }

    Public Shared Function Extract(ParamArray textBlocks As String()) As TextHints
        Dim hints As New TextHints()
        Dim combined As String = String.Join(" " & vbCrLf, textBlocks.Where(
            Function(t) Not String.IsNullOrWhiteSpace(t)))
        If combined.Length = 0 Then Return hints

        For Each m As Match In ClaimRefPattern.Matches(combined)
            Dim value As String = m.Value.ToUpperInvariant()
            If Not hints.ClaimRefs.Contains(value) Then hints.ClaimRefs.Add(value)
        Next

        For Each m As Match In PolicyPattern.Matches(combined)
            Dim value As String = m.Value.Trim().ToUpperInvariant()
            ' A claim reference is not a policy number.
            If ClaimRefPattern.IsMatch(value) Then Continue For
            If Not hints.PolicyNumbers.Contains(value) Then hints.PolicyNumbers.Add(value)
        Next

        Dim gb As CultureInfo = CultureInfo.GetCultureInfo("en-GB")
        For Each m As Match In DatePattern.Matches(combined)
            Dim parsed As Date
            If Date.TryParseExact(m.Value, DateFormats, gb,
                                  DateTimeStyles.None, parsed) Then
                ' Reject anything implausible for a date of loss.
                If parsed.Year >= 1990 AndAlso parsed <= Date.Today.AddDays(1) Then
                    If Not hints.Dates.Contains(parsed) Then hints.Dates.Add(parsed)
                End If
            End If
        Next

        Return hints
    End Function

    Public ReadOnly Property FirstPolicyNumber As String
        Get
            If PolicyNumbers.Count = 0 Then Return String.Empty
            Return PolicyNumbers(0)
        End Get
    End Property

    Public ReadOnly Property EarliestDate As Date?
        Get
            If Dates.Count = 0 Then Return Nothing
            Return Dates.Min()
        End Get
    End Property

    ''' <summary>Best guess at a searchable surname from a sender name.</summary>
    Public Shared Function SurnameFrom(fromName As String) As String
        If String.IsNullOrWhiteSpace(fromName) Then Return String.Empty
        Dim parts As String() = fromName.Trim().Split(
            New Char() {" "c, ","c}, StringSplitOptions.RemoveEmptyEntries)
        If parts.Length = 0 Then Return String.Empty
        Return parts(parts.Length - 1)
    End Function

End Class
