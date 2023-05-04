using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Info;

internal static class SimpleDefaultSerializationKindExtensions
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

    private static readonly ReadOnlyDictionary<SimpleDefaultSerializationKind, string> Names;

    static SimpleDefaultSerializationKindExtensions()
    {
        Dictionary<SimpleDefaultSerializationKind, string> names = new(SimpleDefaultSerializationKindUtility.DictionaryCapacity);
        foreach (SimpleDefaultSerializationKind serializationKind in SimpleDefaultSerializationKindUtility.All)
        {
            names.Add
            (
                serializationKind,
                serializationKind switch
                {
                    SimpleDefaultSerializationKind.Byte => SimpleDefaultSerializationKindUtility.ByteTypeName,
                    SimpleDefaultSerializationKind.Sbyte => SimpleDefaultSerializationKindUtility.SbyteTypeName,
                    SimpleDefaultSerializationKind.Bool => SimpleDefaultSerializationKindUtility.BoolTypeName,
                    SimpleDefaultSerializationKind.Char => SimpleDefaultSerializationKindUtility.CharTypeName,
                    SimpleDefaultSerializationKind.Int => SimpleDefaultSerializationKindUtility.IntTypeName,
                    SimpleDefaultSerializationKind.Long => SimpleDefaultSerializationKindUtility.LongTypeName,
                    SimpleDefaultSerializationKind.Short => SimpleDefaultSerializationKindUtility.ShortTypeName,
                    SimpleDefaultSerializationKind.Uint => SimpleDefaultSerializationKindUtility.UintTypeName,
                    SimpleDefaultSerializationKind.Ulong => SimpleDefaultSerializationKindUtility.UlongTypeName,
                    SimpleDefaultSerializationKind.Ushort => SimpleDefaultSerializationKindUtility.UshortTypeName,
                    SimpleDefaultSerializationKind.Float => SimpleDefaultSerializationKindUtility.FloatTypeName,
                    SimpleDefaultSerializationKind.Double => SimpleDefaultSerializationKindUtility.DoubleTypeName,
                    SimpleDefaultSerializationKind.Decimal => SimpleDefaultSerializationKindUtility.DecimalTypeName,
                    SimpleDefaultSerializationKind.String => SimpleDefaultSerializationKindUtility.StringTypeName,
                    _ => throw new ArgumentOutOfRangeException()
                }
            );
        }

        Names = new ReadOnlyDictionary<SimpleDefaultSerializationKind, string>(names);
    }

    public static bool TryGetKeyword(this SimpleDefaultSerializationKind serializationKind, out string keyword)
    {
        keyword = serializationKind switch
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
            _ => throw new ArgumentOutOfRangeException(nameof(serializationKind), serializationKind, null)
        };
        return true;
    }

    public static GenerationTree.Serialization.GetSizeExpressionDelegate GetSizeExpressionStringFactory(this SimpleDefaultSerializationKind target)
    {
        return target switch
        {
            SimpleDefaultSerializationKind.Byte => _ => GlobalConstants.ByteSize,
            SimpleDefaultSerializationKind.Sbyte => _ => GlobalConstants.SByteSize,
            SimpleDefaultSerializationKind.Bool => _ => GlobalConstants.BoolSize,
            SimpleDefaultSerializationKind.Char => _ => GlobalConstants.CharSize,
            SimpleDefaultSerializationKind.Int => _ => GlobalConstants.IntSize,
            SimpleDefaultSerializationKind.Long => _ => GlobalConstants.LongSize,
            SimpleDefaultSerializationKind.Short => _ => GlobalConstants.ShortSize,
            SimpleDefaultSerializationKind.Uint => _ => GlobalConstants.UIntSize,
            SimpleDefaultSerializationKind.Ulong => _ => GlobalConstants.ULongSize,
            SimpleDefaultSerializationKind.Ushort => _ => GlobalConstants.UShortSize,
            SimpleDefaultSerializationKind.Float => _ => GlobalConstants.FloatSize,
            SimpleDefaultSerializationKind.Double => _ => GlobalConstants.DoubleSize,
            SimpleDefaultSerializationKind.Decimal => _ => GlobalConstants.DecimalSize,
            SimpleDefaultSerializationKind.String => targetExpression => $"{Types.StringSerializer}.GetSize({targetExpression})",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static GenerationTree.Serialization.GetSerializationStatementDelegate GetSerializationStatementStringFactory(this SimpleDefaultSerializationKind target)
    {
        return target switch
        {
            SimpleDefaultSerializationKind.Byte => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Sbyte => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Bool => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Char => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Int => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Long => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Short => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Uint => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Ulong => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Ushort => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Float => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Double => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.Decimal => DefaultSerializationTypeWriteStatement,
            SimpleDefaultSerializationKind.String => DefaultSerializationTypeWriteStatement,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static GenerationTree.Serialization.GetDeserializationExpressionDelegate GetDeserializationExpressionStringFactory(this SimpleDefaultSerializationKind target)
    {
        return target switch
        {
            SimpleDefaultSerializationKind.Byte => bufferReaderExpression => bufferReaderExpression + ReadByte,
            SimpleDefaultSerializationKind.Sbyte => bufferReaderExpression => bufferReaderExpression + ReadSByte,
            SimpleDefaultSerializationKind.Bool => bufferReaderExpression => bufferReaderExpression + ReadBool,
            SimpleDefaultSerializationKind.Char => bufferReaderExpression => bufferReaderExpression + ReadChar,
            SimpleDefaultSerializationKind.Int => bufferReaderExpression => bufferReaderExpression + ReadInt,
            SimpleDefaultSerializationKind.Long => bufferReaderExpression => bufferReaderExpression + ReadLong,
            SimpleDefaultSerializationKind.Short => bufferReaderExpression => bufferReaderExpression + ReadShort,
            SimpleDefaultSerializationKind.Uint => bufferReaderExpression => bufferReaderExpression + ReadUInt,
            SimpleDefaultSerializationKind.Ulong => bufferReaderExpression => bufferReaderExpression + ReadULong,
            SimpleDefaultSerializationKind.Ushort => bufferReaderExpression => bufferReaderExpression + ReadUShort,
            SimpleDefaultSerializationKind.Float => bufferReaderExpression => bufferReaderExpression + ReadFloat,
            SimpleDefaultSerializationKind.Double => bufferReaderExpression => bufferReaderExpression + ReadDouble,
            SimpleDefaultSerializationKind.Decimal => bufferReaderExpression => bufferReaderExpression + ReadDecimal,
            SimpleDefaultSerializationKind.String => bufferReaderExpression => bufferReaderExpression + ReadString,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static TypeInfo GetTypeInfo(this SimpleDefaultSerializationKind simpleDefaultSerializationKind)
        => TypeInfo.CreateSimple(Names[simpleDefaultSerializationKind]);
}