using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Generator.Extensions;

internal static class NamedTypeSymbolExtensions
{
    public static bool GetIsRpcContractAttribute(this INamedTypeSymbol attributeClass)
        => attributeClass.Name == nameof(RpcContractAttribute) && attributeClass.ContainingNamespace.GetIsAttributesNamespace();

    public static bool GetIsGenerateServerAttribute(this INamedTypeSymbol attributeClass)
        => attributeClass.Name == nameof(GenerateServerAttribute) && attributeClass.ContainingNamespace.GetIsAttributesNamespace();

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