namespace MsbRpc.Generator.Info;

public readonly struct ContractNames
{
    private const string InterfacePrefix = "I";
    private const string ServerPostfix = "Server";
    private const string ProcedurePostfix = "Procedure";

    public ContractNames(string contractInterfaceName, string namespaceName)
    {
        string contractName;
        if (contractInterfaceName.StartsWith(InterfacePrefix, StringComparison.Ordinal) && char.IsUpper(contractInterfaceName[1]))
        {
            contractName = contractInterfaceName.Substring(1);
        }
        else
        {
            contractName = contractInterfaceName;
            contractInterfaceName = InterfacePrefix + contractInterfaceName;
        }

        const string generatedFileEnding = ".g.cs";

        GeneratedNamespace = namespaceName + ".Generated";

        ServerInterfaceName = contractInterfaceName + ServerPostfix;
        ServerInterfaceFileName = $"{GeneratedNamespace}.{ServerInterfaceName}{generatedFileEnding}";

        ServerProcedureEnumName = contractName + ServerPostfix + ProcedurePostfix;
        ServerProcedureEnumFileName = $"{GeneratedNamespace}.{ServerProcedureEnumName}{generatedFileEnding}";
    }

    public string GeneratedNamespace { get; }
    public string ServerInterfaceName { get; }
    public string ServerInterfaceFileName { get; }

    public string ServerProcedureEnumName { get; }

    public string ServerProcedureEnumFileName { get; }
}
