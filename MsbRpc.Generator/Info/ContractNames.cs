namespace MsbRpc.Generator;

public readonly struct ContractNames
{
    private const string ServerPostfix = "Server";

    public ContractNames(string name, string namespaceName)
    {
        const string generatedFileEnding = ".g.cs";

        GeneratedNamespace = namespaceName + ".Generated";

        ServerInterfaceName = name + ServerPostfix;
        ServerInterfaceFileName = $"{GeneratedNamespace}.{ServerInterfaceName}{generatedFileEnding}";
    }

    public string GeneratedNamespace { get; }
    public string ServerInterfaceName { get; }
    public string ServerInterfaceFileName { get; }
}
