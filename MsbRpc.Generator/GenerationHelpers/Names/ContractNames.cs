﻿namespace MsbRpc.Generator.GenerationHelpers.Names;

public class ContractNames
{
    public const string ProcedurePostfix = "Procedure";

    private readonly EndPointNames _client;
    private readonly EndPointNames _server;

    /// <summary>{ContractNamespace}.Generated</summary>
    public string GeneratedNamespace { get; }

    public EndPointNames this[EndPointId endPointId]
        => endPointId switch
        {
            EndPointId.Client => _client,
            EndPointId.Server => _server,
            _ => throw new ArgumentOutOfRangeException(nameof(endPointId), endPointId, null)
        };

    public ContractNames(string contractNamespace, string contractInterfaceName)
    {
        string contractName = GetContractName(contractInterfaceName);
        GeneratedNamespace = $"{contractNamespace}.Generated";
        _server = new EndPointNames(GeneratedNamespace, contractName, EndPointId.Server);
        _client = new EndPointNames(GeneratedNamespace, contractName, EndPointId.Client);
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