Imports CC_JsonParser
Imports System.IO
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CC_JsonParser.Sources

Namespace JsonParser_Tests
    <TestClass>
    Public Class FileTests



        <TestMethod>
        Public Sub test_file_test1_valid()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test1-valid.json"))
            Assert.IsInstanceOfType(Of CC_JsonParser.Json)(CC_JsonParser.JsonParser.Parse(source))
        End Sub

        <TestMethod>
        Public Sub test_file_test1_invalid()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test1-invalid.json"))

            Try
                Dim result = JsonParser.Parse(source)
                Assert.Fail("Expected exception was not thrown.")
            Catch ex As Exception
                Assert.IsTrue(TypeOf ex Is Exception) ' You can narrow this if needed
            End Try
        End Sub


        <TestMethod>
        Public Sub test_file_test2_valid()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test2-valid.json"))
            Assert.IsInstanceOfType(Of CC_JsonParser.Json)(CC_JsonParser.JsonParser.Parse(source))
        End Sub

        <TestMethod>
        Public Sub test_file_test2_invalid()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test2-invalid.json"))

            Try
                Dim result = JsonParser.Parse(source)
                Assert.Fail("Expected exception was not thrown.")
            Catch ex As Exception
                Assert.IsTrue(TypeOf ex Is Exception) ' You can narrow this if needed
            End Try
        End Sub


        <TestMethod>
        Public Sub test_file_test2_valid2()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test2-valid2.json"))
            Assert.IsInstanceOfType(Of CC_JsonParser.Json)(CC_JsonParser.JsonParser.Parse(source))
        End Sub

        <TestMethod>
        Public Sub test_file_test2_invalid2()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test2-invalid2.json"))

            Try
                Dim result = JsonParser.Parse(source)
                Assert.Fail("Expected exception was not thrown.")
            Catch ex As Exception
                Assert.IsTrue(TypeOf ex Is Exception) ' You can narrow this if needed
            End Try
        End Sub

        <TestMethod>
        Public Sub test_file_test3_valid()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test3-valid.json"))
            Assert.IsInstanceOfType(Of CC_JsonParser.Json)(CC_JsonParser.JsonParser.Parse(source))
        End Sub

        <TestMethod>
        Public Sub test_file_test3_invalid()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test3-invalid.json"))

            Try
                Dim result = JsonParser.Parse(source)
                Assert.Fail("Expected exception was not thrown.")
            Catch ex As Exception
                Assert.IsTrue(TypeOf ex Is Exception) ' You can narrow this if needed
            End Try
        End Sub



        <TestMethod>
        Public Sub test_file_test4_valid()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test4-valid.json"))
            Dim r = CC_JsonParser.JsonParser.Parse(source)
            Assert.IsInstanceOfType(Of CC_JsonParser.Json)(r)
        End Sub
        <TestMethod>
        Public Sub test_file_test4_valid2()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test4-valid2.json"))
            Assert.IsInstanceOfType(Of CC_JsonParser.Json)(CC_JsonParser.JsonParser.Parse(source))
        End Sub

        <TestMethod>
        Public Sub test_file_test4_invalid()
            Dim source As New FileJsonSource(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test4-invalid.json"))

            Try
                Dim result = JsonParser.Parse(source)
                Assert.Fail("Expected exception was not thrown.")
            Catch ex As Exception
                Assert.IsTrue(TypeOf ex Is Exception) ' You can narrow this if needed
            End Try
        End Sub

    End Class
End Namespace

