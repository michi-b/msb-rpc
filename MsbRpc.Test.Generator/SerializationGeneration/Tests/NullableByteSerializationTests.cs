using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Test.Generator.SerializationGeneration.Tests;

[TestClass]
public class NullableByteSerializationTests : Base.Test
{
    private const string ExpectedSizeExpression = @"MsbRpc.Serialization.NullableSerializer<byte>.GetSize
(
    target,
    (innerValue) => MsbRpc.Serialization.Primitives.PrimitiveSerializer.ByteSize
)";

    private static readonly TypeReferenceInfo NullableBoolInfo = new
    (
        new TypeDeclarationInfo("System.Nullable", 1),
        false,
        new[] { TypeReferenceInfo.CreateSimple("System.Byte") }.ToImmutableList()
    );

    private static ISerialization Serialization
    {
        get
        {
            SerializationResolver resolver = new(ImmutableArray<CustomSerializationInfo>.Empty);
            return resolver.Resolve(NullableBoolInfo);
        }
    }

    [TestMethod]
    public void SerializationsAreResolved()
    {
        Assert.IsTrue(Serialization.GetIsResolved());
    }

    [TestMethod]
    public void SerializationsAreNotVoid()
    {
        Assert.IsFalse(Serialization.GetIsVoid());
    }

    [TestMethod]
    public void NullableDeclarationSyntaxIsCorrect()
    {
        Assert.AreEqual("System.Nullable<byte>", Serialization.GetDeclarationSyntax());
    }

    [TestMethod]
    public void NullableSizeExpressionIsCorrect()
    {
        new SerializationTest(NullableBoolInfo) { ExpectedSizeExpression = ExpectedSizeExpression }.Run();
    }
}