Option Strict On
Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' Shown while the database is opened or, on a first run, created and populated. The work
''' happens on this thread, so ReportProgress repaints and pumps messages itself rather than
''' leaving a frozen white rectangle on screen.
''' </summary>
Public Class frmSplash

    Private ReadOnly _shownAt As Date = Date.Now

    ''' <summary>On a second run there is almost nothing to do, and a splash that appears
    ''' for 40 milliseconds reads as a glitch rather than a start-up.</summary>
    Private Const MinimumVisibleMs As Integer = 900

    Public Sub New()
        InitializeComponent()

        Font = UiStyle.BodyFont
        lblProduct.ForeColor = UiStyle.BrandNavy
        LoadLogo()
    End Sub

    ''' <summary>Passed to DbInitializer so start-up can say what it is doing.</summary>
    Public Sub ReportProgress(message As String, percent As Integer)
        If Not String.IsNullOrEmpty(message) Then lblStatus.Text = message

        If percent >= 0 Then
            Dim clamped As Integer = Math.Max(0, Math.Min(100, percent))
            prgLoading.Value = clamped
        End If

        lblStatus.Refresh()
        prgLoading.Refresh()
        Application.DoEvents()
    End Sub

    ''' <summary>Closes, but not before the splash has been on screen long enough to read.</summary>
    Public Sub CloseWhenReady()
        Dim elapsed As Double = (Date.Now - _shownAt).TotalMilliseconds

        If elapsed < MinimumVisibleMs Then
            System.Threading.Thread.Sleep(CInt(MinimumVisibleMs - elapsed))
        End If

        Close()
    End Sub

    ''' <summary>
    ''' The logo is compiled into the assembly, so there is no loose image file to lose.
    ''' The resource is found by suffix rather than by its full manifest name, which depends
    ''' on the root namespace and folder and is easy to get subtly wrong.
    ''' </summary>
    Private Sub LoadLogo()
        Try
            Dim assembly As System.Reflection.Assembly =
                System.Reflection.Assembly.GetExecutingAssembly()

            For Each resourceName As String In assembly.GetManifestResourceNames()
                If Not resourceName.EndsWith("NorthbridgeAssurance.png",
                                             StringComparison.OrdinalIgnoreCase) Then
                    Continue For
                End If

                Using source As Stream = assembly.GetManifestResourceStream(resourceName)
                    If source Is Nothing Then Exit For

                    ' Copied into memory because an Image holds on to its stream.
                    Dim buffer As New MemoryStream()
                    source.CopyTo(buffer)
                    buffer.Position = 0
                    pbLogo.Image = Drawing.Image.FromStream(buffer)
                End Using

                Return
            Next
        Catch ex As Exception
            ' Fall through to the text-only treatment below.
        End Try

        pbLogo.Visible = False
        lblProduct.Text = "Northbridge Assurance - Claims Manager"
    End Sub

    ''' <summary>A borderless form with a white face needs an edge, or it floats.</summary>
    Private Sub frmSplash_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        Using edge As New Drawing.Pen(UiStyle.BrandNavy)
            e.Graphics.DrawRectangle(edge, 0, 0, Width - 1, Height - 1)
        End Using
    End Sub

End Class
