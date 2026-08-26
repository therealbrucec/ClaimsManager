Option Strict On
Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' Keeps the period look in one place: Tahoma 8.25pt, sunken borders, grey control
''' faces. Applied from each form's constructor rather than repeated in every designer.
''' </summary>
Public Module UiStyle

    ''' <summary>Sampled from the Northbridge Assurance mark, so the chrome agrees with it.</summary>
    Public ReadOnly Property BrandNavy As Color
        Get
            Return Color.FromArgb(16, 46, 88)
        End Get
    End Property

    Public ReadOnly Property BrandBlue As Color
        Get
            Return Color.FromArgb(0, 108, 180)
        End Get
    End Property

    Public ReadOnly Property BodyFont As Font
        Get
            Return New Font("Tahoma", 8.25!, FontStyle.Regular, GraphicsUnit.Point)
        End Get
    End Property

    Public ReadOnly Property HeadingFont As Font
        Get
            Return New Font("Tahoma", 8.25!, FontStyle.Bold, GraphicsUnit.Point)
        End Get
    End Property

    Public ReadOnly Property MonoFont As Font
        Get
            Return New Font("Courier New", 8.25!, FontStyle.Regular, GraphicsUnit.Point)
        End Get
    End Property

    Public Sub ApplyTo(target As Form)
        target.Font = BodyFont
        target.BackColor = SystemColors.Control

        ' Puts the Northbridge mark in the title bar and on the taskbar.
        If AppIcon.Value IsNot Nothing Then target.Icon = AppIcon.Value
    End Sub

    ''' <summary>Read-only, full-row selection, no row headers. How every list in a
    ''' claims system of this era behaved.</summary>
    Public Sub StyleGrid(grid As DataGridView)
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.AllowUserToResizeRows = False
        grid.RowHeadersVisible = False
        grid.MultiSelect = False
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.BackgroundColor = Color.White
        grid.BorderStyle = BorderStyle.Fixed3D
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control
        grid.ColumnHeadersDefaultCellStyle.Font = HeadingFont
        grid.DefaultCellStyle.Font = BodyFont
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245)
    End Sub

    ''' <summary>Hides a column that is needed for the row's identity but not for reading.</summary>
    Public Sub HideColumn(grid As DataGridView, columnName As String)
        If grid.Columns.Contains(columnName) Then
            grid.Columns(columnName).Visible = False
        End If
    End Sub

    Public Sub FormatCurrencyColumn(grid As DataGridView, columnName As String)
        If Not grid.Columns.Contains(columnName) Then Return
        grid.Columns(columnName).DefaultCellStyle.Format = "N2"
        grid.Columns(columnName).DefaultCellStyle.Alignment =
            DataGridViewContentAlignment.MiddleRight
    End Sub

    Public Sub FormatDateColumn(grid As DataGridView, columnName As String,
                                Optional includeTime As Boolean = False)
        If Not grid.Columns.Contains(columnName) Then Return
        grid.Columns(columnName).DefaultCellStyle.Format =
            If(includeTime, "dd/MM/yyyy HH:mm", "dd/MM/yyyy")
    End Sub

    ''' <summary>Colour cue for the status column, so a queue can be read at a glance.</summary>
    Public Function ColourFor(status As String) As Color
        Select Case status
            Case ClaimStatus.WithAdjuster, ClaimStatus.SentToUnderwriter
                Return Color.FromArgb(0, 0, 128)
            Case ClaimStatus.AdjusterRejected, ClaimStatus.UnderwriterDeclined,
                 ClaimStatus.Withdrawn
                Return Color.FromArgb(128, 0, 0)
            Case ClaimStatus.AdjusterApproved, ClaimStatus.UnderwriterAgreed,
                 ClaimStatus.Settled
                Return Color.FromArgb(0, 90, 0)
            Case ClaimStatus.Closed
                Return Color.FromArgb(90, 90, 90)
            Case Else
                Return SystemColors.ControlText
        End Select
    End Function

    ''' <summary>Consistent error reporting: says what happened and what to do next.</summary>
    Public Sub ShowProblem(owner As IWin32Window, message As String)
        MessageBox.Show(owner, message, "Claims Manager",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Public Sub ShowError(owner As IWin32Window, ex As Exception)
        MessageBox.Show(owner, ex.Message, "Claims Manager",
                        MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

End Module
