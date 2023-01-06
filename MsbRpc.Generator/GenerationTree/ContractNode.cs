using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.GenerationTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ContractNode
{
    public readonly EndPoint Client;
    public readonly bool IsValid = true;
    public readonly ContractNames Names;
    public readonly EndPoint Server;
    public readonly ProcedureCollection? Procedures;

    public ContractNode(ref ContractInfo info, SourceProductionContext context)
    {
        Names = new ContractNames(info.Namespace, info.InterfaceName);

        var typeCache = new TypeNodeCache();

        var clientNames = new EndPointNames(Names, EndPointTypeId.Client);
        var serverNames = new EndPointNames(Names, EndPointTypeId.Server);

        ImmutableArray<ProcedureInfo> procedures = info.Procedures;
        
        Procedures = procedures.Length > 0 ? new ProcedureCollection(procedures, Names, typeCache, context) : null;
        
        IsValid = IsValid && (Procedures == null || Procedures.IsValid);


        Client = new EndPoint
        (
            clientNames,
            Procedures
        );

        Server = new EndPoint
        (
            serverNames,
            Procedures
        );
    }
}