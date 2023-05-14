﻿using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Test.Generator.SerializationGeneration.Tests;

[TestClass]
public class NullableByteSerializationTests : Base.Test
{
    private static readonly TypeReferenceInfo NullableBoolInfo = new
    (
        new NamedTypeDeclarationInfo("System.Nullable", 1),
        new[] { TypeReferenceInfo.CreateSimple("System.Byte") }.ToImmutableList()
    );

    private static ISerialization Serialization => new SerializationResolver().Resolve(NullableBoolInfo);

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
        Assert.AreEqual("byte?", actual);
        TestContext.WriteLine(actual);
    }

    [TestMethod]
    public void DeserializationExpressionIsCorrect()
    {
        const string expected = @"MsbRpc.Serialization.NullableSerializer<byte>.Read
(
    ref bufferReader,
    (responseReader) => responseReader.ReadByte());
";
        string actual = new SerializationTest(NullableBoolInfo).GetFinalizedDeserializationExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void SerializationStatementIsCorrect()
    {
        const string expected = @"MsbRpc.Serialization.NullableSerializer<byte>.Write
(
    ref bufferWriter,
    value,
    (requestWriter, innerValue) => 
    {
        requestWriter.Write(innerValue);
    }
);
";
        string actual = new SerializationTest(NullableBoolInfo).GetFinalizedSerializationStatement();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void SizeExpressionIsCorrect()
    {
        const string expected = @"(MsbRpc.Serialization.Primitives.PrimitiveSerializer.ByteSize + MsbRpc.Serialization.Primitives.PrimitiveSerializer.BoolSize)";
        string actual = new SerializationTest(NullableBoolInfo).GetSizeExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }
}