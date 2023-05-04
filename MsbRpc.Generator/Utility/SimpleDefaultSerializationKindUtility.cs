using System.Collections.Generic;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Utility;

public static class SimpleDefaultSerializationKindUtility
{
    public const int DictionaryCapacity = 100;

    public const string FloatTypeName = "System.Single";
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
    public const string DoubleTypeName = "System.Double";
    public const string DecimalTypeName = "System.Decimal";
    public const string StringTypeName = "System.String";

    private static readonly ReadOnlyDictionary<TypeInfo, SimpleDefaultSerializationKind> Values;

    public static readonly SimpleDefaultSerializationKind[] All =
    {
        SimpleDefaultSerializationKind.Byte,
        SimpleDefaultSerializationKind.Sbyte,
        SimpleDefaultSerializationKind.Bool,
        SimpleDefaultSerializationKind.Char,
        SimpleDefaultSerializationKind.Int,
        SimpleDefaultSerializationKind.Long,
        SimpleDefaultSerializationKind.Short,
        SimpleDefaultSerializationKind.Uint,
        SimpleDefaultSerializationKind.Ulong,
        SimpleDefaultSerializationKind.Ushort,
        SimpleDefaultSerializationKind.Float,
        SimpleDefaultSerializationKind.Double,
        SimpleDefaultSerializationKind.Decimal,
        SimpleDefaultSerializationKind.String
    };

    static SimpleDefaultSerializationKindUtility()
    {
        Dictionary<TypeInfo, SimpleDefaultSerializationKind> values = new(DictionaryCapacity);
        foreach (SimpleDefaultSerializationKind serializationKind in All)
        {
            values.Add(serializationKind.GetTypeInfo(), serializationKind);
        }

        Values = new ReadOnlyDictionary<TypeInfo, SimpleDefaultSerializationKind>(values);
    }

    public static bool TryGet(TypeInfo typeInfo, out SimpleDefaultSerializationKind value) => Values.TryGetValue(typeInfo, out value);
}