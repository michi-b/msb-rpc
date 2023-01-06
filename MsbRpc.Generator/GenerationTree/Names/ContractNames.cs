using System;

namespace MsbRpc.Generator.GenerationTree.Names;

internal readonly struct ContractNames
{
    public ContractNames(string contractNamespace, string contractInterfaceName)
    {
        InterfaceName = contractInterfaceName;
        PascalCaseName = GetContractName(contractInterfaceName);
        CamelCaseName = PascalCaseName.ToCamelCase();
        GeneratedNamespace = $"{contractNamespace}.Generated";
    }

    private static string GetContractName(string contractInterfaceName)
    {
        string iStripped = contractInterfaceName.StartsWith(IndependentNames.InterfacePrefix, StringComparison.Ordinal)
                           && char.IsUpper(contractInterfaceName[1])
            ? contractInterfaceName.Substring(1)
            : contractInterfaceName;
        return iStripped.ToPascalCase();
    }

    public readonly string GeneratedNamespace;
    public readonly string PascalCaseName;
    public readonly string CamelCaseName;
    public readonly string InterfaceName;
}