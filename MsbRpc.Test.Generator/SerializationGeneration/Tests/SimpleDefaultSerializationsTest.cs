using System;
using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Serialization.Default;

namespace MsbRpc.Test.Generator.SerializationGeneration.Tests;

[TestClass]
public class SimpleDefaultSerializationsTest : Base.Test
{
    [TestMethod]
    public void HasAllDefaultSerializations()
    {
        SerializationResolver resolver = CreateResolver();
        foreach (SimpleDefaultSerializationKind serializationKind in SimpleDefaultSerializationKindUtility.All)
        {
            ISerialization serialization = resolver.Resolve(serializationKind.GetTargetType());
            Assert.IsNotNull(serialization);
        }
    }

    [TestMethod]
    public void CreatesCorrectByteSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Byte);
    }

    [TestMethod]
    public void CreatesCorrectSbyteSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Sbyte);
    }

    [TestMethod]
    public void CreatesCorrectBoolSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Bool);
    }

    [TestMethod]
    public void CreatesCorrectCharSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Char);
    }

    [TestMethod]
    public void CreatesCorrectIntSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Int);
    }

    [TestMethod]
    public void CreatesCorrectLongSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Long);
    }

    [TestMethod]
    public void CreatesCorrectShortSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Short);
    }

    [TestMethod]
    public void CreatesCorrectUintSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Uint);
    }

    [TestMethod]
    public void CreatesCorrectUlongSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Ulong);
    }

    [TestMethod]
    public void CreatesCorrectUshortSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Ushort);
    }

    [TestMethod]
    public void CreatesCorrectFloatSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Float);
    }

    [TestMethod]
    public void CreatesCorrectDoubleSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Double);
    }

    [TestMethod]
    public void CreatesCorrectDecimalSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.Decimal);
    }

    [TestMethod]
    public void CreatesCorrectStringSerialization()
    {
        RunSerializationTest(SimpleDefaultSerializationKind.String);
    }

    private static string GetExpectedDeclarationSyntax(SimpleDefaultSerializationKind target)
        => target switch
        {
            SimpleDefaultSerializationKind.Byte => "byte",
            SimpleDefaultSerializationKind.Sbyte => "sbyte",
            SimpleDefaultSerializationKind.Bool => "bool",
            SimpleDefaultSerializationKind.Char => "char",
            SimpleDefaultSerializationKind.Int => "int",
            SimpleDefaultSerializationKind.Long => "long",
            SimpleDefaultSerializationKind.Short => "short",
            SimpleDefaultSerializationKind.Uint => "uint",
            SimpleDefaultSerializationKind.Ulong => "ulong",
            SimpleDefaultSerializationKind.Ushort => "ushort",
            SimpleDefaultSerializationKind.Float => "float",
            SimpleDefaultSerializationKind.Double => "double",
            SimpleDefaultSerializationKind.Decimal => "decimal",
            SimpleDefaultSerializationKind.String => "string",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    private static string GetExpectedSizeExpression(SimpleDefaultSerializationKind target)
        => target switch
        {
            SimpleDefaultSerializationKind.Byte => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.ByteSize",
            SimpleDefaultSerializationKind.Sbyte => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.SByteSize",
            SimpleDefaultSerializationKind.Bool => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.BoolSize",
            SimpleDefaultSerializationKind.Char => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.CharSize",
            SimpleDefaultSerializationKind.Int => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize",
            SimpleDefaultSerializationKind.Long => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.LongSize",
            SimpleDefaultSerializationKind.Short => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.ShortSize",
            SimpleDefaultSerializationKind.Uint => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.UIntSize",
            SimpleDefaultSerializationKind.Ulong => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.ULongSize",
            SimpleDefaultSerializationKind.Ushort => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.UShortSize",
            SimpleDefaultSerializationKind.Float => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.FloatSize",
            SimpleDefaultSerializationKind.Double => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.DoubleSize",
            SimpleDefaultSerializationKind.Decimal => "MsbRpc.Serialization.Primitives.PrimitiveSerializer.DecimalSize",
            SimpleDefaultSerializationKind.String => "MsbRpc.Serialization.StringSerializer.GetSize(target)",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    private static string GetExpectedSerializationStatement(SimpleDefaultSerializationKind target)
        => target switch
        {
            SimpleDefaultSerializationKind.Byte => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Sbyte => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Bool => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Char => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Int => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Long => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Short => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Uint => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Ulong => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Ushort => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Float => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Double => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.Decimal => "bufferWriter.Write(value)",
            SimpleDefaultSerializationKind.String => "bufferWriter.Write(value)",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    private static string GetExpectedDeserializationExpression(SimpleDefaultSerializationKind target)
        => target switch
        {
            SimpleDefaultSerializationKind.Byte => "bufferReader.ReadByte()",
            SimpleDefaultSerializationKind.Sbyte => "bufferReader.ReadSByte()",
            SimpleDefaultSerializationKind.Bool => "bufferReader.ReadBool()",
            SimpleDefaultSerializationKind.Char => "bufferReader.ReadChar()",
            SimpleDefaultSerializationKind.Int => "bufferReader.ReadInt()",
            SimpleDefaultSerializationKind.Long => "bufferReader.ReadLong()",
            SimpleDefaultSerializationKind.Short => "bufferReader.ReadShort()",
            SimpleDefaultSerializationKind.Uint => "bufferReader.ReadUInt()",
            SimpleDefaultSerializationKind.Ulong => "bufferReader.ReadULong()",
            SimpleDefaultSerializationKind.Ushort => "bufferReader.ReadUShort()",
            SimpleDefaultSerializationKind.Float => "bufferReader.ReadFloat()",
            SimpleDefaultSerializationKind.Double => "bufferReader.ReadDouble()",
            SimpleDefaultSerializationKind.Decimal => "bufferReader.ReadDecimal()",
            SimpleDefaultSerializationKind.String => "bufferReader.ReadString()",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    private void RunSerializationTest(SimpleDefaultSerializationKind target)
    {
        new SerializationTest(target.GetTargetType())
        {
            ExpectedDeclarationSyntax = GetExpectedDeclarationSyntax(target),
            ExpectedSizeExpression = GetExpectedSizeExpression(target),
            ExpectedSerializationStatement = GetExpectedSerializationStatement(target),
            ExpectedDeserializationExpression = GetExpectedDeserializationExpression(target)
        }.Run(TestContext);
    }

    private static SerializationResolver CreateResolver() => new(ImmutableArray<CustomSerializationInfo>.Empty);
}