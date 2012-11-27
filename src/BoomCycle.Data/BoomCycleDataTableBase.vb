Imports System.ComponentModel

Public MustInherit Class BoomCycleDataTableBase
    Private _WrappedObject As Object
    Private _ListOfAFFDrs As BindingList(Of BoomCycleDataRow)

    ' BTO 8/16/2012
    Public Sub New(ByVal dt As DataTable)
        WrappedObject() = dt      ' triggers a call to PopulateListOfAFF
    End Sub

    Protected Overrides Sub Finalize()
        _WrappedObject = Nothing
        _ListOfAFFDrs = Nothing
    End Sub

    Public Overridable Property WrappedObject() As Object
        Get
            Return _WrappedObject
        End Get
        Set(ByVal value As Object)
            _WrappedObject = value
            PopulateListOfAFF()
        End Set
    End Property

    Public Overridable ReadOnly Property AFFDataRowsList() As BindingList(Of BoomCycleDataRow)
        Get
            If _ListOfAFFDrs Is Nothing Then
                _ListOfAFFDrs = New BindingList(Of BoomCycleDataRow)()
            End If

            Return _ListOfAFFDrs
        End Get
    End Property

    Protected MustOverride Sub PopulateListOfAFF()
    Public MustOverride Sub DoQuery(ByVal conn As String, ByVal sql As String, ByVal sqlParams As List(Of BoomCycleSqlParams))
    Public MustOverride Function DoCommand(ByVal conn As String, ByVal sql As String, ByVal sqlParams As List(Of BoomCycleSqlParams)) As Integer
    Public MustOverride Function GetColumns() As List(Of String)
    Public MustOverride Function RecordCount() As Integer
    Public MustOverride Sub AddColumn(ByVal colName As String)
    Public MustOverride Function NewRow() As BoomCycleDataRow
    Public MustOverride Sub AddRow(ByVal row As BoomCycleDataRow)
End Class
