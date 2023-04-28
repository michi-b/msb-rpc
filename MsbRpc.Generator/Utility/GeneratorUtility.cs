using System;
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
        var typeDeclarationSyntax = syntaxNode as TypeDeclarationSyntax;
        if (typeDeclarationSyntax == null)
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

    public static ConstantSizeSerializerInfo? GetConstantSizeSerializerInfo(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        SemanticModel semanticModel = context.SemanticModel;

        if (semanticModel.GetDeclaredSymbol(context.Node, cancellationToken) is not INamedTypeSymbol constantSizeSerializer)
        {
            return null;
        }

        TypeKind typeKind = constantSizeSerializer.TypeKind;
        if (typeKind != TypeKind.Class && typeKind != TypeKind.Struct)
        {
            return null;
        }

        return ConstantSizeSerializerInfoParser.Parse(constantSizeSerializer);
    }
}