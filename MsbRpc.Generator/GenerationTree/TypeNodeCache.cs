using System.Collections.Generic;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal readonly struct TypeNodeCache
{
    private readonly Dictionary<TypeInfo, TypeNode> _cache = new();

    public TypeNodeCache() { }

    public TypeNode GetOrAdd(TypeInfo typeInfo)
    {
        if (_cache.TryGetValue(typeInfo, out TypeNode? node))
        {
            return node;
        }

        node = Create(ref typeInfo);

        _cache.Add(typeInfo, node);

        return node;
    }

    private static TypeNode Create(ref TypeInfo typeInfo)
    {
        string fullName = typeInfo.Name;
        SerializationKind serializationKind = GetIndependentSerializationKind(fullName);
        bool isNullable = typeInfo.IsNullable;
        return new TypeNode(fullName, serializationKind, isNullable);
    }

    private static SerializationKind GetIndependentSerializationKind(string typeName)
    {
        return typeName switch
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
            StringTypeName => SerializationKind.String,
            VoidTypeName => SerializationKind.Void,
            _ => SerializationKind.Unresolved
        };
    }

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
}