Imports CC_JsonParser
Imports CC_JsonParser.Sources
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace JsonParser_Tests
    <TestClass>
    Public Class JsonParserTests
        <TestMethod>
        Public Sub TestJsonParsingLiteralKeys()
            Dim input = "{""key"":""value"", ""key-n"":101, ""key-o"":{}, ""key-l"":[]}"
            Dim result = JsonParser.Parse(New StringJsonSource(input))

            Assert.IsNotNull(result)
            Assert.IsInstanceOfType(result, GetType(Json))
        End Sub

        <TestMethod>
        Public Sub Test_OpenClose_ShouldReturnJsonObject()
            Dim input As New CC_JsonParser.Sources.StringJsonSource("{}")
            Assert.IsInstanceOfType(Of CC_JsonParser.Json)(CC_JsonParser.JsonParser.Parse(input))
        End Sub


        <TestMethod>
        Public Sub Test_EmptyString_ShouldReturnNothing()

            Try
                Dim input As New CC_JsonParser.Sources.StringJsonSource("")
                Dim result = JsonParser.Parse(input)
                Assert.Fail("Expected exception was not thrown.")
            Catch ex As Exception
                Assert.IsTrue(TypeOf ex Is InvalidJsonException) ' You can narrow this if needed
            End Try

        End Sub

        <TestMethod>
        Public Sub Test_MalformedLvl1Key_ShouldReturnNothing()
            Try
                Dim input As New CC_JsonParser.Sources.StringJsonSource("{ ""key"": ""value"",  key2: ""value""}")
                Dim result = JsonParser.Parse(input)
                Assert.Fail("Expected exception was not thrown.")
            Catch ex As Exception
                Assert.IsTrue(TypeOf ex Is InvalidJsonException) ' You can narrow this if needed
            End Try

        End Sub

        <TestMethod>
        Public Sub Test_TrailingComma_ShouldReturnNothing()
            Try
                Dim Input As New CC_JsonParser.Sources.StringJsonSource("{ ""key"": ""value"", }")
                Dim result = JsonParser.Parse(Input)
                Assert.Fail("Expected exception was not thrown.")
            Catch ex As Exception
                Assert.IsTrue(TypeOf ex Is InvalidJsonException) ' You can narrow this if needed
            End Try
        End Sub


        <TestMethod>
        Public Sub Test_OneKeyLvl1_ShouldReturnJsonObject()
            Dim input As New CC_JsonParser.Sources.StringJsonSource("{""key"": ""value""}")
            Assert.IsInstanceOfType(Of CC_JsonParser.Json)(CC_JsonParser.JsonParser.Parse(input))
        End Sub

        <TestMethod>
        Public Sub Test_TwoKeysLvl1_ShouldReturnJsonObject()
            Dim input As New CC_JsonParser.Sources.StringJsonSource("{""key"": ""value"", ""key2"": ""value""}")
            Assert.IsInstanceOfType(Of CC_JsonParser.Json)(CC_JsonParser.JsonParser.Parse(input))
        End Sub

        <TestMethod>
        Public Sub TestParsedJson()
            Dim jsonString As String = "{
        ""name"": ""Alice"",
        ""age"": 30,
        ""isMember"": true,
        ""scores"": [85, 90, 88],
        ""profile"": {
            ""email"": ""alice@example.com"",
            ""verified"": false
        }
    }"

            Dim source As IJsonSource = New StringJsonSource(jsonString)
            Dim result As Object = JsonParser.Parse(source)

            ' First, check it’s a Json object
            Assert.IsInstanceOfType(result, GetType(Json))

            Dim obj As Json = CType(result, Json)

            ' Validate primitives
            Assert.AreEqual("Alice", obj.GetValue("name"))
            Assert.AreEqual(30L, obj.GetValue("age"))
            Assert.AreEqual(True, obj.GetValue("isMember"))

            ' Validate array
            Dim scores = CType(obj.GetValue("scores"), List(Of Object))
            Assert.AreEqual(3, scores.Count)
            Assert.AreEqual(85L, scores(0))
            Assert.AreEqual(90L, scores(1))
            Assert.AreEqual(88L, scores(2))

            ' Validate nested object
            Dim profile = CType(obj.GetValue("profile"), Json)
            Assert.AreEqual("alice@example.com", profile.GetValue("email"))
            Assert.AreEqual(False, profile.GetValue("verified"))
        End Sub


        <TestMethod>
        Public Sub TestRootLevelElements()
            Assert.AreEqual("hello", JsonParser.Parse(New StringJsonSource("""hello""")))
            Assert.AreEqual(False, JsonParser.Parse(New StringJsonSource("false")))
            Assert.AreEqual(True, JsonParser.Parse(New StringJsonSource("true")))
            Assert.AreEqual("theo", JsonParser.Parse(New StringJsonSource("""theo""")))
            Assert.AreEqual(19L, JsonParser.Parse(New StringJsonSource("19")))
            Assert.AreEqual(Convert.ToDouble(19.2), JsonParser.Parse(New StringJsonSource("19.2")))
        End Sub

        <TestMethod>
        Public Sub TestRootLevelArray()
            Dim source As New StringJsonSource("[1, 2, 3]")
            Dim result As Object = JsonParser.Parse(source)
            Dim list = CType(result, List(Of Object))
            Assert.AreEqual(3, list.Count)
        End Sub



    End Class
End Namespace

