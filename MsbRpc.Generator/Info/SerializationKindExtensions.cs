using System;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.Info;

internal static class SerializationKindExtensions
{
    public static string? GetKeyword(this SerializationKind serializationKind)
    {
        return serializationKind switch
        {
            SerializationKind.Byte => "byte",
            SerializationKind.Sbyte => "sbyte",
            SerializationKind.Bool => "bool",
            SerializationKind.Char => "char",
            SerializationKind.Int => "int",
            SerializationKind.Long => "long",
            SerializationKind.Short => "short",
            SerializationKind.Uint => "uint",
            SerializationKind.Ulong => "ulong",
            SerializationKind.Ushort => "ushort",
            SerializationKind.Float => "float",
            SerializationKind.Double => "double",
            SerializationKind.Decimal => "decimal",
            SerializationKind.Void => "void",
            _ => null
        };
    }

    public static bool GetIsValidParameterType(this SerializationKind serializationKind)
    {
        return serializationKind switch
        {
            SerializationKind.Byte => true,
            SerializationKind.Sbyte => true,
            SerializationKind.Bool => true,
            SerializationKind.Char => true,
            SerializationKind.Int => true,
            SerializationKind.Long => true,
            SerializationKind.Short => true,
            SerializationKind.Uint => true,
            SerializationKind.Ulong => true,
            SerializationKind.Ushort => true,
            SerializationKind.Float => true,
            SerializationKind.Double => true,
            SerializationKind.Decimal => true,
            SerializationKind.Void => false,
            _ => false
        };
    }

    public static bool GetIsValidReturnType(this SerializationKind serializationKind)
    {
        return serializationKind switch
        {
            SerializationKind.Byte => true,
            SerializationKind.Sbyte => true,
            SerializationKind.Bool => true,
            SerializationKind.Char => true,
            SerializationKind.Int => true,
            SerializationKind.Long => true,
            SerializationKind.Short => true,
            SerializationKind.Uint => true,
            SerializationKind.Ulong => true,
            SerializationKind.Ushort => true,
            SerializationKind.Float => true,
            SerializationKind.Double => true,
            SerializationKind.Decimal => true,
            SerializationKind.Void => true,
            _ => false
        };
    }

    public static string? GetConstantSizeExpression(this SerializationKind target)
    {
        return target switch
        {
            SerializationKind.Byte => $"{Types.PrimitiveSerializer}.ByteSize",
            SerializationKind.Sbyte => $"{Types.PrimitiveSerializer}.SbyteSize",
            SerializationKind.Bool => $"{Types.PrimitiveSerializer}.BoolSize",
            SerializationKind.Char => $"{Types.PrimitiveSerializer}.CharSize",
            SerializationKind.Int => $"{Types.PrimitiveSerializer}.IntSize",
            SerializationKind.Long => $"{Types.PrimitiveSerializer}.LongSize",
            SerializationKind.Short => $"{Types.PrimitiveSerializer}.ShortSize",
            SerializationKind.Uint => $"{Types.PrimitiveSerializer}.UintSize",
            SerializationKind.Ulong => $"{Types.PrimitiveSerializer}.UlongSize",
            SerializationKind.Ushort => $"{Types.PrimitiveSerializer}.UshortSize",
            SerializationKind.Float => $"{Types.PrimitiveSerializer}.FloatSize",
            SerializationKind.Double => $"{Types.PrimitiveSerializer}.DoubleSize",
            SerializationKind.Decimal => $"{Types.PrimitiveSerializer}.DecimalSize",
            _ => null
        };
    }

    public static string? GetBufferReadMethodName(this SerializationKind target)
    {
        return target switch
        {
            SerializationKind.Byte => "ReadByte",
            SerializationKind.Sbyte => "ReadSbyte",
            SerializationKind.Bool => "ReadBool",
            SerializationKind.Char => "ReadChar",
            SerializationKind.Int => "ReadInt",
            SerializationKind.Long => "ReadLong",
            SerializationKind.Short => "ReadShort",
            SerializationKind.Uint => "ReadUint",
            SerializationKind.Ulong => "ReadUlong",
            SerializationKind.Ushort => "ReadUshort",
            SerializationKind.Float => "ReadFloat",
            SerializationKind.Double => "ReadDouble",
            SerializationKind.Decimal => "ReadDecimal",
            _ => null
        };
    }

    public static string GetName(this SerializationKind target)
    {
        return target switch
        {
            SerializationKind.Unresolved => nameof(SerializationKind.Unresolved),
            SerializationKind.Byte => nameof(SerializationKind.Byte),
            SerializationKind.Sbyte => nameof(SerializationKind.Sbyte),
            SerializationKind.Bool => nameof(SerializationKind.Bool),
            SerializationKind.Char => nameof(SerializationKind.Char),
            SerializationKind.Int => nameof(SerializationKind.Int),
            SerializationKind.Long => nameof(SerializationKind.Long),
            SerializationKind.Short => nameof(SerializationKind.Short),
            SerializationKind.Uint => nameof(SerializationKind.Uint),
            SerializationKind.Ulong => nameof(SerializationKind.Ulong),
            SerializationKind.Ushort => nameof(SerializationKind.Ushort),
            SerializationKind.Float => nameof(SerializationKind.Float),
            SerializationKind.Double => nameof(SerializationKind.Double),
            SerializationKind.Decimal => nameof(SerializationKind.Decimal),
            SerializationKind.Void => nameof(SerializationKind.Void),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}