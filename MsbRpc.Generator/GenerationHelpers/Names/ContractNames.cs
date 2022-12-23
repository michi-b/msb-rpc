﻿namespace MsbRpc.Generator.GenerationHelpers.Names;

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

    private static string GetContractName(string contractInterfaceName)
    {
        string iStripped = contractInterfaceName.StartsWith(GeneralNames.InterfacePrefix, StringComparison.Ordinal)
                           && char.IsUpper(contractInterfaceName[1])
            ? contractInterfaceName.Substring(1)
            : contractInterfaceName;
        return iStripped.WithUpperFirstChar();
    }
}