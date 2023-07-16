#region

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Serialization.Default;
using MsbRpc.Test.Generator.SerializationGeneration.Utility;

#endregion

namespace MsbRpc.Test.Generator.SerializationGeneration;

[TestClass]
[TestCategory(TestCategories.Serialization)]
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

    private static string GetExpectedFinalizedSerializationStatement(SimpleDefaultSerializationKind target)
        => target switch
        {
            SimpleDefaultSerializationKind.Byte => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Sbyte => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Bool => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Char => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Int => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Long => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Short => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Uint => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Ulong => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Ushort => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Float => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Double => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.Decimal => "bufferWriter.Write(value);\n",
            SimpleDefaultSerializationKind.String => "bufferWriter.Write(value);\n",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    private static string GetExpectedFinalizedDeserializationExpression(SimpleDefaultSerializationKind target)
        => target switch
        {
            SimpleDefaultSerializationKind.Byte => "bufferReader.ReadByte();\n",
            SimpleDefaultSerializationKind.Sbyte => "bufferReader.ReadSByte();\n",
            SimpleDefaultSerializationKind.Bool => "bufferReader.ReadBool();\n",
            SimpleDefaultSerializationKind.Char => "bufferReader.ReadChar();\n",
            SimpleDefaultSerializationKind.Int => "bufferReader.ReadInt();\n",
            SimpleDefaultSerializationKind.Long => "bufferReader.ReadLong();\n",
            SimpleDefaultSerializationKind.Short => "bufferReader.ReadShort();\n",
            SimpleDefaultSerializationKind.Uint => "bufferReader.ReadUInt();\n",
            SimpleDefaultSerializationKind.Ulong => "bufferReader.ReadULong();\n",
            SimpleDefaultSerializationKind.Ushort => "bufferReader.ReadUShort();\n",
            SimpleDefaultSerializationKind.Float => "bufferReader.ReadFloat();\n",
            SimpleDefaultSerializationKind.Double => "bufferReader.ReadDouble();\n",
            SimpleDefaultSerializationKind.Decimal => "bufferReader.ReadDecimal();\n",
            SimpleDefaultSerializationKind.String => "bufferReader.ReadString();\n",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    private void RunSerializationTest(SimpleDefaultSerializationKind target)
    {
        new SerializationTest(target.GetTargetType())
        {
            ExpectedDeclarationSyntax = GetExpectedDeclarationSyntax(target),
            ExpectedSizeExpression = GetExpectedSizeExpression(target),
            ExpectedSerializationStatement = GetExpectedFinalizedSerializationStatement(target),
            ExpectedDeserializationExpression = GetExpectedFinalizedDeserializationExpression(target)
        }.Run(TestContext);
    }

    private static SerializationResolver CreateResolver() => new();
}