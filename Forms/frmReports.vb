Option Strict On
Imports System.Data
Imports System.IO
Imports System.Windows.Forms

Public Class frmReports

    Public Sub New()
        InitializeComponent()
        UiStyle.ApplyTo(Me)
        AppHelp.Attach(Me, AppHelp.Reports)
    End Sub

    Private Sub frmReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each status As String In ClaimStatus.All
            clbStatuses.Items.Add(status, True)
        Next

        ResetToLastTwelveMonths()
    End Sub

    ''' <summary>A full year back, matching the span the demo data is generated across, so
    ''' nothing in the database sits outside the default period.</summary>
    Private Sub ResetToLastTwelveMonths()
        Dim today As Date = Date.Today
        dtpFrom.Value = today.AddYears(-1)
        dtpTo.Value = today
    End Sub

#Region "Selection"

    Private Sub btnLast12_Click(sender As Object, e As EventArgs) Handles btnLast12.Click
        ResetToLastTwelveMonths()
    End Sub

    Private Sub btnAllStatuses_Click(sender As Object, e As EventArgs) Handles btnAllStatuses.Click
        SetAllChecked(True)
    End Sub

    Private Sub btnNoStatuses_Click(sender As Object, e As EventArgs) Handles btnNoStatuses.Click
        SetAllChecked(False)
    End Sub

    Private Sub btnOpenOnly_Click(sender As Object, e As EventArgs) Handles btnOpenOnly.Click
        For i As Integer = 0 To clbStatuses.Items.Count - 1
            Dim status As String = Convert.ToString(clbStatuses.Items(i))
            clbStatuses.SetItemChecked(i, ClaimStatus.Open.Contains(status))
        Next
    End Sub

    Private Sub SetAllChecked(checked As Boolean)
        For i As Integer = 0 To clbStatuses.Items.Count - 1
            clbStatuses.SetItemChecked(i, checked)
        Next
    End Sub

    Private Function SelectedStatuses() As List(Of String)
        Dim chosen As New List(Of String)()
        For Each item As Object In clbStatuses.CheckedItems
            chosen.Add(Convert.ToString(item))
        Next
        Return chosen
    End Function

    Private Function RangeIsSensible() As Boolean
        If dtpFrom.Value.Date > dtpTo.Value.Date Then
            UiStyle.ShowProblem(Me, "The start of the period is after the end of it. " &
                                    "Swap the two dates and try again.")
            Return False
        End If
        Return True
    End Function

#End Region

#Region "Exports"

    Private Sub btnValueReport_Click(sender As Object, e As EventArgs) Handles btnValueReport.Click
        If Not RangeIsSensible() Then Return

        Dim statuses As List(Of String) = SelectedStatuses()
        If statuses.Count = 0 Then
            UiStyle.ShowProblem(Me, "Tick at least one claim status to report on.")
            Return
        End If

        Dim table As DataTable
        Try
            table = ReportRepository.ClaimValueByMonth(dtpFrom.Value, dtpTo.Value, statuses)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
            Return
        End Try

        Dim scope As String = If(statuses.Count = ClaimStatus.All.Length,
                                 "all statuses",
                                 statuses.Count.ToString() & " of " &
                                 ClaimStatus.All.Length.ToString() & " statuses")

        Deliver(table,
                "Claim value by month",
                "Claim value",
                "Claim value by month of notification (" & scope & ")",
                4, "Value (GBP)", True)
    End Sub

    Private Sub btnFrequencyReport_Click(sender As Object, e As EventArgs) _
        Handles btnFrequencyReport.Click
        If Not RangeIsSensible() Then Return

        Dim table As DataTable
        Try
            table = ReportRepository.ClaimFrequencyByMonth(dtpFrom.Value, dtpTo.Value)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
            Return
        End Try

        Deliver(table,
                "Claim frequency by month",
                "Claim frequency",
                "Claim frequency by month",
                4, "Number of claims", False)
    End Sub

    ''' <summary>
    ''' Asks where to put it, writes it, then offers to open it. Falls back to CSV when
    ''' Excel is not installed rather than leaving the user with nothing.
    ''' </summary>
    Private Sub Deliver(table As DataTable, reportName As String, sheetName As String,
                        chartTitle As String, chartLastColumn As Integer,
                        valueAxisTitle As String, moneyFormat As Boolean)

        If table.Rows.Count = 0 Then
            UiStyle.ShowProblem(Me, "There is nothing to report for that period.")
            Return
        End If

        Dim suggested As String = reportName & " " &
                                  dtpFrom.Value.ToString("yyyy-MM") & " to " &
                                  dtpTo.Value.ToString("yyyy-MM") & ".xlsx"

        Dim target As String
        Using picker As New SaveFileDialog()
            picker.Title = reportName
            picker.FileName = suggested
            picker.Filter = "Excel workbook (*.xlsx)|*.xlsx|CSV file (*.csv)|*.csv"
            picker.DefaultExt = "xlsx"
            picker.OverwritePrompt = True
            picker.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments)

            If picker.ShowDialog(Me) <> DialogResult.OK Then Return
            target = picker.FileName
        End Using

        ' Chose CSV explicitly: no Excel involved, so no chart either.
        If Path.GetExtension(target).ToLowerInvariant() = ".csv" Then
            Try
                ExcelReport.ExportCsv(table, target)
                Report(target, False)
            Catch ex As Exception
                UiStyle.ShowError(Me, ex)
            End Try
            Return
        End If

        Dim savedAs As String = String.Empty
        Dim previousCursor As Cursor = Cursor.Current
        Cursor.Current = Cursors.WaitCursor
        Try
            savedAs = ExcelReport.Export(table, target, sheetName, chartTitle,
                                         chartLastColumn, valueAxisTitle, moneyFormat)
        Catch ex As ExcelNotAvailableException
            Cursor.Current = previousCursor
            OfferCsv(table, target)
            Return
        Catch ex As Exception
            Cursor.Current = previousCursor
            UiStyle.ShowError(Me, ex)
            Return
        Finally
            Cursor.Current = previousCursor
        End Try

        Report(savedAs, True)
    End Sub

    Private Sub OfferCsv(table As DataTable, requestedPath As String)
        Dim csvPath As String = Path.ChangeExtension(requestedPath, ".csv")

        If MessageBox.Show(Me,
            "Excel could not be started, so the chart cannot be drawn." & vbCrLf & vbCrLf &
            "Save the figures as a CSV file instead?" & vbCrLf & csvPath,
            "Reports", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            lblResult.Text = "Nothing was written."
            Return
        End If

        Try
            ExcelReport.ExportCsv(table, csvPath)
            Report(csvPath, False)
        Catch ex As Exception
            UiStyle.ShowError(Me, ex)
        End Try
    End Sub

    Private Sub Report(savedPath As String, withChart As Boolean)
        lblResult.Text = "Written to " & savedPath &
                         If(withChart, " with a line chart.", " as plain figures.")

        If MessageBox.Show(Me, "Report saved. Open it now?", "Reports",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            Process.Start(New ProcessStartInfo(savedPath) With {.UseShellExecute = True})
        Catch ex As Exception
            UiStyle.ShowProblem(Me, "The file was written but could not be opened: " & ex.Message)
        End Try
    End Sub

#End Region

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

End Class
