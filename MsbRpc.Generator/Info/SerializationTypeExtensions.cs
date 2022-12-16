namespace MsbRpc.Generator.Info;

public static class SerializationTypeExtensions
{
    public static string? GetPrimitiveTypeName(this SerializationType target, string result)
    {
        return target switch
        {
            SerializationType.Byte => SerializationTypeUtility.ByteTypeName,
            SerializationType.Sbyte => SerializationTypeUtility.SbyteTypeName,
            SerializationType.Bool => SerializationTypeUtility.BoolTypeName,
            SerializationType.Char => SerializationTypeUtility.CharTypeName,
            SerializationType.Int => SerializationTypeUtility.IntTypeName,
            SerializationType.Long => SerializationTypeUtility.LongTypeName,
            SerializationType.Short => SerializationTypeUtility.ShortTypeName,
            SerializationType.Uint => SerializationTypeUtility.UintTypeName,
            SerializationType.Ulong => SerializationTypeUtility.UlongTypeName,
            SerializationType.Ushort => SerializationTypeUtility.UshortTypeName,
            SerializationType.Float => SerializationTypeUtility.FloatTypeName,
            SerializationType.Double => SerializationTypeUtility.DoubleTypeName,
            SerializationType.Decimal => SerializationTypeUtility.DecimalTypeName,
            _ => null
        };
    }

    public static bool GetIsPrimitiveType(this SerializationType target)
    {
        return target switch
        {
            SerializationType.Byte => true,
            SerializationType.Sbyte => true,
            SerializationType.Bool => true,
            SerializationType.Char => true,
            SerializationType.Int => true,
            SerializationType.Long => true,
            SerializationType.Short => true,
            SerializationType.Uint => true,
            SerializationType.Ulong => true,
            SerializationType.Ushort => true,
            SerializationType.Float => true,
            SerializationType.Double => true,
            SerializationType.Decimal => true,
            _ => false
        };
    }
}