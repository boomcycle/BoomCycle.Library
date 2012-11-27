
Public Class BoomCycleDataRow
    Private _Dr As DataRow    ' a delegate

    Sub New(ByRef dr As DataRow)
        _Dr = dr
    End Sub

    Public ReadOnly Property GetValues As Object()
        Get
            Return _Dr.ItemArray()
        End Get
    End Property

    Public Function GetValue(columnName As String) As Object
        Return _Dr(columnName)
    End Function

    ' BTO 8/16/2012
    Public Function SetValue( _
                            ByVal columnName As String, _
                            ByVal val As Object) As Object
        _Dr(columnName) = val
        Return val
    End Function

End Class
