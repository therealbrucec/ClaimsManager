Option Strict On
Imports System.Data
Imports System.Data.OleDb

''' <summary>
''' Thin wrapper over OleDb/Jet. Jet uses positional parameters, so every "?" in a
''' statement is filled from the values array in order.
''' </summary>
Public Module Db

    Public Function OpenConnection() As OleDbConnection
        Dim cn As New OleDbConnection(Config.ConnectionString)
        Try
            cn.Open()
        Catch ex As InvalidOperationException
            cn.Dispose()
            Throw New ApplicationException(ProviderMissingMessage(), ex)
        Catch ex As OleDbException
            cn.Dispose()
            If ex.Message.IndexOf("not registered", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Throw New ApplicationException(ProviderMissingMessage(), ex)
            End If
            Throw
        End Try
        Return cn
    End Function

    Private Function ProviderMissingMessage() As String
        Return "The Jet 4.0 database provider is not available to this process." & vbCrLf & vbCrLf &
               "Jet is 32-bit only. Build the project for x86 (or tick Prefer 32-bit) and run again."
    End Function

    ''' <summary>
    ''' A null that still knows what it is. Jet raises "data type mismatch" when a null
    ''' parameter arrives with no type attached, so null dates and numbers are passed
    ''' as one of these rather than as Nothing.
    ''' </summary>
    Public Structure TypedNull
        Public ReadOnly ColumnType As OleDbType

        Public Sub New(columnType As OleDbType)
            Me.ColumnType = columnType
        End Sub
    End Structure

    Public Function NullDate() As Object
        Return New TypedNull(OleDbType.Date)
    End Function

    Public Function NullInteger() As Object
        Return New TypedNull(OleDbType.Integer)
    End Function

    Public Function NullCurrency() As Object
        Return New TypedNull(OleDbType.Currency)
    End Function

    Public Function NullText() As Object
        Return New TypedNull(OleDbType.VarWChar)
    End Function

    ''' <summary>
    ''' Binds positional parameters with an explicit OleDbType for each one.
    ''' Inference is not good enough here: OleDb maps a .NET Decimal to DBTYPE_DECIMAL,
    ''' which Jet refuses to coerce into a CURRENCY column, and reports the refusal as
    ''' "Data type mismatch in criteria expression" even on an INSERT.
    ''' </summary>
    Private Sub Bind(cmd As OleDbCommand, values As Object())
        If values Is Nothing Then Return

        For i As Integer = 0 To values.Length - 1
            Dim name As String = "p" & i.ToString()
            Dim v As Object = values(i)

            If TypeOf v Is TypedNull Then
                cmd.Parameters.Add(name, DirectCast(v, TypedNull).ColumnType).Value = DBNull.Value

            ElseIf v Is Nothing OrElse v Is DBNull.Value Then
                cmd.Parameters.Add(name, OleDbType.VarWChar).Value = DBNull.Value

            ElseIf TypeOf v Is Decimal Then
                ' Every money column in this schema is CURRENCY.
                cmd.Parameters.Add(name, OleDbType.Currency).Value = v

            ElseIf TypeOf v Is Date Then
                ' Jet stores DATETIME as a double, which is DBTYPE_DATE, not DBTimeStamp.
                cmd.Parameters.Add(name, OleDbType.Date).Value = v

            ElseIf TypeOf v Is Boolean Then
                cmd.Parameters.Add(name, OleDbType.Boolean).Value = v

            ElseIf TypeOf v Is Integer Then
                cmd.Parameters.Add(name, OleDbType.Integer).Value = v

            ElseIf TypeOf v Is Long Then
                ' Jet's LONG is 32-bit, so an Int64 cannot go straight in. Only file sizes
                ' arrive here, and 2 GB is far beyond any scanned document.
                Dim big As Long = DirectCast(v, Long)
                If big > Integer.MaxValue Then big = Integer.MaxValue
                cmd.Parameters.Add(name, OleDbType.Integer).Value = CInt(big)

            ElseIf TypeOf v Is String Then
                ' Anything over 255 characters belongs to a MEMO column, which is
                ' LongVarWChar. Binding it as VarWChar risks truncation.
                Dim text As String = DirectCast(v, String)
                If text.Length > 255 Then
                    cmd.Parameters.Add(name, OleDbType.LongVarWChar).Value = text
                Else
                    cmd.Parameters.Add(name, OleDbType.VarWChar).Value = text
                End If

            Else
                cmd.Parameters.AddWithValue(name, v)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Jet's error messages name neither the statement nor the parameter that upset it
    ''' ("Data type mismatch in criteria expression" being the usual example), so every
    ''' failure is re-thrown with the SQL and the bound parameters attached.
    ''' </summary>
    Private Function Describe(sql As String, values As Object(), ex As Exception) As String
        Dim report As New System.Text.StringBuilder()
        report.AppendLine("The database rejected this statement:")
        report.AppendLine()
        report.AppendLine(sql)
        report.AppendLine()

        If values Is Nothing OrElse values.Length = 0 Then
            report.AppendLine("Parameters: none")
        Else
            report.AppendLine("Parameters:")
            For i As Integer = 0 To values.Length - 1
                Dim v As Object = values(i)
                Dim shown As String
                If TypeOf v Is TypedNull Then
                    shown = "null (" & DirectCast(v, TypedNull).ColumnType.ToString() & ")"
                ElseIf v Is Nothing OrElse v Is DBNull.Value Then
                    shown = "null (untyped - suspect this one)"
                Else
                    shown = v.GetType().Name & " = " & Convert.ToString(v)
                End If
                report.AppendLine("  p" & i.ToString() & "  " & shown)
            Next
        End If

        report.AppendLine()
        report.AppendLine("Jet reported: " & ex.Message)
        Return report.ToString()
    End Function

    Public Function GetTable(sql As String, ParamArray values As Object()) As DataTable
        Dim dt As New DataTable()
        Try
            Using cn As OleDbConnection = OpenConnection()
                Using cmd As New OleDbCommand(sql, cn)
                    Bind(cmd, values)
                    Using da As New OleDbDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As OleDbException
            Throw New ApplicationException(Describe(sql, values, ex), ex)
        Catch ex As InvalidOperationException
            Throw New ApplicationException(Describe(sql, values, ex), ex)
        End Try
        Return dt
    End Function

    Public Function Execute(sql As String, ParamArray values As Object()) As Integer
        Try
            Using cn As OleDbConnection = OpenConnection()
                Using cmd As New OleDbCommand(sql, cn)
                    Bind(cmd, values)
                    Return cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As OleDbException
            Throw New ApplicationException(Describe(sql, values, ex), ex)
        End Try
    End Function

    Public Function Scalar(sql As String, ParamArray values As Object()) As Object
        Try
            Using cn As OleDbConnection = OpenConnection()
                Using cmd As New OleDbCommand(sql, cn)
                    Bind(cmd, values)
                    Return cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As OleDbException
            Throw New ApplicationException(Describe(sql, values, ex), ex)
        End Try
    End Function

    ''' <summary>Runs an INSERT and returns the new autonumber key. @@IDENTITY must be
    ''' read on the same connection, hence the single Using block.</summary>
    Public Function InsertAndGetId(sql As String, ParamArray values As Object()) As Integer
        Try
            Using cn As OleDbConnection = OpenConnection()
                Using cmd As New OleDbCommand(sql, cn)
                    Bind(cmd, values)
                    cmd.ExecuteNonQuery()
                End Using
                Using idCmd As New OleDbCommand("SELECT @@IDENTITY", cn)
                    Return Convert.ToInt32(idCmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As OleDbException
            Throw New ApplicationException(Describe(sql, values, ex), ex)
        End Try
    End Function

#Region "Null-safe readers"

    Public Function AsString(o As Object) As String
        If o Is Nothing OrElse o Is DBNull.Value Then Return String.Empty
        Return Convert.ToString(o)
    End Function

    Public Function AsInt(o As Object) As Integer
        If o Is Nothing OrElse o Is DBNull.Value Then Return 0
        Return Convert.ToInt32(o)
    End Function

    Public Function AsNullableInt(o As Object) As Integer?
        If o Is Nothing OrElse o Is DBNull.Value Then Return Nothing
        Return Convert.ToInt32(o)
    End Function

    Public Function AsLong(o As Object) As Long
        If o Is Nothing OrElse o Is DBNull.Value Then Return 0L
        Return Convert.ToInt64(o)
    End Function

    Public Function AsDecimal(o As Object) As Decimal
        If o Is Nothing OrElse o Is DBNull.Value Then Return 0D
        Return Convert.ToDecimal(o)
    End Function

    Public Function AsDate(o As Object) As Date
        If o Is Nothing OrElse o Is DBNull.Value Then Return Date.MinValue
        Return Convert.ToDateTime(o)
    End Function

    Public Function AsNullableDate(o As Object) As Date?
        If o Is Nothing OrElse o Is DBNull.Value Then Return Nothing
        Return Convert.ToDateTime(o)
    End Function

    Public Function AsBool(o As Object) As Boolean
        If o Is Nothing OrElse o Is DBNull.Value Then Return False
        Return Convert.ToBoolean(o)
    End Function

#End Region

End Module
