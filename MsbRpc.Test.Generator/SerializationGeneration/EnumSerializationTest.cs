#region

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Serialization.Default;
using MsbRpc.Test.Generator.SerializationGeneration.Utility;

#endregion

namespace MsbRpc.Test.Generator.SerializationGeneration;

[TestClass]
[TestCategory(TestCategories.Serialization)]
public class EnumSerializationTest : Base.Test
{
    private const string EnumName = "MyEnum";

    private static readonly NamedTypeDeclarationInfo EnumDeclaration = new(EnumName, 0, EnumSerializationKind.Int);

    private static readonly TypeReferenceInfo EnumInfo = new(EnumDeclaration);

    private static ISerialization Serialization => new SerializationResolver().Resolve(EnumInfo);

    [TestMethod]
    public void SerializationIsResolved()
    {
        Assert.IsTrue(Serialization.IsResolved);
    }

    [TestMethod]
    public void NeedsSemicolonAfterSerializationStatement()
    {
        Assert.IsTrue(Serialization.NeedsSemicolonAfterSerializationStatement);
    }

    [TestMethod]
    public void DeclarationSyntaxIsCorrect()
    {
        string actual = Serialization.DeclarationSyntax;
        Assert.AreEqual(EnumName, actual);
        TestContext.WriteLine(actual);
    }

    [TestMethod]
    public void DeserializationExpressionIsCorrect()
    {
        const string expected = @"(MyEnum)(bufferReader.ReadInt());
";
        string actual = new SerializationTest(EnumInfo).GetFinalizedDeserializationExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void SerializationStatementIsCorrect()
    {
        const string expected = @"bufferWriter.Write((int)value);
";
        string actual = new SerializationTest(EnumInfo).GetFinalizedSerializationStatement();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void SizeExpressionIsCorrect()
    {
        const string expected =
            @"MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize";
        string actual = new SerializationTest(EnumInfo).GetSizeExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }
}