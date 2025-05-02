
Namespace Sources
    Public Class StringJsonSource
        Implements IJsonSource

        Private ReadOnly _json As String

        Public Sub New(json As String)
            _json = json
        End Sub

        Public Function ReadJson() As String Implements IJsonSource.ReadJson
            Return _json
        End Function
    End Class

End Namespace

