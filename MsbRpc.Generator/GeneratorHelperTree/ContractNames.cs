namespace MsbRpc.Generator.GeneratorHelperTree;

public readonly struct ContractNames
{
    public ContractNames(string contractNamespace, string contractInterfaceName)
    {
        UpperCaseContract = GetContractName(contractInterfaceName);
        LowerCaseContract = UpperCaseContract.WithLowerFirstChar();
        GeneratedNamespace = $"{contractNamespace}.Generated";
    }

    private static string GetContractName(string contractInterfaceName)
    {
        string iStripped = contractInterfaceName.StartsWith(IndependentNames.InterfacePrefix, StringComparison.Ordinal)
                           && char.IsUpper(contractInterfaceName[1])
            ? contractInterfaceName.Substring(1)
            : contractInterfaceName;
        return iStripped.WithUpperFirstChar();
    }

    public readonly string GeneratedNamespace;
    public readonly string UpperCaseContract;
    public readonly string LowerCaseContract;
}