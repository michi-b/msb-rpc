using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Test.Generator.SerializationGeneration.Tests;

[TestClass]
public class StringSerializationTest : Base.Test
{
    private static readonly TypeReferenceInfo NullableStringInfo = TypeReferenceInfo.CreateSimple("System.String");

    private static ISerialization Serialization => new SerializationResolver().Resolve(NullableStringInfo);

    [TestMethod]
    public void SerializationIsResolved()
    {
        Assert.IsTrue(Serialization.IsResolved);
    }

    [TestMethod]
    public void DoesNotNeedSemicolonAfterSerializationStatement()
    {
        Assert.IsTrue(Serialization.NeedsSemicolonAfterSerializationStatement);
    }

    [TestMethod]
    public void DeclarationSyntaxIsCorrect()
    {
        string actual = Serialization.DeclarationSyntax;
        Assert.AreEqual(@"string", actual);
        TestContext.WriteLine(actual);
    }

    [TestMethod]
    public void SizeExpressionIsCorrect()
    {
        const string expected =
            @"MsbRpc.Serialization.StringSerializer.GetSize(target)";
        string actual = new SerializationTest(NullableStringInfo).GetSizeExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void SerializationStatementIsCorrect()
    {
        const string expected = @"bufferWriter.Write(value);
";
        string actual = new SerializationTest(NullableStringInfo).GetFinalizedSerializationStatement();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void DeserializationExpressionIsCorrect()
    {
        const string expected = @"bufferReader.ReadString();
";
        string actual = new SerializationTest(NullableStringInfo).GetFinalizedDeserializationExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }
}