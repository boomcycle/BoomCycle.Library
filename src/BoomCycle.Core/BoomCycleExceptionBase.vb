Public Class BoomCycleExceptionBase
    Inherits Exception

    Protected _OriginalException As Exception
    Protected _Message As String

    ''' <summary>
    ''' This is the contructor for our custom exception not based on a .net framework exception
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Sub New(message As String)
        ' TODO: Complete member initialization 
        _Message = message
        _OriginalException = Nothing
    End Sub

    ''' <summary>
    ''' '''This is the contructor for our custom exception based on a .net framework exception
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="originalException"></param>
    ''' <remarks></remarks>
    Sub New(message As String, originalException As Exception)
        ' TODO: Complete member initialization 
        _Message = message
        _OriginalException = originalException
    End Sub

    Protected Sub New()
        ' TODO: Complete member initialization 
    End Sub

    Public Overrides ReadOnly Property Message As String
        Get
            Return _Message
        End Get
    End Property

    Public ReadOnly Property OriginalException As Exception
        Get
            Return _OriginalException
        End Get
    End Property
End Class
