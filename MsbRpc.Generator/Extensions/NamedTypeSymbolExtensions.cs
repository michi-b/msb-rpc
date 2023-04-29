using System.Text;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Extensions;

internal static class NamedTypeSymbolExtensions
{
    private const int DefaultStringBuilderCapacity = 100;

    /// <summary>internal method for symbol referencing name computation</summary>
    /// <returns>the fully qualified reference name of the symbol</returns>
    /// <remarks>
    ///     this is different from ISymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
    ///     in that it results e.g. in "System.Int32" instead of just "int" for C# type keywords
    /// </remarks>
    /// <seealso cref="ISymbol.ToDisplayString" />
    public static string GetFullName(this INamedTypeSymbol symbol)
    {
        StringBuilder stringBuilder = new(DefaultStringBuilderCapacity);

        ISymbol? containingSymbol = symbol.ContainingSymbol;
        while (containingSymbol is not null && !containingSymbol.GetIsGlobalNamespace())
        {
            stringBuilder.Insert(0, '.');
            stringBuilder.Insert(0, containingSymbol.Name);
            containingSymbol = containingSymbol.ContainingSymbol;
        }

        stringBuilder.Append(symbol.Name);

        return stringBuilder.ToString();
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