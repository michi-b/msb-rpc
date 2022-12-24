namespace MsbRpc.Generator.HelperTree;

public readonly struct ContractNames
{
    public ContractNames(string contractNamespace, string contractInterfaceName)
    {
        UpperCaseName = GetContractName(contractInterfaceName);
        LowerCaseName = UpperCaseName.ToLowerFirstChar();
        GeneratedNamespace = $"{contractNamespace}.Generated";
    }

    private static string GetContractName(string contractInterfaceName)
    {
        string iStripped = contractInterfaceName.StartsWith(IndependentNames.InterfacePrefix, StringComparison.Ordinal)
                           && char.IsUpper(contractInterfaceName[1])
            ? contractInterfaceName.Substring(1)
            : contractInterfaceName;
        return iStripped.ToUpperFirstChar();
    }

    public readonly string GeneratedNamespace;
    public readonly string UpperCaseName;
    public readonly string LowerCaseName;
}