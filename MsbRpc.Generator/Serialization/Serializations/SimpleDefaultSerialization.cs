using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.Serialization.Default;
using static MsbRpc.Generator.Utility.IndependentNames;

namespace MsbRpc.Generator.Serialization.Serializations;

public sealed class SimpleDefaultSerialization : ISerialization
{
    private const string ReadByte = "." + Methods.BufferReaderReadByte + "()";
    private const string ReadSByte = "." + Methods.BufferReaderReadSByte + "()";
    private const string ReadBool = "." + Methods.BufferReaderReadBool + "()";
    private const string ReadChar = "." + Methods.BufferReaderReadChar + "()";
    private const string ReadInt = "." + Methods.BufferReaderReadInt + "()";
    private const string ReadLong = "." + Methods.BufferReaderReadLong + "()";
    private const string ReadShort = "." + Methods.BufferReaderReadShort + "()";
    private const string ReadUInt = "." + Methods.BufferReaderReadUInt + "()";
    private const string ReadULong = "." + Methods.BufferReaderReadULong + "()";
    private const string ReadUShort = "." + Methods.BufferReaderReadUShort + "()";
    private const string ReadFloat = "." + Methods.BufferReaderReadFloat + "()";
    private const string ReadDouble = "." + Methods.BufferReaderReadDouble + "()";
    private const string ReadDecimal = "." + Methods.BufferReaderReadDecimal + "()";

    private const string ReadString = "." + Methods.BufferReaderReadString + "()";

    private static readonly Func<string, string, string> DefaultSerializationTypeWriteStatement =
        (bufferWriterExpression, valueExpression) => $"{bufferWriterExpression}.{Methods.BufferWriterWrite}({valueExpression})";

    private readonly SimpleDefaultSerializationKind _serializationKind;

    public bool CanUseNullableAnnotationInsteadOfWrapper
        => _serializationKind switch
        {
            SimpleDefaultSerializationKind.Byte => true,
            SimpleDefaultSerializationKind.Sbyte => true,
            SimpleDefaultSerializationKind.Bool => true,
            SimpleDefaultSerializationKind.Char => true,
            SimpleDefaultSerializationKind.Int => true,
            SimpleDefaultSerializationKind.Long => true,
            SimpleDefaultSerializationKind.Short => true,
            SimpleDefaultSerializationKind.Uint => true,
            SimpleDefaultSerializationKind.Ulong => true,
            SimpleDefaultSerializationKind.Ushort => true,
            SimpleDefaultSerializationKind.Float => true,
            SimpleDefaultSerializationKind.Double => true,
            SimpleDefaultSerializationKind.Decimal => true,
            SimpleDefaultSerializationKind.String => false,
            _ => throw new ArgumentOutOfRangeException()
        };

    public SimpleDefaultSerialization(SimpleDefaultSerializationKind serializationKind) => _serializationKind = serializationKind;

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        string sizeExpression = _serializationKind switch
        {
            SimpleDefaultSerializationKind.Byte => GlobalConstants.ByteSize,
            SimpleDefaultSerializationKind.Sbyte => GlobalConstants.SByteSize,
            SimpleDefaultSerializationKind.Bool => GlobalConstants.BoolSize,
            SimpleDefaultSerializationKind.Char => GlobalConstants.CharSize,
            SimpleDefaultSerializationKind.Int => GlobalConstants.IntSize,
            SimpleDefaultSerializationKind.Long => GlobalConstants.LongSize,
            SimpleDefaultSerializationKind.Short => GlobalConstants.ShortSize,
            SimpleDefaultSerializationKind.Uint => GlobalConstants.UIntSize,
            SimpleDefaultSerializationKind.Ulong => GlobalConstants.ULongSize,
            SimpleDefaultSerializationKind.Ushort => GlobalConstants.UShortSize,
            SimpleDefaultSerializationKind.Float => GlobalConstants.FloatSize,
            SimpleDefaultSerializationKind.Double => GlobalConstants.DoubleSize,
            SimpleDefaultSerializationKind.Decimal => GlobalConstants.DecimalSize,
            SimpleDefaultSerializationKind.String => $"{Types.StringSerializer}.GetSize({targetExpression})",
            _ => throw new ArgumentOutOfRangeException()
        };
        writer.Write(sizeExpression);
    }

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        writer.Write
        (
            _serializationKind switch
            {
                SimpleDefaultSerializationKind.Byte => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Sbyte => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Bool => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Char => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Int => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Long => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Short => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Uint => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Ulong => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Ushort => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Float => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Double => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.Decimal => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                SimpleDefaultSerializationKind.String => DefaultSerializationTypeWriteStatement(bufferWriterExpression, valueExpression),
                _ => throw new ArgumentOutOfRangeException()
            }
        );
    }

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.Write
        (
            _serializationKind switch
            {
                SimpleDefaultSerializationKind.Byte => bufferReaderExpression + ReadByte,
                SimpleDefaultSerializationKind.Sbyte => bufferReaderExpression + ReadSByte,
                SimpleDefaultSerializationKind.Bool => bufferReaderExpression + ReadBool,
                SimpleDefaultSerializationKind.Char => bufferReaderExpression + ReadChar,
                SimpleDefaultSerializationKind.Int => bufferReaderExpression + ReadInt,
                SimpleDefaultSerializationKind.Long => bufferReaderExpression + ReadLong,
                SimpleDefaultSerializationKind.Short => bufferReaderExpression + ReadShort,
                SimpleDefaultSerializationKind.Uint => bufferReaderExpression + ReadUInt,
                SimpleDefaultSerializationKind.Ulong => bufferReaderExpression + ReadULong,
                SimpleDefaultSerializationKind.Ushort => bufferReaderExpression + ReadUShort,
                SimpleDefaultSerializationKind.Float => bufferReaderExpression + ReadFloat,
                SimpleDefaultSerializationKind.Double => bufferReaderExpression + ReadDouble,
                SimpleDefaultSerializationKind.Decimal => bufferReaderExpression + ReadDecimal,
                SimpleDefaultSerializationKind.String => bufferReaderExpression + ReadString,
                _ => throw new ArgumentOutOfRangeException()
            }
        );
    }

    public bool GetIsVoid() => false;

    public bool GetIsResolved() => true;

    public string GetDeclarationSyntax()
        => _serializationKind switch
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
            _ => throw new ArgumentOutOfRangeException()
        };
}