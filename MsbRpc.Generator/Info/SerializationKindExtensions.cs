using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.Info;

internal static class SerializationKindExtensions
{
    public static bool GetIsPrimitive(this SerializationKind target)
    {
        return target switch
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

    public static bool HasKeyword(this SerializationKind serializationKind) => serializationKind.GetIsPrimitive();

    public static bool GetKeyword(this SerializationKind serializationKind, out string? keyword)
    {
        if (serializationKind.GetIsPrimitive())
        {
            keyword = serializationKind switch
            {
                SerializationKind.Byte => SerializationKindUtility.ByteKeyword,
                SerializationKind.Sbyte => SerializationKindUtility.SbyteKeyword,
                SerializationKind.Bool => SerializationKindUtility.BoolKeyword,
                SerializationKind.Char => SerializationKindUtility.CharKeyword,
                SerializationKind.Int => SerializationKindUtility.IntKeyword,
                SerializationKind.Long => SerializationKindUtility.LongKeyword,
                SerializationKind.Short => SerializationKindUtility.ShortKeyword,
                SerializationKind.Uint => SerializationKindUtility.UintKeyword,
                SerializationKind.Ulong => SerializationKindUtility.UlongKeyword,
                SerializationKind.Ushort => SerializationKindUtility.UshortKeyword,
                SerializationKind.Float => SerializationKindUtility.FloatKeyword,
                SerializationKind.Double => SerializationKindUtility.DoubleKeyword,
                SerializationKind.Decimal => SerializationKindUtility.DecimalKeyword,
                _ => throw new ArgumentOutOfRangeException(nameof(serializationKind), serializationKind, null)
            };
            return true;
        }

        keyword = null;
        return false;
    }
}