Namespace Sources
    Public Class FileJsonSource
        Implements IJsonSource

        Private ReadOnly _filename As String

        Public Sub New(filename As String)
            _filename = filename
        End Sub

        Public Function ReadJson() As String Implements IJsonSource.ReadJson
            Return System.IO.File.ReadAllText(_filename)
        End Function
    End Class

End Namespace
