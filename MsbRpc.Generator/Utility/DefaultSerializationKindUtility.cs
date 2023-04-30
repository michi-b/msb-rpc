using System.Collections.Generic;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Utility;

internal static class DefaultSerializationKindUtility
{
    private const string FloatTypeName = "System.Single";
    private const string ByteTypeName = "System.Byte";
    private const string SbyteTypeName = "System.SByte";
    private const string BoolTypeName = "System.Boolean";
    private const string CharTypeName = "System.Char";
    private const string IntTypeName = "System.Int32";
    private const string LongTypeName = "System.Int64";
    private const string ShortTypeName = "System.Int16";
    private const string UintTypeName = "System.UInt32";
    private const string UlongTypeName = "System.UInt64";
    private const string UshortTypeName = "System.UInt16";
    private const string DoubleTypeName = "System.Double";
    private const string DecimalTypeName = "System.Decimal";
    private const string StringTypeName = "System.String";
    private const string VoidTypeName = "System.Void";

    public static readonly Dictionary<string, DefaultSerializationKind> DefaultSerializationKinds = new()
    {
        { FloatTypeName, DefaultSerializationKind.Float },
        { ByteTypeName, DefaultSerializationKind.Byte },
        { SbyteTypeName, DefaultSerializationKind.Sbyte },
        { BoolTypeName, DefaultSerializationKind.Bool },
        { CharTypeName, DefaultSerializationKind.Char },
        { IntTypeName, DefaultSerializationKind.Int },
        { LongTypeName, DefaultSerializationKind.Long },
        { ShortTypeName, DefaultSerializationKind.Short },
        { UintTypeName, DefaultSerializationKind.Uint },
        { UlongTypeName, DefaultSerializationKind.Ulong },
        { UshortTypeName, DefaultSerializationKind.Ushort },
        { DoubleTypeName, DefaultSerializationKind.Double },
        { DecimalTypeName, DefaultSerializationKind.Decimal },
        { StringTypeName, DefaultSerializationKind.String },
        { VoidTypeName, DefaultSerializationKind.Void }
    };
}