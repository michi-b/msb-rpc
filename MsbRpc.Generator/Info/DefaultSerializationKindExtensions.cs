using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Info;

internal static class DefaultSerializationKindExtensions
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

    private static readonly GenerationTree.Serialization.GetSerializationStatementDelegate DefaultSerializationTypeWriteStatement =
        (bufferWriterExpression, valueExpression) => $"{bufferWriterExpression}.{Methods.BufferWriterWrite}({valueExpression});";

    private static readonly ReadOnlyDictionary<DefaultSerializationKind, string> Names;

    static DefaultSerializationKindExtensions()
    {
        Dictionary<DefaultSerializationKind, string> names = new(DefaultSerializationKindUtility.DictionaryCapacity);
        foreach (DefaultSerializationKind serializationKind in DefaultSerializationKindUtility.All)
        {
            names.Add
            (
                serializationKind,
                serializationKind switch
                {
                    DefaultSerializationKind.Byte => DefaultSerializationKindUtility.ByteTypeName,
                    DefaultSerializationKind.Sbyte => DefaultSerializationKindUtility.SbyteTypeName,
                    DefaultSerializationKind.Bool => DefaultSerializationKindUtility.BoolTypeName,
                    DefaultSerializationKind.Char => DefaultSerializationKindUtility.CharTypeName,
                    DefaultSerializationKind.Int => DefaultSerializationKindUtility.IntTypeName,
                    DefaultSerializationKind.Long => DefaultSerializationKindUtility.LongTypeName,
                    DefaultSerializationKind.Short => DefaultSerializationKindUtility.ShortTypeName,
                    DefaultSerializationKind.Uint => DefaultSerializationKindUtility.UintTypeName,
                    DefaultSerializationKind.Ulong => DefaultSerializationKindUtility.UlongTypeName,
                    DefaultSerializationKind.Ushort => DefaultSerializationKindUtility.UshortTypeName,
                    DefaultSerializationKind.Float => DefaultSerializationKindUtility.FloatTypeName,
                    DefaultSerializationKind.Double => DefaultSerializationKindUtility.DoubleTypeName,
                    DefaultSerializationKind.Decimal => DefaultSerializationKindUtility.DecimalTypeName,
                    DefaultSerializationKind.String => DefaultSerializationKindUtility.StringTypeName,
                    _ => throw new ArgumentOutOfRangeException()
                }
            );
        }

        Names = new ReadOnlyDictionary<DefaultSerializationKind, string>(names);
    }

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
            DefaultSerializationKind.String => "string",
            _ => throw new ArgumentOutOfRangeException(nameof(serializationKind), serializationKind, null)
        };
        return true;
    }

    public static GenerationTree.Serialization.GetSizeExpressionDelegate GetSizeExpressionStringFactory(this DefaultSerializationKind target)
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
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static GenerationTree.Serialization.GetSerializationStatementDelegate GetSerializationStatementStringFactory(this DefaultSerializationKind target)
    {
        return target switch
        {
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

    public static GenerationTree.Serialization.GetDeserializationExpressionDelegate GetDeserializationExpressionStringFactory(this DefaultSerializationKind target)
    {
        return target switch
        {
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

    public static string GetName(this DefaultSerializationKind defaultSerializationKind) => Names[defaultSerializationKind];
}