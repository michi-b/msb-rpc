using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Utility;

public static class WellKnownAttributes
{
    public static bool IsRpcContractAttribute(ISymbol attributeClass)
        => attributeClass is
        {
            Name: "RpcContractAttribute",
            ContainingNamespace:
            {
                Name: "Attributes",
                ContainingNamespace:
                {
                    Name: "Generator", ContainingNamespace: { Name: "MsbRpc", ContainingNamespace: { IsGlobalNamespace: true } }
                }
            }
        };
}