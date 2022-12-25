using System.Collections.Immutable;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class ContractNode
{
    public readonly EndPoint Client;
    public readonly ContractNames Names;
    public readonly EndPoint Server;
    public readonly TypeCache TypeCache;
    public readonly ProcedureCollection? ClientProcedures;
    public readonly ProcedureCollection? ServerProcedures;

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
        if(ClientProcedures is null)
        {
            procedures = null;
            return false;
        }
        procedures = ClientProcedures;
        return true;
    }
    
    public bool TryGetServerProcedures(out ProcedureCollection? procedures)
    {
        if(ServerProcedures is null)
        {
            procedures = null;
            return false;
        }
        procedures = ServerProcedures;
        return true;
    }

    public ContractNode(ref ContractInfo info)
    {
        Names = new ContractNames(info.Namespace, info.InterfaceName);

        TypeCache = new TypeCache();

        var clientNames = new EndPointNames(Names, EndPointTypeId.Client);
        var serverNames = new EndPointNames(Names, EndPointTypeId.Server);

        ProcedureCollection? TryCreateProceduresDefinition(ImmutableArray<ProcedureInfo> procedures, EndPointNames endPointNames) 
            => procedures.Length > 0 ? new ProcedureCollection(procedures, Names, endPointNames, TypeCache) : null;
        
        ClientProcedures = TryCreateProceduresDefinition(info.Client.Procedures, clientNames);
        ServerProcedures = TryCreateProceduresDefinition(info.Server.Procedures, serverNames);

        Client = new EndPoint
        (
            EndPointTypeId.Client,
            ref info.Client,
            this,
            clientNames
        );
        
        Server = new EndPoint
        (
            EndPointTypeId.Server,
            ref info.Server,
            this,
            serverNames
        );
    }

}