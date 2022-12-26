using MsbRpc.EndPoints;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.HelperTree.Names;

namespace MsbRpc.Generator.HelperTree;

public class EndPoint
{
    private readonly EndPointTypeId _type;
    public readonly ContractNode Contract;
    public readonly string InitialDirectionEnumValue;
    public readonly EndPointNames Names;

    public EndPoint
    (
        EndPointTypeId type,
        ContractNode contract,
        EndPointNames names
    )
    {
        Contract = contract;
        Names = names;
        _type = type;
        EndPointDirection initialDirection = type.GetInitialDirection();
        InitialDirectionEnumValue = initialDirection.GetEnumValueCode();
    }

    public bool TryGetInboundProcedures(out ProcedureCollection? procedures) => Contract.TryGetProcedures(_type, out procedures);

    public bool TryGetOutboundProcedures(out ProcedureCollection? procedures) => Contract.TryGetProcedures(_type.GetOther(), out procedures);
}