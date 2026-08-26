Option Strict On
Imports System.Data

''' <summary>
''' Monthly aggregates for the Excel reports. The grouping is done here in VB rather than
''' in SQL on purpose: Jet exposes only part of the Access function library through OLEDB,
''' so FORMAT() and friends are not dependable. Reading a year of claims and bucketing them
''' in memory costs nothing at this scale and cannot fail for that reason.
''' </summary>
Public Module ReportRepository

    ''' <summary>Statuses that represent a claim leaving the book.</summary>
    Private ReadOnly ConcludingStatuses As String() = {
        ClaimStatus.Settled, ClaimStatus.Closed, ClaimStatus.Withdrawn,
        ClaimStatus.AdjusterRejected, ClaimStatus.UnderwriterDeclined}

    ''' <summary>The first of every month across the range, so the chart has no gaps.</summary>
    Public Function MonthStarts(fromDate As Date, toDate As Date) As List(Of Date)
        Dim months As New List(Of Date)()
        Dim cursor As Date = New Date(fromDate.Year, fromDate.Month, 1)
        Dim last As Date = New Date(toDate.Year, toDate.Month, 1)

        While cursor <= last
            months.Add(cursor)
            cursor = cursor.AddMonths(1)
        End While

        Return months
    End Function

    ''' <summary>
    ''' Claim value by month of notification, restricted to the chosen statuses.
    ''' Columns are ordered so that A to D can be charted directly and the claim count
    ''' sits outside the plotted range, where it cannot flatten the money axis.
    ''' </summary>
    Public Function ClaimValueByMonth(fromDate As Date, toDate As Date,
                                      statuses As IEnumerable(Of String)) As DataTable
        Dim table As New DataTable("ClaimValue")
        table.Columns.Add("Month", GetType(String))
        table.Columns.Add("Estimated value", GetType(Decimal))
        table.Columns.Add("Reserve", GetType(Decimal))
        table.Columns.Add("Paid to date", GetType(Decimal))
        table.Columns.Add("Claims", GetType(Integer))

        Dim months As List(Of Date) = MonthStarts(fromDate, toDate)
        Dim estimated As New Dictionary(Of Date, Decimal)()
        Dim reserve As New Dictionary(Of Date, Decimal)()
        Dim paid As New Dictionary(Of Date, Decimal)()
        Dim counts As New Dictionary(Of Date, Integer)()

        For Each m As Date In months
            estimated(m) = 0D
            reserve(m) = 0D
            paid(m) = 0D
            counts(m) = 0
        Next

        Dim filter As String = StatusFilter(statuses)
        Dim sql As String =
            "SELECT [DateNotified],[EstimatedValue],[ReserveAmount],[SettledAmount] " &
            "FROM [Claims] WHERE [DateNotified] >= ? AND [DateNotified] < ?" & filter

        Dim data As DataTable = Db.GetTable(sql, fromDate.Date, toDate.Date.AddDays(1))

        For Each row As DataRow In data.Rows
            Dim bucket As Date = MonthOf(Db.AsDate(row("DateNotified")))
            If Not counts.ContainsKey(bucket) Then Continue For

            estimated(bucket) += Db.AsDecimal(row("EstimatedValue"))
            reserve(bucket) += Db.AsDecimal(row("ReserveAmount"))
            paid(bucket) += Db.AsDecimal(row("SettledAmount"))
            counts(bucket) += 1
        Next

        For Each m As Date In months
            table.Rows.Add(Label(m), estimated(m), reserve(m), paid(m), counts(m))
        Next

        Return table
    End Function

    ''' <summary>
    ''' How many claims arrived each month against how many left the book, plus the
    ''' resulting number still open at each month end.
    ''' </summary>
    Public Function ClaimFrequencyByMonth(fromDate As Date, toDate As Date) As DataTable
        Dim table As New DataTable("ClaimFrequency")
        table.Columns.Add("Month", GetType(String))
        table.Columns.Add("Claims notified", GetType(Integer))
        table.Columns.Add("Claims concluded", GetType(Integer))
        table.Columns.Add("Open at month end", GetType(Integer))

        Dim months As List(Of Date) = MonthStarts(fromDate, toDate)
        Dim notified As New Dictionary(Of Date, Integer)()
        Dim concluded As New Dictionary(Of Date, Integer)()

        For Each m As Date In months
            notified(m) = 0
            concluded(m) = 0
        Next

        Dim upperBound As Date = toDate.Date.AddDays(1)

        Dim arrivals As DataTable = Db.GetTable(
            "SELECT [DateNotified] FROM [Claims] " &
            "WHERE [DateNotified] >= ? AND [DateNotified] < ?",
            fromDate.Date, upperBound)

        For Each row As DataRow In arrivals.Rows
            Dim bucket As Date = MonthOf(Db.AsDate(row("DateNotified")))
            If notified.ContainsKey(bucket) Then notified(bucket) += 1
        Next

        Dim exits As DataTable = Db.GetTable(
            "SELECT [ChangedAt] FROM [ClaimStatusHistory] " &
            "WHERE [ChangedAt] >= ? AND [ChangedAt] < ? AND [ToStatus] IN (" &
            Quoted(ConcludingStatuses) & ")",
            fromDate.Date, upperBound)

        For Each row As DataRow In exits.Rows
            Dim bucket As Date = MonthOf(Db.AsDate(row("ChangedAt")))
            If concluded.ContainsKey(bucket) Then concluded(bucket) += 1
        Next

        ' Claims already open when the period started, so the running figure is honest
        ' rather than starting from zero.
        Dim running As Integer = OpenBefore(fromDate.Date)

        For Each m As Date In months
            running += notified(m) - concluded(m)
            If running < 0 Then running = 0
            table.Rows.Add(Label(m), notified(m), concluded(m), running)
        Next

        Return table
    End Function

    ''' <summary>Claims notified before the period that had not concluded by then.</summary>
    Private Function OpenBefore(boundary As Date) As Integer
        Dim opened As Integer = Db.AsInt(Db.Scalar(
            "SELECT COUNT(*) FROM [Claims] WHERE [DateNotified] < ?", boundary))

        Dim closed As Integer = Db.AsInt(Db.Scalar(
            "SELECT COUNT(*) FROM [ClaimStatusHistory] WHERE [ChangedAt] < ? " &
            "AND [ToStatus] IN (" & Quoted(ConcludingStatuses) & ")", boundary))

        Dim result As Integer = opened - closed
        Return If(result < 0, 0, result)
    End Function

    Private Function MonthOf(value As Date) As Date
        Return New Date(value.Year, value.Month, 1)
    End Function

    Private Function Label(monthStart As Date) As String
        Return monthStart.ToString("MMM yyyy")
    End Function

    ''' <summary>Builds the AND [Status] IN (...) clause. An empty selection means no filter.</summary>
    Private Function StatusFilter(statuses As IEnumerable(Of String)) As String
        If statuses Is Nothing Then Return String.Empty

        Dim chosen As String() = statuses.Where(
            Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray()

        If chosen.Length = 0 OrElse chosen.Length = ClaimStatus.All.Length Then
            Return String.Empty
        End If

        Return " AND [Status] IN (" & Quoted(chosen) & ")"
    End Function

    ''' <summary>These values come from the fixed ClaimStatus list, never from free text,
    ''' but the quotes are still escaped.</summary>
    Private Function Quoted(values As String()) As String
        Return String.Join(",", values.Select(Function(v) "'" & v.Replace("'", "''") & "'"))
    End Function

End Module
