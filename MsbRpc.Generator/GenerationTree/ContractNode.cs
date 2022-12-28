using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.GenerationTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class ContractNode
{
    public readonly EndPoint Client;
    public readonly ProcedureCollection? ClientProcedures;
    public readonly bool IsValid = true;
    public readonly ContractNames Names;
    public readonly EndPoint Server;
    public readonly ProcedureCollection? ServerProcedures;

    public ContractNode(ref ContractInfo info, SourceProductionContext context)
    {
        Names = new ContractNames(info.Namespace, info.InterfaceName);

        var typeCache = new TypeNodeCache();

        var clientNames = new EndPointNames(Names, EndPointTypeId.Client);
        var serverNames = new EndPointNames(Names, EndPointTypeId.Server);

        bool areProceduresValid = true;

        ProcedureCollection? CreateProceduresDefinition(ImmutableArray<ProcedureInfo> procedures, EndPointNames endPointNames)
        {
            if (procedures.Length > 0)
            {
                var collection = new ProcedureCollection(procedures, endPointNames, typeCache, context);
                areProceduresValid = areProceduresValid && collection.IsValid;
                return collection;
            }

            return null;
        }

        ClientProcedures = CreateProceduresDefinition(info.Client.Procedures, clientNames);
        ServerProcedures = CreateProceduresDefinition(info.Server.Procedures, serverNames);

        IsValid = IsValid && areProceduresValid;

        Client = new EndPoint
        (
            EndPointTypeId.Client,
            this,
            clientNames
        );

        Server = new EndPoint
        (
            EndPointTypeId.Server,
            this,
            serverNames
        );
    }
}