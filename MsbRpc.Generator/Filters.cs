using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MsbRpc.Generator;

public static class Filters
{
    public static bool IsInterfaceDeclarationWithAttributes(SyntaxNode node, CancellationToken cancellationToken)
        => (node as InterfaceDeclarationSyntax)?.AttributeLists.Count > 0;
}