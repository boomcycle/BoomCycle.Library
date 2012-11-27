Imports System.Configuration
Imports System.Data.SqlClient

Public Class BoomCycleData
    Protected _ConnString As String
    Protected _AFFDataType As BoomCycleDataType = BoomCycleDataType.DataTable

    Sub New()
        If (ConfigurationManager.ConnectionStrings.Count > 0) Then
            If (ConfigurationManager.ConnectionStrings.Count > 1) Then
                Init(ConfigurationManager.ConnectionStrings(1).ConnectionString)
            Else
                Init(ConfigurationManager.ConnectionStrings(0).ConnectionString)
            End If
        Else
            Throw New BoomCycleDataException("No Connection String Set In Config File")
        End If
    End Sub

    Sub New(ByVal databaseName As BoomCycleDatabaseEnum)
        Dim connstringKey As String = [Enum].GetName(databaseName.GetType(), databaseName)
        Me.Init(ConfigurationManager.ConnectionStrings(connstringKey).ConnectionString)
    End Sub

    Sub New(ByVal connectionKey As String)
        Init(ConfigurationManager.ConnectionStrings(connectionKey).ConnectionString)
    End Sub

    Sub New(ByVal connstring As String, useConnString As Boolean)
        Init(connstring)
    End Sub

    Protected Sub Init(connstring As String)
        _ConnString = connstring
    End Sub


    ''' <summary>
    '''Use variable # of args Do query can have optional args of the following form(s):
    '''DoQuery( sql { {, paramName, paramValue}* {, SortFld}} )
    '''That is, 0 or more pairs paramName, paramValue, 
    '''followed possibly by a final parameter giving the name of the field to sort by.
    '''
    '''The R of CRUD -- Retrieve.
    ''' </summary>
    ''' <param name="sql"></param>
    ''' <param name="sqlParams"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DoQuery(ByVal sql As String, Optional ByVal sqlParams As List(Of BoomCycleSqlParams) = Nothing) As BoomCycleDataTableBase
        If Me._AFFDataType = BoomCycleDataType.DataTable Then
            Dim aFFDataTable As BoomCycleDataTable = New BoomCycleDataTable()
            aFFDataTable.DoQuery(_ConnString, sql, sqlParams)
            Return aFFDataTable
        End If
        Return Nothing
    End Function

    Public Function DoCommand(ByVal sql As String, Optional ByVal sqlParams As List(Of BoomCycleSqlParams) = Nothing) As Integer
        If Me._AFFDataType = BoomCycleDataType.DataTable Then
            Dim aFFDataTable As BoomCycleDataTable = New BoomCycleDataTable()
            Return aFFDataTable.DoCommand(_ConnString, sql, sqlParams)
        End If
        Return -1
    End Function

    ''' DoQuery, varargs version.
    ''' DoQuery( sql, paramName, paramValue {, paramName, paramValue}* )
    ''' That is, 1 or more pairs paramName, paramValue, ... .
    ''' where each paramName is a string (beginning with "@")
    ''' Returns AFFDataTableBase

    Public Function DoQuery(ByVal sql As String, _
                            ByVal param0 As String, ByVal value0 As Object, _
                            ByVal ParamArray paramOrValue() As Object) As BoomCycleDataTableBase

        Dim sqlParams As List(Of BoomCycleSqlParams) = GetSqlParams(param0, value0, paramOrValue)
        Return DoQuery(sql, sqlParams)

    End Function

    ''' DoCommand, varargs version.
    ''' DoCommand( sql, paramName0, paramValue0 {, paramName, paramValue}* )
    ''' That is, 1 or more pairs paramName, paramValue, ... .
    ''' where each paramName is a string (beginning with "@")
    ''' Returns number of rows AFFected

    Public Function DoCommand(ByVal sql As String, _
                            ByVal param0 As String, ByVal value0 As Object, _
                            ByVal ParamArray paramOrValue() As Object) As Integer

        Dim sqlParams As List(Of BoomCycleSqlParams) = GetSqlParams(param0, value0, paramOrValue)
        Return DoCommand(sql, sqlParams)

    End Function

    ' lil helper function to convert array of (alternating) parameterNames and values
    ' to List of AFFSqlParams

    Private Function GetSqlParams(ByVal param0 As String, ByVal value0 As Object, _
                                  ByVal ParamArray paramOrValue() As Object) As List(Of BoomCycleSqlParams)

        Dim sqlParams As List(Of BoomCycleSqlParams) = New List(Of BoomCycleSqlParams)()

        sqlParams.Add(New BoomCycleSqlParams(param0, value0))

        If paramOrValue IsNot Nothing Then
            For i As Integer = 0 To UBound(paramOrValue) \ 2
                If (2 * i + 1 <= UBound(paramOrValue)) Then
                    Dim param = paramOrValue(2 * i)
                    Debug.Assert(TypeOf (param) Is String)
                    Dim val = paramOrValue(2 * i + 1)
                    sqlParams.Add(New BoomCycleSqlParams(param, val))
                End If
            Next
        End If

        Return sqlParams

    End Function

End Class

