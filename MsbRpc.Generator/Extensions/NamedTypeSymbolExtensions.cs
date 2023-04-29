using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Extensions;

internal static class NamedTypeSymbolExtensions
{
    private static readonly List<string> ReusableStringList = new(10);
    private static readonly StringBuilder ReusableStringBuilder = new(100);

    public static string GetFullName(this INamedTypeSymbol symbol)
    {
        ReusableStringBuilder.Clear();
        ReusableStringList.Clear();

        INamespaceSymbol? nameSpace = symbol.ContainingNamespace;
        while (nameSpace is not null && !nameSpace.IsGlobalNamespace)
        {
            ReusableStringList.Add(nameSpace.Name);
            nameSpace = nameSpace.ContainingNamespace;
        }

        for (int i = ReusableStringList.Count - 1; i >= 0; i--)
        {
            ReusableStringBuilder.Append(ReusableStringList[i]);
            ReusableStringBuilder.Append('.');
        }

        ReusableStringBuilder.Append(symbol.Name);

        return ReusableStringBuilder.ToString();
    }

    public static bool GetIsRpcContractAttribute(this INamedTypeSymbol attributeClass)
        => attributeClass.Name == "RpcContractAttribute" && attributeClass.ContainingNamespace.GetIsAttributesNamespace();

    public static bool GetIsRpcContractInterface(this INamedTypeSymbol interfaceSymbol)
        => interfaceSymbol.Name == "IRpcContract" && interfaceSymbol.ContainingNamespace.GetIsContracts();

    public static bool GetIsNullable(this INamedTypeSymbol typeSymbol) => typeSymbol.Name == "Nullable" && typeSymbol.ContainingNamespace.GetIsSystem();

    public static bool GetIsConstantSizeSerializerAttribute(this INamedTypeSymbol attributeClass)
        => attributeClass is { Name: "ConstantSizeSerializerAttribute" } && attributeClass.ContainingNamespace.GetIsSerializerAttributes();

    public static bool GetIsSerializationMethodAttribute(this INamedTypeSymbol attributeClass)
        => attributeClass is { Name: "SerializationMethodAttribute" } && attributeClass.ContainingNamespace.GetIsSerializerAttributes();

    public static bool GetIsDeserializationMethodAttribute(this INamedTypeSymbol attributeClass)
        => attributeClass is { Name: "DeserializationMethodAttribute" } && attributeClass.ContainingNamespace.GetIsSerializerAttributes();

    public static bool GetIsSerializedSizeAttribute(this INamedTypeSymbol attributeClass)
        => attributeClass is { Name: "SerializedSizeAttribute" } && attributeClass.ContainingNamespace.GetIsSerializerAttributes();

    public static bool GetIsBufferWriter(this INamedTypeSymbol target) => target is { Name: "BufferWriter" } && target.ContainingNamespace.GetIsBuffers();

    public static bool GetIsBufferReader(this INamedTypeSymbol target) => target is { Name: "BufferReader" } && target.ContainingNamespace.GetIsBuffers();
}