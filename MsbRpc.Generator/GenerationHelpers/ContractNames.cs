namespace MsbRpc.Generator.GenerationHelpers;

public readonly ref struct ContractNames
{
    private const string InterfacePrefix = "I";
    private const string ServerPostfix = "Server";
    private const string ProcedurePostfix = "Procedure";

    /// <summary>{ContractNamespace}.Generated.{Contract}ServerEndpoint.g.cs</summary>
    public string ServerEndpointFile { get; }

    /// <summary>{ContractNamespace}.Generated.I{Contract}Server.g.cs</summary>
    public string ServerInterfaceFile { get; }

    /// <summary>{ContractNamespace}.Generated.{Contract}ServerProcedure.g.cs</summary>
    public string ServerProcedureFile { get; }

    /// <summary>{ContractNamespace}.Generated.{Contract}ServerProcedureExtensions.g.cs</summary>
    public string ServerProcedureEnumExtensionsFile { get; }

    /// <summary>{ContractNamespace}.Generated</summary>
    public string GeneratedNamespace { get; }

    /// <summary>_{contract}Server</summary>
    public string ServerField { get; }
    
    /// <summary>{contract}Server</summary>
    public string ServerParameter { get; }

    /// <summary>I{Contract}Server</summary>
    public string ServerInterface { get; }

    /// <summary>{Contract}ServerProcedure</summary>
    public string ServerProcedure { get; }

    /// <summary>{Contract}ServerProcedureExtensions</summary>
    public string ServerProcedureEnumExtensions { get; }

    /// <summary>{Contract}ServerEndpoint</summary>
    public string ServerEndPoint { get; }

    public ContractNames(string contractNamespace, string contractInterfaceName)
    {
        string contractName = GetContractName(contractInterfaceName);
        string lowerCaseContractName = GetLowerCaseContractName(contractName);
        
        const string generatedFileEnding = ".g.cs";

        GeneratedNamespace = $"{contractNamespace}.Generated";

        ServerField = $"_{lowerCaseContractName}{ServerPostfix}";
        ServerParameter = $"{lowerCaseContractName}{ServerPostfix}";

        ServerInterface = 'I' + contractName + ServerPostfix;
        ServerInterfaceFile = $"{GeneratedNamespace}.{ServerInterface}{generatedFileEnding}";

        ServerProcedure = $"{contractName}{ServerPostfix}{ProcedurePostfix}";
        ServerProcedureFile = $"{GeneratedNamespace}.{ServerProcedure}{generatedFileEnding}";

        ServerProcedureEnumExtensions = $"{contractName}{ServerPostfix}{ProcedurePostfix}Extensions";
        ServerProcedureEnumExtensionsFile = $"{GeneratedNamespace}.{ServerProcedureEnumExtensions}{generatedFileEnding}";

        ServerEndPoint = $"{contractName}{ServerPostfix}Endpoint";
        ServerEndpointFile = $"{GeneratedNamespace}.{ServerEndPoint}{generatedFileEnding}";
    }

    private static string GetContractName(string contractInterfaceName)
    {
        string iStripped = contractInterfaceName.StartsWith(InterfacePrefix, StringComparison.Ordinal)
                           && char.IsUpper(contractInterfaceName[1])
            ? contractInterfaceName.Substring(1)
            : contractInterfaceName;
        char firstChar = char.ToUpperInvariant(iStripped[0]);
        return firstChar + iStripped.Substring(1);
    }

    private static string GetLowerCaseContractName(string contractName)
    {
        char firstChar = char.ToLowerInvariant(contractName[0]);
        return firstChar + contractName.Substring(1);
    }
}