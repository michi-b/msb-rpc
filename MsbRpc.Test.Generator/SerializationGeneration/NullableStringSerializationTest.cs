﻿#region

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Test.Generator.SerializationGeneration.Utility;

#endregion

namespace MsbRpc.Test.Generator.SerializationGeneration;

[TestClass]
[TestCategory(TestCategories.Serialization)]
public class NullableStringSerializationTest : Base.Test
{
    private static readonly TypeReferenceInfo NullableStringInfo = TypeReferenceInfo.CreateSimple("System.String", true);

    private static ISerialization Serialization => new SerializationResolver().Resolve(NullableStringInfo);

    [TestMethod]
    public void SerializationIsResolved()
    {
        Assert.IsTrue(Serialization.IsResolved);
    }

    [TestMethod]
    public void DoesNotNeedSemicolonAfterSerializationStatement()
    {
        Assert.IsFalse(Serialization.NeedsSemicolonAfterSerializationStatement);
    }

    [TestMethod]
    public void DeclarationSyntaxIsCorrect()
    {
        string actual = Serialization.DeclarationSyntax;
        Assert.AreEqual(@"string?", actual);
        TestContext.WriteLine(actual);
    }

    [TestMethod]
    public void DeserializationExpressionIsCorrect()
    {
        const string expected = @"(bufferReader.ReadBool() ? bufferReader.ReadString() : null);
";
        string actual = new SerializationTest(NullableStringInfo).GetFinalizedDeserializationExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void SerializationStatementIsCorrect()
    {
        const string expected = @"if (value == null)
{
    bufferWriter.Write(false);
}
else
{
    bufferWriter.Write(true);
    bufferWriter.Write(value);
}
";
        string actual = new SerializationTest(NullableStringInfo).GetFinalizedSerializationStatement();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void SizeExpressionIsCorrect()
    {
        const string expected =
            @"(target == null ? MsbRpc.Serialization.Primitives.PrimitiveSerializer.BoolSize : MsbRpc.Serialization.Primitives.PrimitiveSerializer.BoolSize + MsbRpc.Serialization.StringSerializer.GetSize(target))";
        string actual = new SerializationTest(NullableStringInfo).GetSizeExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }
}