using Microsoft.CodeAnalysis;
using MsbRps.Generator.Extensions;
using MsbRps.GeneratorAttributes;

namespace MsbRps.Generator;

public class Symbols
{
    private static Symbols? _instance;

    public static readonly string MsbRpsObjectAttributeName = typeof(MsbRpsObject).FullName;
    public readonly INamedTypeSymbol MsbRpsObjectAttribute;

    private Symbols(Compilation compilation) => MsbRpsObjectAttribute = compilation.ResolveType(MsbRpsObjectAttributeName);

    public static Symbols Get(Compilation compilation)
    {
        return _instance ??= new Symbols(compilation);
    }
}