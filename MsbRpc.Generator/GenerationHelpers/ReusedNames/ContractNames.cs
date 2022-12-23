using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers.ReusedNames;

public class ContractNames
{
    public const string ProcedurePostfix = "Procedure";
    public string GeneratedNamespace { get; }
    public string ContractName { get; }
    public string LowerCaseContractName { get; }
    
    public ContractNames(string contractNamespace, string contractInterfaceName)
    {
        ContractName = GetContractName(contractInterfaceName);
        LowerCaseContractName = ContractName.WithLowerFirstChar();
        GeneratedNamespace = $"{contractNamespace}.Generated";
    }

    public ContractNames(ContractInfo info)
    {
        ContractName = GetContractName(info.InterfaceName);
        LowerCaseContractName = ContractName.WithLowerFirstChar();
        GeneratedNamespace = $"{info.Namespace}.Generated";
    }

    private static string GetContractName(string contractInterfaceName)
    {
        string iStripped = contractInterfaceName.StartsWith(IndependentNames.InterfacePrefix, StringComparison.Ordinal)
                           && char.IsUpper(contractInterfaceName[1])
            ? contractInterfaceName.Substring(1)
            : contractInterfaceName;
        return iStripped.WithUpperFirstChar();
    }
}