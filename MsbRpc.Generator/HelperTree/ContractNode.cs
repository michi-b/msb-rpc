using System.Collections.Immutable;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class ContractNode
{
    private readonly ProcedureCollection? _clientProcedures;
    private readonly ProcedureCollection? _serverProcedures;
    public readonly EndPoint Client;
    public readonly ContractNames Names;
    public readonly EndPoint Server;

    public ContractNode(ref ContractInfo info)
    {
        Names = new ContractNames(info.Namespace, info.InterfaceName);

        var typeCache = new TypeCache();

        var clientNames = new EndPointNames(Names, EndPointTypeId.Client);
        var serverNames = new EndPointNames(Names, EndPointTypeId.Server);

        ProcedureCollection? TryCreateProceduresDefinition(ImmutableArray<ProcedureInfo> procedures, EndPointNames endPointNames)
            => procedures.Length > 0 ? new ProcedureCollection(procedures, endPointNames, typeCache) : null;

        _clientProcedures = TryCreateProceduresDefinition(info.Client.Procedures, clientNames);
        _serverProcedures = TryCreateProceduresDefinition(info.Server.Procedures, serverNames);

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

    public bool TryGetProcedures(EndPointTypeId endPointType, out ProcedureCollection? procedures)
    {
        return endPointType switch
        {
            EndPointTypeId.Client => TryGetClientProcedures(out procedures),
            EndPointTypeId.Server => TryGetServerProcedures(out procedures),
            _ => throw new ArgumentOutOfRangeException(nameof(endPointType), endPointType, null)
        };
    }

    public bool TryGetClientProcedures(out ProcedureCollection? procedures)
    {
        if (_clientProcedures is null)
        {
            procedures = null;
            return false;
        }

        procedures = _clientProcedures;
        return true;
    }

    private bool TryGetServerProcedures(out ProcedureCollection? procedures)
    {
        if (_serverProcedures is null)
        {
            procedures = null;
            return false;
        }

        procedures = _serverProcedures;
        return true;
    }
}