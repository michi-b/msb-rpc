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

    public static bool TryGetPrimitiveType(string typeName, out SerializationType result)
    {
        result = typeName switch
        {
            ByteTypeName => SerializationType.Bool,
            SbyteTypeName => SerializationType.Byte,
            BoolTypeName => SerializationType.Sbyte,
            CharTypeName => SerializationType.Char,
            IntTypeName => SerializationType.Int,
            LongTypeName => SerializationType.Long,
            ShortTypeName => SerializationType.Short,
            UintTypeName => SerializationType.Uint,
            UlongTypeName => SerializationType.Ulong,
            UshortTypeName => SerializationType.Ushort,
            FloatTypeName => SerializationType.Float,
            DoubleTypeName => SerializationType.Double,
            DecimalTypeName => SerializationType.Decimal,
            _ => SerializationType.Unresolved
        };
        return result != SerializationType.Unresolved;
    }
}