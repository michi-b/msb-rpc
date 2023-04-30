using System.Collections.Generic;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    /// <summary>
    ///     the type name for declarations, e.g. string, int etc., or the qualified reference name of the type otherwise
    /// </summary>
    public readonly string DeclarationSyntax;

    public readonly bool IsVoid;
    public readonly SerializationNode? Serialization;

    public TypeNode(TypeInfo typeInfo, IReadOnlyDictionary<string, CustomSerializationNode> customSerializations)
    {
        bool isDefaultSerializableType = DefaultSerializationKindUtility.DefaultSerializationKinds.TryGetValue
            (typeInfo.Name, out DefaultSerializationKind defaultSerializationKind);

        DeclarationSyntax = isDefaultSerializableType && defaultSerializationKind.TryGetKeyword(out string keyword)
            ? typeInfo.IsNullable ? keyword + '?' : keyword
            : typeInfo.IsNullable
                ? typeInfo.Name + '?'
                : typeInfo.Name;

        IsVoid = isDefaultSerializableType && defaultSerializationKind == DefaultSerializationKind.Void;

        if (IsVoid)
        {
            return;
        }

        if (customSerializations.TryGetValue(typeInfo.Name, out CustomSerializationNode? customSerialization))
        {
            Serialization = new SerializationNode(customSerialization, typeInfo.IsNullable);
            return;
        }

        if (isDefaultSerializableType)

        {
            Serialization = new SerializationNode(defaultSerializationKind, typeInfo.IsNullable);
        }
    }
}