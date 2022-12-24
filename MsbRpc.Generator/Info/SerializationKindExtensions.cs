using static MsbRpc.Generator.IndependentNames;


namespace MsbRpc.Generator.Info;

public static class SerializationKindExtensions
{
    public static string? GetPrimitiveTypeName(this SerializationKind target, string result)
    {
        return target switch
        {
            SerializationKind.Byte => SerializationTypeUtility.ByteTypeName,
            SerializationKind.Sbyte => SerializationTypeUtility.SbyteTypeName,
            SerializationKind.Bool => SerializationTypeUtility.BoolTypeName,
            SerializationKind.Char => SerializationTypeUtility.CharTypeName,
            SerializationKind.Int => SerializationTypeUtility.IntTypeName,
            SerializationKind.Long => SerializationTypeUtility.LongTypeName,
            SerializationKind.Short => SerializationTypeUtility.ShortTypeName,
            SerializationKind.Uint => SerializationTypeUtility.UintTypeName,
            SerializationKind.Ulong => SerializationTypeUtility.UlongTypeName,
            SerializationKind.Ushort => SerializationTypeUtility.UshortTypeName,
            SerializationKind.Float => SerializationTypeUtility.FloatTypeName,
            SerializationKind.Double => SerializationTypeUtility.DoubleTypeName,
            SerializationKind.Decimal => SerializationTypeUtility.DecimalTypeName,
            _ => null
        };
    }

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

    [Obsolete("Use GetIsPrimitive instead")]
    public static bool TryGetConstantSizeCode(this SerializationKind target, out string result)
    {
        result = target switch
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
            _ => string.Empty
        };
        return result != string.Empty;
    }
    
    public static string GetConstantSizeCode(this SerializationKind target)
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
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }

    public static bool GetIsConstantSize(this SerializationKind target)
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

    public static string GetBufferReadMethodName(this SerializationKind target)
    {
        return target switch
        {
            SerializationKind.Unresolved => throw new InvalidOperationException(),
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
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}