using System.Collections.Generic;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Utility;

internal static class DefaultSerializationKindUtility
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

    private static readonly ReadOnlyDictionary<string, DefaultSerializationKind> ValuesByName;

    public static readonly DefaultSerializationKind[] All =
    {
        DefaultSerializationKind.Byte,
        DefaultSerializationKind.Sbyte,
        DefaultSerializationKind.Bool,
        DefaultSerializationKind.Char,
        DefaultSerializationKind.Int,
        DefaultSerializationKind.Long,
        DefaultSerializationKind.Short,
        DefaultSerializationKind.Uint,
        DefaultSerializationKind.Ulong,
        DefaultSerializationKind.Ushort,
        DefaultSerializationKind.Float,
        DefaultSerializationKind.Double,
        DefaultSerializationKind.Decimal,
        DefaultSerializationKind.String
    };

    static DefaultSerializationKindUtility()
    {
        Dictionary<string, DefaultSerializationKind> valuesByName = new(DictionaryCapacity);
        foreach (DefaultSerializationKind serializationKind in All)
        {
            valuesByName.Add(serializationKind.GetName(), serializationKind);
        }

        ValuesByName = new ReadOnlyDictionary<string, DefaultSerializationKind>(valuesByName);
    }

    public static bool TryGetByName(string name, out DefaultSerializationKind value) => ValuesByName.TryGetValue(name, out value);
}