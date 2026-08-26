Option Strict On
Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms

''' <summary>
''' The application icon, read once from the embedded resource and cached. Every form gets
''' it through UiStyle.ApplyTo, which is why it shows in the title bar and on the taskbar
''' as well as on the Start menu shortcut the installer creates.
''' </summary>
Public Module AppIcon

    Private _cached As Icon
    Private _attempted As Boolean

    ''' <summary>Returns Nothing if the icon cannot be loaded, in which case forms keep the
    ''' default WinForms icon rather than failing to open.</summary>
    Public ReadOnly Property Value As Icon
        Get
            If Not _attempted Then
                _attempted = True
                _cached = Load()
            End If
            Return _cached
        End Get
    End Property

    Private Function Load() As Icon
        Try
            Dim source As Assembly = Assembly.GetExecutingAssembly()

            For Each resourceName As String In source.GetManifestResourceNames()
                If Not resourceName.EndsWith("ClaimsManager.ico",
                                             StringComparison.OrdinalIgnoreCase) Then
                    Continue For
                End If

                Using stream As Stream = source.GetManifestResourceStream(resourceName)
                    If stream Is Nothing Then Exit For

                    ' Copied into memory because the icon keeps a reference to its stream.
                    Dim buffer As New MemoryStream()
                    stream.CopyTo(buffer)
                    buffer.Position = 0
                    Return New Icon(buffer)
                End Using
            Next

            ' Nothing embedded: fall back to the icon compiled into the executable itself.
            Return Icon.ExtractAssociatedIcon(Application.ExecutablePath)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Module
