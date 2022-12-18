namespace MsbRpc.Generator.Info;

public static class SerializationTypeExtensions
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
}