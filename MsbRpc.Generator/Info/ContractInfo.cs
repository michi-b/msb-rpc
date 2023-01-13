using System;
using System.Collections.Immutable;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Generator.Info;

internal readonly struct ContractInfo : IEquatable<ContractInfo>
{
    public readonly string InterfaceName;
    public readonly string Namespace;
    public readonly ImmutableArray<ProcedureInfo> Procedures;
    public readonly RpcContractType ContractType;
    public readonly bool GenerateServer;
    public readonly int DefaultPort;

    internal ContractInfo
    (
        string interfaceName,
        string namespaceName,
        ImmutableArray<ProcedureInfo> procedures,
        RpcContractType contractType,
        bool generateServer,
        int defaultPort
    )
    {
        InterfaceName = interfaceName;
        Namespace = namespaceName;
        Procedures = procedures;
        ContractType = contractType;
        GenerateServer = generateServer;
        DefaultPort = defaultPort;
    }

    public bool Equals(ContractInfo other)
        => InterfaceName == other.InterfaceName
           && Namespace == other.Namespace
           && Procedures.Equals(other.Procedures)
           && ContractType == other.ContractType
           && GenerateServer == other.GenerateServer
           && DefaultPort == other.DefaultPort;

    public override bool Equals(object? obj) => obj is ContractInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = InterfaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            hashCode = (hashCode * 397) ^ Procedures.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)ContractType;
            hashCode = (hashCode * 397) ^ GenerateServer.GetHashCode();
            hashCode = (hashCode * 397) ^ DefaultPort;
            return hashCode;
        }
    }
}