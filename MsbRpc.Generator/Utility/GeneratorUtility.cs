using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Utility;

internal static class GeneratorUtility
{
    public static bool GetIsAttributedInterfaceDeclarationSyntax(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        var interfaceDeclarationSyntax = syntaxNode as InterfaceDeclarationSyntax;
        return interfaceDeclarationSyntax != null && interfaceDeclarationSyntax.AttributeLists.Any();
    }

    public static bool GetIsAttributedNonInterfaceTypeDeclarationSyntax(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        if (syntaxNode is not TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return false;
        }

        Type typeType = typeDeclarationSyntax.GetType();
        return (typeType == typeof(ClassDeclarationSyntax)
                || typeType == typeof(StructDeclarationSyntax)
                || typeType == typeof(RecordDeclarationSyntax))
               && typeDeclarationSyntax.AttributeLists.Any();
    }

    public static ContractInfo? GetContractInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = context.SemanticModel;

        // get interface symbol
        if (semanticModel.GetDeclaredSymbol(context.Node, cancellationToken) is not INamedTypeSymbol contractInterface)
        {
            return null;
        }

        // check that interfaceSymbol is actually an interface and derives from IRpcContract
        if (contractInterface.TypeKind != TypeKind.Interface
            || !contractInterface.Interfaces.Any(TypeCheck.IsRpcContractInterface))
        {
            return null;
        }

        return ContractInfoParser.Parse(contractInterface);
    }

    public static ImmutableArray<CustomSerializerInfo> GetCustomSerializerInfos(GeneratorSyntaxContext context, CancellationToken cancellationToken)
        => EnumerateCustomSerializerInfos(context, cancellationToken).ToImmutableArray();

    private static IEnumerable<CustomSerializerInfo> EnumerateCustomSerializerInfos(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = context.SemanticModel;

        if (semanticModel.GetDeclaredSymbol(context.Node, cancellationToken) is not INamedTypeSymbol serializerTypeSymbol)
        {
            yield break;
        }

        TypeKind typeKind = serializerTypeSymbol.TypeKind;
        if (typeKind != TypeKind.Class && typeKind != TypeKind.Struct)
        {
            yield break;
        }

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        // might want to catch different types of attributes in the future
        foreach (AttributeData customSerializerAttribute in serializerTypeSymbol.GetAttributes())
        {
            INamedTypeSymbol? attributeClass = customSerializerAttribute.AttributeClass;
            if (attributeClass != null)
            {
                if (attributeClass.IsConstantSizeSerializerAttribute())
                {
                    CustomSerializerInfo? customSerializerInfo = ConstantSizeSerializerInfoParser.Parse(serializerTypeSymbol, customSerializerAttribute);
                    if (customSerializerInfo != null)
                    {
                        yield return customSerializerInfo.Value;
                    }
                }
            }
        }
    }
}