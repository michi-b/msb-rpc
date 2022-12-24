using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class EndPoint
{
    public readonly ContractNode Contract;
    public readonly EndPointNames Names;
    public readonly EndPointTypeId Type;
    
    public EndPoint
    (
        EndPointTypeId type,
        ref EndPointInfo info,
        ContractNode contract,
        EndPointNames names
    )
    {
        Contract = contract;
        Names = names;
        Type = type;
    }

    public bool TryGetInbound(out ProcedureCollection? procedures) => Contract.TryGetProcedures(Type, out procedures);
    
    public bool TryGetOutboundProcedures(out ProcedureCollection? procedures) => Contract.TryGetProcedures(Type.GetOther(), out procedures);
}