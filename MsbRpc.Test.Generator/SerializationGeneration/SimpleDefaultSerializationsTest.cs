using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Serialization.Default;

namespace MsbRpc.Test.Generator.SerializationGeneration;

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
        SimpleDefaultSerializationTest test = new(SimpleDefaultSerializationKind.Byte)
        {
            ExpectedSizeExpression = "MsbRpc.Serialization.Primitives.PrimitiveSerializer.ByteSize",
            ExpectedSerializationStatement = "bufferWriter.Write(value);",
            ExpectedDeserializationExpression = "bufferReader.ReadByte()",
            ExpectedDeclarationSyntax = "byte"
        };
        test.Run(CreateResolver());
    }

    [TestMethod]
    public void CreatesCorrectSByteSerialization()
    {
        SimpleDefaultSerializationTest test = new(SimpleDefaultSerializationKind.Sbyte)
        {
            ExpectedSizeExpression = "MsbRpc.Serialization.Primitives.PrimitiveSerializer.SByteSize",
            ExpectedSerializationStatement = "bufferWriter.Write(value);",
            ExpectedDeserializationExpression = "bufferReader.ReadSByte()",
            ExpectedDeclarationSyntax = "sbyte"
        };
        test.Run(CreateResolver());
    }

    [TestMethod]
    public void CreatesCorrectBoolSerialization()
    {
        SimpleDefaultSerializationTest test = new(SimpleDefaultSerializationKind.Bool)
        {
            ExpectedSizeExpression = "MsbRpc.Serialization.Primitives.PrimitiveSerializer.BoolSize",
            ExpectedSerializationStatement = "bufferWriter.Write(value);",
            ExpectedDeserializationExpression = "bufferReader.ReadBool()",
            ExpectedDeclarationSyntax = "bool"
        };
        test.Run(CreateResolver());
    }

    [TestMethod]
    public void CreatesCorrectCharSerialization()
    {
        SimpleDefaultSerializationTest test = new(SimpleDefaultSerializationKind.Char)
        {
            ExpectedSizeExpression = "MsbRpc.Serialization.Primitives.PrimitiveSerializer.CharSize",
            ExpectedSerializationStatement = "bufferWriter.Write(value);",
            ExpectedDeserializationExpression = "bufferReader.ReadChar()",
            ExpectedDeclarationSyntax = "char"
        };
        test.Run(CreateResolver());
    }

    [TestMethod]
    public void CreatesCorrectIntSerialization()
    {
        SimpleDefaultSerializationTest test = new(SimpleDefaultSerializationKind.Int)
        {
            ExpectedSizeExpression = "MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize",
            ExpectedSerializationStatement = "bufferWriter.Write(value);",
            ExpectedDeserializationExpression = "bufferReader.ReadInt()",
            ExpectedDeclarationSyntax = "int"
        };
        test.Run(CreateResolver());
    }

    //todo: implement integral values serialization test once copilot starts working normally again
    [TestMethod]
    public void CreatesCorrectLongSerialization() { }

    [TestMethod]
    public void CreatesCorrectFloatSerialization()
    {
        SimpleDefaultSerializationTest test = new(SimpleDefaultSerializationKind.Float)
        {
            ExpectedSizeExpression = "MsbRpc.Serialization.Primitives.PrimitiveSerializer.FloatSize",
            ExpectedSerializationStatement = "bufferWriter.Write(value);",
            ExpectedDeserializationExpression = "bufferReader.ReadFloat()",
            ExpectedDeclarationSyntax = "float"
        };
    }

    [TestMethod]
    public void CreatesCorrectDoubleSerialization()
    {
        SimpleDefaultSerializationTest test = new(SimpleDefaultSerializationKind.Double)
        {
            ExpectedSizeExpression = "MsbRpc.Serialization.Primitives.PrimitiveSerializer.DoubleSize",
            ExpectedSerializationStatement = "bufferWriter.Write(value);",
            ExpectedDeserializationExpression = "bufferReader.ReadDouble()",
            ExpectedDeclarationSyntax = "double"
        };
        test.Run(CreateResolver());
    }

    [TestMethod]
    public void CreatesCorrectDecimalSerialization()
    {
        SimpleDefaultSerializationTest test = new(SimpleDefaultSerializationKind.Decimal)
        {
            ExpectedSizeExpression = "MsbRpc.Serialization.Primitives.PrimitiveSerializer.DecimalSize",
            ExpectedSerializationStatement = "bufferWriter.Write(value);",
            ExpectedDeserializationExpression = "bufferReader.ReadDecimal()",
            ExpectedDeclarationSyntax = "decimal"
        };
        test.Run(CreateResolver());
    }

    [TestMethod]
    public void CreatesCorrectStringSerialization()
    {
        SimpleDefaultSerializationTest test = new(SimpleDefaultSerializationKind.String)
        {
            ExpectedSizeExpression = "MsbRpc.Serialization.StringSerializer.GetSize(target)",
            ExpectedSerializationStatement = "bufferWriter.Write(value);",
            ExpectedDeserializationExpression = "bufferReader.ReadString()",
            ExpectedDeclarationSyntax = "string"
        };
        test.Run(CreateResolver());
    }

    private static SerializationResolver CreateResolver() => new(ImmutableArray<CustomSerializationInfo>.Empty);
}