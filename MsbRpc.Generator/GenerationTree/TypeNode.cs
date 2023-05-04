using System.Collections.Generic;
using MsbRpc.Generator.CodeWriters.Utility;
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
    public readonly Serialization? Serialization;

    public TypeNode(TypeInfo typeInfo, IReadOnlyDictionary<TypeInfo, CustomSerializationNode> customSerializations)
    {
        IsVoid = typeInfo.Name == IndependentNames.Types.Void;

        if (IsVoid)
        {
            DeclarationSyntax = "void";
            return;
        }

        bool isDefaultSerializableType = SimpleDefaultSerializationKindUtility.TryGet(typeInfo, out SimpleDefaultSerializationKind defaultSerializationKind);

        DeclarationSyntax = isDefaultSerializableType && defaultSerializationKind.TryGetKeyword(out string keyword)
            ? typeInfo.IsNullableReferenceType ? keyword + '?' : keyword
            : typeInfo.IsNullableReferenceType
                ? typeInfo.Name + '?'
                : typeInfo.Name;

        if (customSerializations.TryGetValue(typeInfo, out CustomSerializationNode? customSerialization))
        {
            Serialization = new Serialization(customSerialization, typeInfo.IsNullableReferenceType);
            return;
        }

        if (isDefaultSerializableType)

        {
            Serialization = new Serialization(defaultSerializationKind, typeInfo.IsNullableReferenceType);
        }
    }
}