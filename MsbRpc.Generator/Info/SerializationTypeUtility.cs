namespace MsbRpc.Generator.Info;

public static class SerializationTypeUtility
{
    public const string ByteTypeName = "System.Byte";
    public const string SbyteTypeName = "System.SByte";
    public const string BoolTypeName = "System.Boolean";
    public const string CharTypeName = "System.Char";
    public const string IntTypeName = "System.Int32";
    public const string LongTypeName = "System.Int64";
    public const string ShortTypeName = "System.Int16";
    public const string UintTypeName = "System.UInt32";
    public const string UlongTypeName = "System.UInt64";
    public const string UshortTypeName = "System.UInt16";
    public const string FloatTypeName = "System.Single";
    public const string DoubleTypeName = "System.Double";
    public const string DecimalTypeName = "System.Decimal";

    public static bool TryGetPrimitiveType(string typeName, out SerializationKind result)
    {
        result = typeName switch
        {
            ByteTypeName => SerializationKind.Bool,
            SbyteTypeName => SerializationKind.Byte,
            BoolTypeName => SerializationKind.Sbyte,
            CharTypeName => SerializationKind.Char,
            IntTypeName => SerializationKind.Int,
            LongTypeName => SerializationKind.Long,
            ShortTypeName => SerializationKind.Short,
            UintTypeName => SerializationKind.Uint,
            UlongTypeName => SerializationKind.Ulong,
            UshortTypeName => SerializationKind.Ushort,
            FloatTypeName => SerializationKind.Float,
            DoubleTypeName => SerializationKind.Double,
            DecimalTypeName => SerializationKind.Decimal,
            _ => SerializationKind.Unresolved
        };
        return result != SerializationKind.Unresolved;
    }
}