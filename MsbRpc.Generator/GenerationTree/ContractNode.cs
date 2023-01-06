using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Attributes;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ContractNode
{

    public readonly bool IsValid = true;
    public readonly ProcedureCollection? Procedures;
    public readonly EndPointNode Client;
    public readonly EndPointNode Server;

    public readonly string InterfaceName;
    public readonly string PascalCaseName;
    public readonly string CamelCaseName;
    public readonly string Namespace;
    
    public ContractNode(ref ContractInfo info, SourceProductionContext context)
    {
        InterfaceName = info.InterfaceName;
        PascalCaseName = GetContractName(InterfaceName);
        CamelCaseName = PascalCaseName.ToCamelCase();
        Namespace = $"{info.Namespace}{GeneratedNamespacePostFix}";

        var typeCache = new TypeNodeCache();

        ImmutableArray<ProcedureInfo> procedures = info.Procedures;

        Procedures = procedures.Length > 0 ? new ProcedureCollection(procedures, this, typeCache, context) : null;

        IsValid = IsValid && (Procedures == null || Procedures.IsValid);

        Client = new EndPointNode
        (
            this,
            info.ContractType switch
            {
                RpcContractType.Server => EndPointType.OutboundClient,
                RpcContractType.ClientToServer => EndPointType.OutboundClient,
                RpcContractType.ServerToClient => EndPointType.InboundClient,
                _ => throw new ArgumentOutOfRangeException()
            }
        );

        Server = new EndPointNode
        (
            this,
            info.ContractType switch
            {
                RpcContractType.Server => EndPointType.Server,
                RpcContractType.ClientToServer => EndPointType.InboundServer,
                RpcContractType.ServerToClient => EndPointType.OutboundServer,
                _ => throw new ArgumentOutOfRangeException()
            }
        );
    }

    private static string GetContractName(string contractInterfaceName)
    {
        string iStripped = contractInterfaceName.StartsWith(InterfacePrefix, StringComparison.Ordinal)
                           && char.IsUpper(contractInterfaceName[1])
            ? contractInterfaceName.Substring(1)
            : contractInterfaceName;
        return iStripped.ToPascalCase();
    }
}