#region

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Test.Generator.SerializationGeneration.Utility;

#endregion

namespace MsbRpc.Test.Generator.SerializationGeneration;

[TestClass]
[TestCategory(TestCategories.Serialization)]
public class ArraySerializationTest : Base.Test
{
    private static readonly TypeReferenceInfo IntInfo = new(new NamedTypeDeclarationInfo("System.Int32"));

    private static readonly TypeReferenceInfo IntArrayInfo = new(new ArrayDeclarationInfo(IntInfo, 1));

    private static ISerialization Serialization => new SerializationResolver().Resolve(IntArrayInfo);

    [TestMethod]
    public void SerializationIsResolved()
    {
        Assert.IsTrue(Serialization.IsResolved);
    }

    [TestMethod]
    public void SerializationsAreNotVoid()
    {
        Assert.IsFalse(Serialization.IsVoid);
    }

    [TestMethod]
    public void DeclarationSyntaxIsCorrect()
    {
        string actual = Serialization.DeclarationSyntax;
        Assert.AreEqual(@"int[]", actual);
        TestContext.WriteLine(actual);
    }

    [TestMethod]
    public void DeserializationExpressionIsCorrect()
    {
        const string expected = @"MsbRpc.Serialization.Arrays.ArraySerializer<int>.Read
(
    ref bufferReader,
    (ref MsbRpc.Serialization.Buffers.BufferReader reader) => reader.ReadInt()
);
";
        string actual = new SerializationTest(IntArrayInfo).GetFinalizedDeserializationExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void SerializationStatementIsCorrect()
    {
        const string expected = @"MsbRpc.Serialization.Arrays.ArraySerializer<int>.Write
(
    ref bufferWriter,
    value,
    (ref MsbRpc.Serialization.Buffers.BufferWriter writer, int element) => 
    {
        writer.Write(element);
    }
);
";
        string actual = new SerializationTest(IntArrayInfo).GetFinalizedSerializationStatement();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void SizeExpressionIsCorrect()
    {
        const string expected =
            @"MsbRpc.Serialization.Arrays.ArraySerializer<int>.GetSize(
    target,
    MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize
)";
        string actual = new SerializationTest(IntArrayInfo).GetSizeExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }
}