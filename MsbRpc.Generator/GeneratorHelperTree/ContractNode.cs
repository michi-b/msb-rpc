using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GeneratorHelperTree;

public class ContractNode
{
    public readonly ContractNames Names;
    public readonly EndPoint Client;
    public readonly EndPoint Server;
    public readonly TypeCache TypeCache;
    
    public ContractNode(ref ContractInfo info)
    {
        Names = new ContractNames(info.Namespace, info.InterfaceName);
        
        TypeCache = new TypeCache();
        
        ProcedureNode[] clientProcedures = info.Client.InboundProcedures.Select(GetProcedureNode).ToArray();
        ProcedureNode[] serverProcedures = info.Server.InboundProcedures.Select(GetProcedureNode).ToArray();
        
        Client = new EndPoint(this, ref info.Client, clientProcedures, ref info.Server, serverProcedures);
        Server = new EndPoint(this, ref info.Server, serverProcedures, ref info.Client, clientProcedures);
    }

    private ProcedureNode GetProcedureNode(ProcedureInfo info) => new ProcedureNode(this, ref info, TypeCache);
}

