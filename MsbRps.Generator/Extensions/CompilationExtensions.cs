using Microsoft.CodeAnalysis;
using MsbRps.Generator.Exceptions;

namespace MsbRps.Generator.Extensions;

public static class CompilationExtensions
{
    public static INamedTypeSymbol ResolveType(this Compilation target, string symbolName)
    {
        INamedTypeSymbol? result = target.GetTypeByMetadataName(symbolName);
        if (result == null)
        {
            throw new SymbolCannotBeResolvedException(symbolName);
        }
        return result;
    }
}