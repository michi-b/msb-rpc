using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Test.Generator.SerializationGeneration.Tests;

[TestClass]
public class NullableByteSerializationTests : Base.Test
{
    private const string NullableSizeExpression = @"MsbRpc.Serialization.NullableSerializer<byte>.GetSize
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

    private static readonly TypeReferenceInfo NullableNullableBoolInfo = new
    (
        new TypeDeclarationInfo("System.Nullable", 1),
        false,
        new[] { NullableBoolInfo }.ToImmutableList()
    );

    [TestMethod]
    public void SerializationsAreResolved()
    {
        foreach (ISerialization serialization in GetSerializations())
        {
            Assert.IsTrue(serialization.GetIsResolved());
        }
    }

    [TestMethod]
    public void SerializationsAreNotVoid()
    {
        foreach (ISerialization serialization in GetSerializations())
        {
            Assert.IsFalse(serialization.GetIsVoid());
        }
    }

    [TestMethod]
    public void NullableDeclarationSyntaxIsCorrect()
    {
        Assert.AreEqual("System.Nullable<byte>", Resolve(NullableBoolInfo).GetDeclarationSyntax());
    }

    [TestMethod]
    public void NullableNullableDeclarationSyntaxIsCorrect()
    {
        Assert.AreEqual("System.Nullable<System.Nullable<byte>>", Resolve(NullableNullableBoolInfo).GetDeclarationSyntax());
    }

    [TestMethod]
    public void NullableSizeExpressionIsCorrect()
    {
        new SerializationTest(NullableBoolInfo) { ExpectedSizeExpression = NullableSizeExpression }.Run();
    }

    private static IEnumerable<ISerialization> GetSerializations()
    {
        SerializationResolver resolver = CreateResolver();
        yield return resolver.Resolve(NullableBoolInfo);
        yield return resolver.Resolve(NullableNullableBoolInfo);
    }

    private static ISerialization Resolve(TypeReferenceInfo typeReferenceInfo)
    {
        SerializationResolver resolver = CreateResolver();
        return resolver.Resolve(typeReferenceInfo);
    }

    private static SerializationResolver CreateResolver() => new(ImmutableArray<CustomSerializationInfo>.Empty);
}