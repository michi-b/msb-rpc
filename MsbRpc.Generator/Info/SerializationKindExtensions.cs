using System;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Info;

internal static class SerializationKindExtensions
{
    private static readonly SerializationNode.GetSerializationStatementDelegate DefaultSerializationTypeWriteStatement = (bufferWriterExpression, valueExpression)
        => $"{bufferWriterExpression}.{Methods.BufferWriterWrite}({valueExpression});";

    public static bool TryGetKeyword(this DefaultSerializationKind serializationKind, out string keyword)
    {
        if (serializationKind == DefaultSerializationKind.Unresolved)
        {
            keyword = null!;
            return false;
        }

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
            DefaultSerializationKind.Unresolved => throw new ArgumentOutOfRangeException(nameof(serializationKind), serializationKind, null),
            _ => throw new ArgumentOutOfRangeException(nameof(serializationKind), serializationKind, null)
        };
        return true;
    }

    public static SerializationNode.GetSizeExpressionDelegate? GetGetSizeExpression(this DefaultSerializationKind target)
    {
        return target switch
        {
            DefaultSerializationKind.Unresolved => null,
            DefaultSerializationKind.Byte => _ => "PrimitiveSerializer.ByteSize",
            DefaultSerializationKind.Sbyte => _ => "PrimitiveSerializer.SByteSize",
            DefaultSerializationKind.Bool => _ => "PrimitiveSerializer.BoolSize",
            DefaultSerializationKind.Char => _ => "PrimitiveSerializer.CharSize",
            DefaultSerializationKind.Int => _ => "PrimitiveSerializer.IntSize",
            DefaultSerializationKind.Long => _ => "PrimitiveSerializer.LongSize",
            DefaultSerializationKind.Short => _ => "PrimitiveSerializer.ShortSize",
            DefaultSerializationKind.Uint => _ => "PrimitiveSerializer.UIntSize",
            DefaultSerializationKind.Ulong => _ => "PrimitiveSerializer.ULongSize",
            DefaultSerializationKind.Ushort => _ => "PrimitiveSerializer.UShortSize",
            DefaultSerializationKind.Float => _ => "PrimitiveSerializer.FloatSize",
            DefaultSerializationKind.Double => _ => "PrimitiveSerializer.DoubleSize",
            DefaultSerializationKind.Decimal => _ => "PrimitiveSerializer.DecimalSize",
            DefaultSerializationKind.String => targetExpression => $"{Types.StringSerializer}.GetSize({targetExpression})",
            DefaultSerializationKind.Void => null,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static SerializationNode.GetSerializationStatementDelegate? GetGetSerializationStatement(this DefaultSerializationKind target)
    {
        return target switch
        {
            DefaultSerializationKind.Unresolved => null,
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
            DefaultSerializationKind.Unresolved => null,
            DefaultSerializationKind.Byte => bufferReaderExpression => $"{bufferReaderExpression}.ReadByte()",
            DefaultSerializationKind.Sbyte => bufferReaderExpression => $"{bufferReaderExpression}.ReadSByte()",
            DefaultSerializationKind.Bool => bufferReaderExpression => $"{bufferReaderExpression}.ReadBool()",
            DefaultSerializationKind.Char => bufferReaderExpression => $"{bufferReaderExpression}.ReadChar()",
            DefaultSerializationKind.Int => bufferReaderExpression => $"{bufferReaderExpression}.ReadInt()",
            DefaultSerializationKind.Long => bufferReaderExpression => $"{bufferReaderExpression}.ReadLong()",
            DefaultSerializationKind.Short => bufferReaderExpression => $"{bufferReaderExpression}.ReadShort()",
            DefaultSerializationKind.Uint => bufferReaderExpression => $"{bufferReaderExpression}.ReadUInt()",
            DefaultSerializationKind.Ulong => bufferReaderExpression => $"{bufferReaderExpression}.ReadULong()",
            DefaultSerializationKind.Ushort => bufferReaderExpression => $"{bufferReaderExpression}.ReadUShort()",
            DefaultSerializationKind.Float => bufferReaderExpression => $"{bufferReaderExpression}.ReadFloat()",
            DefaultSerializationKind.Double => bufferReaderExpression => $"{bufferReaderExpression}.ReadDouble()",
            DefaultSerializationKind.Decimal => bufferReaderExpression => $"{bufferReaderExpression}.ReadDecimal()",
            DefaultSerializationKind.String => bufferReaderExpression => $"{bufferReaderExpression}.ReadString()",
            DefaultSerializationKind.Void => null,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}