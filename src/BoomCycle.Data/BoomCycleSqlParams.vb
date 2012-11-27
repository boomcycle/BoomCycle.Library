
Public Class BoomCycleSqlParams
    ' BTO 8/25/12 Changed "val", "Value", "_Value", etc. from String to Object throughout.
    ' Numeric parameters weren't working.

    '    Public Sub New(ByVal name As String, ByVal value As String)
    Public Sub New(ByVal name As String, ByVal value As Object)
        Me._Name = name
        Me._Value = value
    End Sub

    Private _Name As String
    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Private _Value As Object
    Public ReadOnly Property Value() As Object
        Get
            Return _Value
        End Get
    End Property
End Class
