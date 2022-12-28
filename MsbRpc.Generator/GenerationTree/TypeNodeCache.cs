using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Info;
using TypeInfo = MsbRpc.Generator.Info.TypeInfo;

namespace MsbRpc.Generator.GenerationTree;

internal readonly struct TypeNodeCache
{
    private readonly Dictionary<TypeInfo, TypeNode> _cache = new();

    public TypeNodeCache() { }

    public TypeNode GetOrAdd(TypeInfo typeInfo, SourceProductionContext context)
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
        string fullName = typeInfo.Namespace + "." + typeInfo.LocalName;
        SerializationKind serializationKind = GetIndependentSerializationKind(fullName);
        return new TypeNode(fullName, serializationKind);
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
    private const string VoidTypeName = "System.Void";
}