using System.Collections.Immutable;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class ContractNode
{
    public readonly EndPoint Client;
    public readonly ProcedureCollection? ClientProcedures;
    public readonly ContractNames Names;
    public readonly EndPoint Server;
    public readonly ProcedureCollection? ServerProcedures;

    public ContractNode(ref ContractInfo info)
    {
        Names = new ContractNames(info.Namespace, info.InterfaceName);

        var typeCache = new TypeCache();

        var clientNames = new EndPointNames(Names, EndPointTypeId.Client);
        var serverNames = new EndPointNames(Names, EndPointTypeId.Server);

        ProcedureCollection? TryCreateProceduresDefinition(ImmutableArray<ProcedureInfo> procedures, EndPointNames endPointNames)
            => procedures.Length > 0 ? new ProcedureCollection(procedures, endPointNames, typeCache) : null;

        ClientProcedures = TryCreateProceduresDefinition(info.Client.Procedures, clientNames);
        ServerProcedures = TryCreateProceduresDefinition(info.Server.Procedures, serverNames);

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