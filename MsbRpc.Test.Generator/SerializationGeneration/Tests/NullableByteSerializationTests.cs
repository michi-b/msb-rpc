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
    public void DeclarationSyntaxIsCorrect()
    {
        string actual = Serialization.GetDeclarationSyntax();
        Assert.AreEqual("byte?", actual);
        TestContext.WriteLine(actual);
    }

    [TestMethod]
    public void SizeExpressionIsCorrect()
    {
        const string expected = @"MsbRpc.Serialization.NullableSerializer<byte>.GetSize
(
    target,
    (innerValue) => MsbRpc.Serialization.Primitives.PrimitiveSerializer.ByteSize
)";
        string actual = new SerializationTest(NullableBoolInfo).GetSizeExpression();
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
    (bufferWriter, innerValue) => 
    {
        bufferWriter.Write(innerValue);
    }
);
";
        string actual = new SerializationTest(NullableBoolInfo).GetSerializationStatement();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }

    [TestMethod]
    public void DeserializationExpressionIsCorrect()
    {
        const string expected = @"MsbRpc.Serialization.NullableSerializer<byte>.Read
(
    
)";
        string actual = new SerializationTest(NullableBoolInfo).GetDeserializationExpression();
        Assert.AreEqual(expected, actual);
        TestContext.Write(actual);
    }
}