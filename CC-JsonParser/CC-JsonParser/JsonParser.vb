

Public Class JsonParser

    ''' <summary>
    ''' Parses the JSON value into an Object
    ''' </summary>
    ''' <param name="source">The Source of the JSON value. Must implement the IJsonSource interface.</param>
    ''' <returns>
    ''' Returns either a JSON object, a native object, or throws an exception if the value cannot be parsed. 
    ''' </returns>
    Public Shared Function Parse(source As IJsonSource) As Object
        Dim json = source.ReadJson
        If String.IsNullOrWhiteSpace(json) Then
            Throw New InvalidJsonException("No JSON data provided.")
        End If
        Dim i As Integer = 0
        Dim value = ParseValue(json, i)
        If IsNothing(value) Then
            Throw New InvalidJsonException("JSON value was not parsed as expected. No object returned from parser.")
            Return Nothing
        End If
        Functions.SkipWhitespace(json, i)
        If i <> json.Length Then
            Throw New InvalidJsonException("Extra characters after valid JSON")
        End If
        Return value
    End Function


    Private Shared Function ParseValue(json As String, ByRef i As Integer) As Object
        Functions.SkipWhitespace(json, i)
        If i >= json.Length Then Throw New InvalidJsonException("Error Parsing JSON. Too many characters: " & i)

        Select Case json(i)
            Case "{"c
                Return ParseObject(json, i)
            Case "["c
                Return ParseArray(json, i)
            Case """"c
                Return ParseString(json, i)
            Case "t"c, "f"c
                Dim bResult As Boolean
                If TryParseBoolean(json, i, bResult) Then Return bResult
            Case "n"c
                ' discard value
                If TryParseNull(json, i) Then Return Nothing
            Case "-"c, "0"c To "9"c
                Return ParseNumber(json, i)

        End Select

        Throw New InvalidJsonException("Parsing Error on Line " & i)
        Return Nothing
    End Function

    Private Shared Function ParseObject(json As String, ByRef i As Integer) As Json
        If json(i) <> "{"c Then Throw New InvalidJsonException(String.Format("Error Parsing JSON at character {0}. Expected '{' for start of object.", i))
        i += 1
        SkipWhitespace(json, i)

        Dim result As New Json

        If i < json.Length AndAlso json(i) = "}"c Then
            i += 1
            Return result
        End If

        While True
            Dim key As String = ParseString(json, i)
            SkipWhitespace(json, i)

            If i >= json.Length OrElse json(i) <> ":"c Then Throw New InvalidJsonException(String.Format("Error Parsing JSON at character {0}. Missing ':' separator.", i))
            i += 1

            Dim value As Object = ParseValue(json, i)
            result.SetValue(key, value)

            SkipWhitespace(json, i)
            If i >= json.Length Then Throw New InvalidJsonException("Error Parsing JSON. Too many characters: " & i)

            If json(i) = "}"c Then
                i += 1
                Exit While
            End If

            If json(i) <> ","c Then Throw New InvalidJsonException(String.Format("Error Parsing JSON at character {0}. Expected ',' separator.", i))
            i += 1
            SkipWhitespace(json, i)
        End While

        Return result
    End Function

    Private Shared Function TryParseBoolean(json As String, ByRef i As Integer, ByRef result As Boolean) As Boolean
        If json.Substring(i).StartsWith("true") Then
            result = True
            i += 4
            Return True
        ElseIf json.Substring(i).StartsWith("false") Then
            result = False
            i += 5
            Return True
        End If
        Return False
    End Function

    Private Shared Function TryParseNull(json As String, ByRef i As Integer) As Boolean
        If json.Substring(i).StartsWith("null") Then
            i += 4
            Return True
        End If
        Return False
    End Function



    Private Shared Function ParseArray(json As String, ByRef i As Integer) As List(Of Object)
        If json(i) <> "["c Then Throw New InvalidJsonException(String.Format("Error Parsing JSON at character {0}. Expected '[' for start of array.", i))
        i += 1
        SkipWhitespace(json, i)

        Dim items As New List(Of Object)

        If i < json.Length AndAlso json(i) = "]"c Then
            i += 1
            Return items
        End If

        While True
            Dim value = ParseValue(json, i)
            items.Add(value)
            SkipWhitespace(json, i)

            If i >= json.Length Then Throw New InvalidJsonException("Error Parsing JSON. Too many characters: " & i)

            If json(i) = "]"c Then
                i += 1
                Exit While
            End If

            If json(i) <> ","c Then Throw New InvalidJsonException(String.Format("Error Parsing JSON at character {0}. Expected ',' separator.", i))
            i += 1
            SkipWhitespace(json, i)
        End While

        Return items
    End Function


    Private Shared Function ParseString(json As String, ByRef i As Integer) As String
        If json(i) <> """"c Then Throw New InvalidJsonException(String.Format("Error Parsing JSON at character {0}. Expected '""' for start of a string.", i))
        i += 1
        Dim sb As New System.Text.StringBuilder

        While i < json.Length
            If json(i) = "\"c Then
                i += 1
                If i >= json.Length Then Throw New InvalidJsonException("Error Parsing JSON. Too many characters: " & i)
                Select Case json(i)
                    Case """"c : sb.Append(""""c)
                    Case "\"c : sb.Append("\"c)
                    Case "/"c : sb.Append("/"c)
                    Case "b"c : sb.Append(ControlChars.Back)
                    Case "f"c : sb.Append(ControlChars.FormFeed)
                    Case "n"c : sb.Append(ControlChars.Lf)
                    Case "r"c : sb.Append(ControlChars.Cr)
                    Case "t"c : sb.Append(ControlChars.Tab)
                    Case "u"c
                        If i + 4 < json.Length Then
                            Dim hex = json.Substring(i + 1, 4)
                            sb.Append(ChrW(Convert.ToInt32(hex, 16)))
                            i += 4
                        End If
                    Case Else
                        Throw New InvalidJsonException(String.Format("Error Parsing JSON at character {0}. Unexpected Escape Character {1}.", i, json(i))) ' Invalid escape
                End Select
            ElseIf json(i) = """"c Then
                i += 1
                Return sb.ToString()
            Else
                sb.Append(json(i))
            End If
            i += 1
        End While
        Return Nothing
    End Function

    Private Shared Function TryParseLiteral(Of T)(json As String, ByRef i As Integer, expected As String, value As T) As (Boolean, T)
        If json.Substring(i).StartsWith(expected) Then
            i += expected.Length
            Return (True, value)
        End If
        Return (False, Nothing)
    End Function

    Private Shared Function ParseNumber(json As String, ByRef i As Integer) As Object
        Dim start As Integer = i

        If json(i) = "-"c Then i += 1
        If i >= json.Length Then Throw New InvalidJsonException("Error Parsing JSON. Too many characters: " & i)

        If json(i) = "0"c Then
            i += 1
        ElseIf json(i) >= "1"c AndAlso json(i) <= "9"c Then
            While i < json.Length AndAlso Char.IsDigit(json(i))
                i += 1
            End While
        Else
            Return Nothing
        End If

        Dim hasDecimal As Boolean = False
        If i < json.Length AndAlso json(i) = "."c Then
            hasDecimal = True
            i += 1
            If i >= json.Length OrElse Not Char.IsDigit(json(i)) Then Throw New InvalidJsonException(String.Format("Error Parsing JSON at character {0}. Unexpected non-numeric value.", i))
            While i < json.Length AndAlso Char.IsDigit(json(i))
                i += 1
            End While
        End If

        If i < json.Length AndAlso (json(i) = "e"c OrElse json(i) = "E"c) Then
            hasDecimal = True
            i += 1
            If i < json.Length AndAlso (json(i) = "+"c OrElse json(i) = "-"c) Then i += 1
            If i >= json.Length OrElse Not Char.IsDigit(json(i)) Then Throw New InvalidJsonException(String.Format("Error Parsing JSON at character {0}. Unexpected non-numeric value.", i))
            While i < json.Length AndAlso Char.IsDigit(json(i))
                i += 1
            End While
        End If

        Dim numberStr = json.Substring(start, i - start)
        If hasDecimal Then
            Dim dbl As Double
            If Double.TryParse(numberStr, dbl) Then Return dbl
        Else
            Dim intVal As Long
            If Long.TryParse(numberStr, intVal) Then Return intVal
        End If

        Throw New InvalidJsonException("Could Not Parse JSON. Expected a Number at character " & i)
        Return Nothing ' Could not parse
    End Function



End Class
