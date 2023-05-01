using System.Collections.Generic;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    /// <summary>
    ///     the type name for declarations, e.g. string, int etc., or the qualified reference name of the type otherwise
    /// </summary>
    public readonly string DeclarationSyntax;

    public readonly bool IsVoid;
    public readonly Serialization.Serialization? Serialization;

    public TypeNode(TypeInfo typeInfo, IReadOnlyDictionary<string, CustomSerializationNode> customSerializations)
    {
        IsVoid = typeInfo.Name == IndependentNames.Types.Void;

        if (IsVoid)
        {
            return;
        }
        
        bool isDefaultSerializableType = DefaultSerializationKindUtility.DefaultSerializationKinds.TryGetValue
            (typeInfo.Name, out DefaultSerializationKind defaultSerializationKind);

        DeclarationSyntax = isDefaultSerializableType && defaultSerializationKind.TryGetKeyword(out string keyword)
            ? typeInfo.IsNullable ? keyword + '?' : keyword
            : typeInfo.IsNullable
                ? typeInfo.Name + '?'
                : typeInfo.Name;


        if (customSerializations.TryGetValue(typeInfo.Name, out CustomSerializationNode? customSerialization))
        {
            Serialization = new Serialization.Serialization(customSerialization, typeInfo.IsNullable);
            return;
        }

        if (isDefaultSerializableType)

        {
            Serialization = new Serialization.Serialization(defaultSerializationKind, typeInfo.IsNullable);
        }
    }
}