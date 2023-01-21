using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class ContractNode
{
    public readonly string CamelCaseName;
    public readonly EndPointNode ClientEndPoint;

    public readonly string InterfaceName;
    public readonly string InterfaceType;

    public readonly bool IsValid = true;
    public readonly string Namespace;
    public readonly string PascalCaseName;
    public readonly ProcedureCollectionNode Procedures;
    public readonly ServerNode? Server;
    public readonly EndPointNode ServerEndPoint;

    public ContractNode(ref ContractInfo info, SourceProductionContext context)
    {
        InterfaceName = info.InterfaceName;
        InterfaceType = $"{info.Namespace}.{info.InterfaceName}";
        PascalCaseName = GetContractName(InterfaceName);
        CamelCaseName = PascalCaseName.ToCamelCase();
        Namespace = $"{info.Namespace}{GeneratedNamespacePostFix}";

        var typeCache = new TypeNodeCache();

        ImmutableArray<ProcedureInfo> procedures = info.Procedures;

        Procedures = new ProcedureCollectionNode(procedures, this, typeCache, context);

        IsValid = IsValid && Procedures.IsValid;

        ClientEndPoint = CreateEndPointNode(info, ConnectionEndType.Client);
        ServerEndPoint = CreateEndPointNode(info, ConnectionEndType.Server);
        Server = ServerNode.Create(info, ServerEndPoint);
    }

    private EndPointNode CreateEndPointNode(ContractInfo info, ConnectionEndType endType)
        => new
        (
            this,
            endType,
            info.ContractType.GetDirection(endType)
        );

    private static string GetContractName(string contractInterfaceName)
    {
        string iStripped = contractInterfaceName.StartsWith(InterfacePrefix, StringComparison.Ordinal)
                           && char.IsUpper(contractInterfaceName[1])
            ? contractInterfaceName.Substring(1)
            : contractInterfaceName;
        return iStripped.ToPascalCase();
    }
}