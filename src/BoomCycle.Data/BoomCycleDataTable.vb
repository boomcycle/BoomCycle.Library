Imports System.Data.SqlClient

Public Class BoomCycleDataTable
    Inherits BoomCycleDataTableBase
    Implements IEnumerable(Of BoomCycleDataRow)

    ' BTO 8/16/2012
    Public Sub New()
        MyBase.New(New DataTable)
    End Sub

    ' BTO 8/16/2012
    Private ReadOnly Property WrappedDataTable() As DataTable
        Get
            Return DirectCast(WrappedObject(), DataTable)
        End Get
    End Property

    ' BTO 8/16/2012 & earlier
    Public Overrides Function RecordCount() As Integer
        Return WrappedDataTable().Rows.Count()
    End Function

    ' BTO 8/16/2012
    Public Overrides Sub AddColumn(ByVal colName As String)
        WrappedDataTable().Columns.Add(colName)
    End Sub

    ' BTO 8/16/2012
    Public Overrides Function NewRow() As BoomCycleDataRow
        Return New BoomCycleDataRow(WrappedDataTable().NewRow())
    End Function

    ' BTO 8/16/2012
    Public Overrides Sub AddRow(ByVal row As BoomCycleDataRow)
        WrappedDataTable().Rows.Add(row)
        AFFDataRowsList.Add(row)
    End Sub


    Public Overrides Function GetColumns() As List(Of String)
        Dim listofCols As List(Of String) = New List(Of String)()
        For Each col As DataColumn In Me.WrappedDataTable.Columns
            listofCols.Add(col.ColumnName)
        Next
        Return listofCols
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="conn"></param>
    ''' <param name="sql"></param>
    ''' <param name="sqlParams"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DoCommand(ByVal conn As String, ByVal sql As String, ByVal sqlParams As System.Collections.Generic.List(Of BoomCycleSqlParams)) As Integer
        Try
            Using sqlConn As SqlConnection = New SqlConnection(conn)
                Using cmd As New SqlCommand(sql, sqlConn)
                    If Not sql.Contains(" ") Then 'if no space, assume stored procedure
                        cmd.CommandType = CommandType.StoredProcedure
                    End If
                    If sqlParams IsNot Nothing Then
                        SetSqlParams(cmd, sqlParams)
                    End If
                    cmd.Connection.Open()
                    Return cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As SqlException
            Throw New BoomCycleDataException(ex.Message & " (DoCommand)", ex)
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="conn"></param>
    ''' <param name="sql"></param>
    ''' <param name="sqlParams"></param>
    ''' <remarks></remarks>
    Public Overrides Sub DoQuery(ByVal conn As String, ByVal sql As String, ByVal sqlParams As System.Collections.Generic.List(Of BoomCycleSqlParams))

        Dim dt As DataTable = New DataTable

        Try
            Using sqlConn As SqlConnection = New SqlConnection(conn)
                Using adapter As New SqlDataAdapter()
                    Using cmd As SqlCommand = New SqlCommand(sql, sqlConn)
                        If Not sql.Contains(" ") Then 'if no space, assume stored procedure
                            cmd.CommandType = CommandType.StoredProcedure
                        End If
                        If sqlParams IsNot Nothing Then
                            SetSqlParams(cmd, sqlParams)
                        End If
                        adapter.SelectCommand() = cmd
                        adapter.Fill(dt)
                        WrappedObject = dt
                    End Using
                End Using
            End Using
        Catch ex As SqlException
            Throw New BoomCycleDataException(ex.Message & " (DoQuery)", ex)
        End Try
    End Sub

    Protected Overrides Sub PopulateListOfAFF()
        For Each drCur As DataRow In WrappedObject.Rows
            AFFDataRowsList.Add(New BoomCycleDataRow(drCur))
        Next
    End Sub

    ''' <summary>
    ''' itterator for AFFdatarows wrapper
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of BoomCycleDataRow) Implements System.Collections.Generic.IEnumerable(Of BoomCycleDataRow).GetEnumerator
        Return AFFDataRowsList.GetEnumerator()
    End Function

    Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return Me.GetEnumerator()
    End Function

    ''' <summary>
    ''' set all params
    ''' </summary>
    ''' <param name="cmd"></param>
    ''' <param name="sqlParams"></param>
    ''' <remarks></remarks>
    Protected Sub SetSqlParams(ByVal cmd As SqlCommand, ByVal sqlParams As List(Of BoomCycleSqlParams))
        For Each sqlParamsCur As BoomCycleSqlParams In sqlParams
            'Dim sqlParm = _
            cmd.Parameters.AddWithValue(sqlParamsCur.Name, sqlParamsCur.Value)
        Next
    End Sub

    ' TODO: Expose more of the underlying DataTable _dt in this way:
    ' define a corresponding method or property, and have it delegate to _dt.
    ' ....

End Class