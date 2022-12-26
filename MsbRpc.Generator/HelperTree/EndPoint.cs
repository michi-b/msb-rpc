using MsbRpc.EndPoints;
using MsbRpc.Generator.Attributes;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationHelpers.Extensions;
using MsbRpc.Generator.HelperTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree;

public class EndPoint
{
    public readonly ContractNode Contract;
    public readonly EndPointNames Names;
    public readonly EndPointTypeId Type;
    public readonly EndPointDirection InitialDirection;
    public readonly string InitialDirectionEnumValue;

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
        InitialDirection = type.GetInitialDirection();
        InitialDirectionEnumValue = InitialDirection.GetEnumValueCode();
    }

    public bool TryGetInboundProcedures(out ProcedureCollection? procedures) => Contract.TryGetProcedures(Type, out procedures);
    
    public bool TryGetOutboundProcedures(out ProcedureCollection? procedures) => Contract.TryGetProcedures(Type.GetOther(), out procedures);
}