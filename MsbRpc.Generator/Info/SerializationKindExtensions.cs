using System;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Info;

internal static class SerializationKindExtensions
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

    private static readonly SerializationNode.GetSerializationStatementDelegate DefaultSerializationTypeWriteStatement = (bufferWriterExpression, valueExpression)
        => $"{bufferWriterExpression}.{Methods.BufferWriterWrite}({valueExpression});";

    public static bool TryGetKeyword(this DefaultSerializationKind serializationKind, out string keyword)
    {
        keyword = serializationKind switch
        {
            DefaultSerializationKind.Byte => "byte",
            DefaultSerializationKind.Sbyte => "sbyte",
            DefaultSerializationKind.Bool => "bool",
            DefaultSerializationKind.Char => "char",
            DefaultSerializationKind.Int => "int",
            DefaultSerializationKind.Long => "long",
            DefaultSerializationKind.Short => "short",
            DefaultSerializationKind.Uint => "uint",
            DefaultSerializationKind.Ulong => "ulong",
            DefaultSerializationKind.Ushort => "ushort",
            DefaultSerializationKind.Float => "float",
            DefaultSerializationKind.Double => "double",
            DefaultSerializationKind.Decimal => "decimal",
            DefaultSerializationKind.Void => "void",
            DefaultSerializationKind.String => "string",
            _ => throw new ArgumentOutOfRangeException(nameof(serializationKind), serializationKind, null)
        };
        return true;
    }

    public static SerializationNode.GetSizeExpressionDelegate? GetGetSizeExpression(this DefaultSerializationKind target)
    {
        return target switch
        {
            DefaultSerializationKind.Byte => _ => GlobalConstants.ByteSize,
            DefaultSerializationKind.Sbyte => _ => GlobalConstants.SByteSize,
            DefaultSerializationKind.Bool => _ => GlobalConstants.BoolSize,
            DefaultSerializationKind.Char => _ => GlobalConstants.CharSize,
            DefaultSerializationKind.Int => _ => GlobalConstants.IntSize,
            DefaultSerializationKind.Long => _ => GlobalConstants.LongSize,
            DefaultSerializationKind.Short => _ => GlobalConstants.ShortSize,
            DefaultSerializationKind.Uint => _ => GlobalConstants.UIntSize,
            DefaultSerializationKind.Ulong => _ => GlobalConstants.ULongSize,
            DefaultSerializationKind.Ushort => _ => GlobalConstants.UShortSize,
            DefaultSerializationKind.Float => _ => GlobalConstants.FloatSize,
            DefaultSerializationKind.Double => _ => GlobalConstants.DoubleSize,
            DefaultSerializationKind.Decimal => _ => GlobalConstants.DecimalSize,
            DefaultSerializationKind.String => targetExpression => $"{Types.StringSerializer}.GetSize({targetExpression})",
            DefaultSerializationKind.Void => null,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static SerializationNode.GetSerializationStatementDelegate? GetGetSerializationStatement(this DefaultSerializationKind target)
    {
        return target switch
        {
            DefaultSerializationKind.Void => null,
            DefaultSerializationKind.Byte => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Sbyte => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Bool => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Char => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Int => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Long => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Short => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Uint => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Ulong => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Ushort => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Float => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Double => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.Decimal => DefaultSerializationTypeWriteStatement,
            DefaultSerializationKind.String => DefaultSerializationTypeWriteStatement,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static SerializationNode.GetDeserializationExpressionDelegate? GetGetDeserializationExpression(this DefaultSerializationKind target)
    {
        return target switch
        {
            DefaultSerializationKind.Void => null,
            DefaultSerializationKind.Byte => bufferReaderExpression => bufferReaderExpression + ReadByte,
            DefaultSerializationKind.Sbyte => bufferReaderExpression => bufferReaderExpression + ReadSByte,
            DefaultSerializationKind.Bool => bufferReaderExpression => bufferReaderExpression + ReadBool,
            DefaultSerializationKind.Char => bufferReaderExpression => bufferReaderExpression + ReadChar,
            DefaultSerializationKind.Int => bufferReaderExpression => bufferReaderExpression + ReadInt,
            DefaultSerializationKind.Long => bufferReaderExpression => bufferReaderExpression + ReadLong,
            DefaultSerializationKind.Short => bufferReaderExpression => bufferReaderExpression + ReadShort,
            DefaultSerializationKind.Uint => bufferReaderExpression => bufferReaderExpression + ReadUInt,
            DefaultSerializationKind.Ulong => bufferReaderExpression => bufferReaderExpression + ReadULong,
            DefaultSerializationKind.Ushort => bufferReaderExpression => bufferReaderExpression + ReadUShort,
            DefaultSerializationKind.Float => bufferReaderExpression => bufferReaderExpression + ReadFloat,
            DefaultSerializationKind.Double => bufferReaderExpression => bufferReaderExpression + ReadDouble,
            DefaultSerializationKind.Decimal => bufferReaderExpression => bufferReaderExpression + ReadDecimal,
            DefaultSerializationKind.String => bufferReaderExpression => bufferReaderExpression + ReadString,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}