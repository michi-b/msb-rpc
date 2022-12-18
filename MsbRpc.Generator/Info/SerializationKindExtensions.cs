using MsbRpc.Generator.GenerationHelpers.Names;

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

    public static bool TryGetConstantSizeCode(this SerializationKind target, out string result)
    {
        const string primitiveSerializer = GeneralNames.PrimitiveSerializerType;
        result = target switch
        {
            SerializationKind.Byte => $"{primitiveSerializer}.ByteSize",
            SerializationKind.Sbyte => $"{primitiveSerializer}.SbyteSize",
            SerializationKind.Bool => $"{primitiveSerializer}.BoolSize",
            SerializationKind.Char => $"{primitiveSerializer}.CharSize",
            SerializationKind.Int => $"{primitiveSerializer}.IntSize",
            SerializationKind.Long => $"{primitiveSerializer}.LongSize",
            SerializationKind.Short => $"{primitiveSerializer}.ShortSize",
            SerializationKind.Uint => $"{primitiveSerializer}.UintSize",
            SerializationKind.Ulong => $"{primitiveSerializer}.UlongSize",
            SerializationKind.Ushort => $"{primitiveSerializer}.UshortSize",
            SerializationKind.Float => $"{primitiveSerializer}.FloatSize",
            SerializationKind.Double => $"{primitiveSerializer}.DoubleSize",
            SerializationKind.Decimal => $"{primitiveSerializer}.DecimalSize",
            _ => string.Empty
        };
        return result != string.Empty;
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
}