Imports BoomCycle.Core

Public Class BoomCycleDataException
    Inherits BoomCycleExceptionBase

    ''' <summary>
    ''' This is the contructor for our custom exception not based on a .net framework exception
    ''' </summary>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Sub New(message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' '''This is the contructor for our custom exception based on a .net framework exception
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="originalException"></param>
    ''' <remarks></remarks>
    Sub New(message As String, originalException As Exception)
        MyBase.New(message, originalException)
    End Sub

End Class
