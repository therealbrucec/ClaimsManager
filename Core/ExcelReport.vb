' Late binding again, as with ADOX: this keeps Excel out of the project references so the
' application still builds and runs on a machine that has no Office installed.
Option Strict Off
Imports System.Data
Imports System.IO
Imports System.Runtime.InteropServices

''' <summary>Raised when Excel cannot be started, so callers can offer CSV instead.</summary>
Public Class ExcelNotAvailableException
    Inherits ApplicationException

    Public Sub New()
        MyBase.New("Excel could not be started on this machine.")
    End Sub
End Class

''' <summary>
''' Writes a DataTable to a worksheet and draws a line chart underneath it. Excel does the
''' charting, which is both far less work than assembling chart XML by hand and how this
''' kind of report has always been produced on a broker's desk.
''' </summary>
Public Module ExcelReport

    Private Const xlLineMarkers As Integer = 65
    Private Const xlColumns As Integer = 2
    Private Const xlCategory As Integer = 1
    Private Const xlValue As Integer = 2
    Private Const xlOpenXMLWorkbook As Integer = 51
    Private Const xlExcel8 As Integer = 56

    ''' <summary>
    ''' Builds the workbook and returns the path it was actually written to, which may
    ''' differ from the one requested if only the older .xls format was available.
    ''' </summary>
    Public Function Export(table As DataTable, filePath As String, sheetName As String,
                           chartTitle As String, chartLastColumn As Integer,
                           valueAxisTitle As String, moneyFormat As Boolean) As String

        Dim excel As Object = Nothing
        Dim books As Object = Nothing
        Dim book As Object = Nothing
        Dim sheet As Object = Nothing
        Dim savedAs As String = filePath

        Try
            Try
                excel = CreateObject("Excel.Application")
            Catch
                Throw New ExcelNotAvailableException()
            End Try

            excel.Visible = False
            excel.DisplayAlerts = False
            excel.ScreenUpdating = False

            books = excel.Workbooks
            book = books.Add()
            sheet = book.Worksheets(1)
            sheet.Name = sheetName

            WriteGrid(sheet, table)

            Dim lastRow As Integer = table.Rows.Count + 1
            Dim lastColumn As Integer = table.Columns.Count

            ' Header row
            sheet.Range(sheet.Cells(1, 1), sheet.Cells(1, lastColumn)).Font.Bold = True
            sheet.Range(sheet.Cells(1, 1), sheet.Cells(1, lastColumn)).Interior.Color = RGB(230, 230, 230)

            If lastRow > 1 Then
                If moneyFormat Then
                    sheet.Range(sheet.Cells(2, 2), sheet.Cells(lastRow, chartLastColumn)).NumberFormat = "#,##0.00"
                End If
                ' Any count column sits to the right of the charted range.
                If lastColumn > chartLastColumn Then
                    sheet.Range(sheet.Cells(2, chartLastColumn + 1),
                                sheet.Cells(lastRow, lastColumn)).NumberFormat = "#,##0"
                End If
            End If

            sheet.Columns.AutoFit()

            If lastRow > 1 Then
                DrawLineChart(sheet, lastRow, chartLastColumn, chartTitle, valueAxisTitle, moneyFormat)
            End If

            sheet.Range("A1").Select()

            savedAs = SaveWorkbook(book, filePath)
            book.Close(False)
            book = Nothing

            excel.Quit()
            excel = Nothing

        Finally
            Release(sheet)
            Release(book)
            Release(books)

            If excel IsNot Nothing Then
                Try
                    excel.Quit()
                Catch
                    ' Nothing useful to do; the release below still happens.
                End Try
                Release(excel)
            End If

            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

        Return savedAs
    End Function

    Private Sub WriteGrid(sheet As Object, table As DataTable)
        For c As Integer = 0 To table.Columns.Count - 1
            sheet.Cells(1, c + 1).Value = table.Columns(c).ColumnName
        Next

        For r As Integer = 0 To table.Rows.Count - 1
            For c As Integer = 0 To table.Columns.Count - 1
                sheet.Cells(r + 2, c + 1).Value = CellValue(table.Rows(r)(c))
            Next
        Next
    End Sub

    ''' <summary>Excel is happier with a Double than a Decimal across the COM boundary.</summary>
    Private Function CellValue(value As Object) As Object
        If value Is Nothing OrElse value Is DBNull.Value Then Return String.Empty
        If TypeOf value Is Decimal Then Return CDbl(DirectCast(value, Decimal))
        Return value
    End Function

    Private Sub DrawLineChart(sheet As Object, lastRow As Integer, lastColumn As Integer,
                              chartTitle As String, valueAxisTitle As String,
                              moneyFormat As Boolean)
        Dim chartObjects As Object = Nothing
        Dim chartObject As Object = Nothing
        Dim chart As Object = Nothing

        Try
            ' Sit the chart below the figures, roughly a row height per data row.
            Dim top As Integer = (lastRow + 2) * 15

            chartObjects = sheet.ChartObjects()
            chartObject = chartObjects.Add(20, top, 760, 400)
            chart = chartObject.Chart

            chart.ChartType = xlLineMarkers
            chart.SetSourceData(sheet.Range(sheet.Cells(1, 1), sheet.Cells(lastRow, lastColumn)),
                                xlColumns)

            chart.HasTitle = True
            chart.ChartTitle.Text = chartTitle

            chart.Axes(xlCategory).HasTitle = True
            chart.Axes(xlCategory).AxisTitle.Text = "Month"
            chart.Axes(xlValue).HasTitle = True
            chart.Axes(xlValue).AxisTitle.Text = valueAxisTitle

            If moneyFormat Then
                chart.Axes(xlValue).TickLabels.NumberFormat = "#,##0"
            End If

            chart.HasLegend = True
        Catch
            ' A missing chart is disappointing but the figures are still worth having,
            ' so a charting failure does not lose the export.
        Finally
            Release(chart)
            Release(chartObject)
            Release(chartObjects)
        End Try
    End Sub

    ''' <summary>Tries the modern format first, falling back to .xls on older Excel.</summary>
    Private Function SaveWorkbook(book As Object, filePath As String) As String
        Try
            book.SaveAs(filePath, xlOpenXMLWorkbook)
            Return filePath
        Catch
            Dim legacy As String = Path.ChangeExtension(filePath, ".xls")
            book.SaveAs(legacy, xlExcel8)
            Return legacy
        End Try
    End Function

    Private Sub Release(ByRef comObject As Object)
        If comObject Is Nothing Then Return

        Try
            Marshal.FinalReleaseComObject(comObject)
        Catch
            ' Already released, or never a COM object.
        End Try

        comObject = Nothing
    End Sub

    ''' <summary>Plain figures, for when Excel is not installed. No chart, obviously.</summary>
    Public Sub ExportCsv(table As DataTable, filePath As String)
        Using writer As New StreamWriter(filePath, False, Text.Encoding.UTF8)
            Dim headers As New List(Of String)()
            For Each column As DataColumn In table.Columns
                headers.Add(CsvField(column.ColumnName))
            Next
            writer.WriteLine(String.Join(",", headers.ToArray()))

            For Each row As DataRow In table.Rows
                Dim cells As New List(Of String)()
                For c As Integer = 0 To table.Columns.Count - 1
                    cells.Add(CsvField(Convert.ToString(row(c))))
                Next
                writer.WriteLine(String.Join(",", cells.ToArray()))
            Next
        End Using
    End Sub

    Private Function CsvField(value As String) As String
        Dim text As String = If(value, String.Empty)
        If text.IndexOf(""""c) >= 0 Then text = text.Replace("""", """""")
        If text.IndexOfAny(New Char() {","c, """"c, ControlChars.Cr, ControlChars.Lf}) >= 0 Then
            Return """" & text & """"
        End If
        Return text
    End Function

End Module
