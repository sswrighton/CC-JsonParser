Module Functions
    Public Sub SkipWhitespace(json As String, ByRef i As Integer)
        While i < json.Length AndAlso Char.IsWhiteSpace(json(i))
            i += 1
        End While
    End Sub
End Module
