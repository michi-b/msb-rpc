using MsbRpc.EndPoints;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree.Names;

namespace MsbRpc.Generator.GenerationTree;

public class EndPoint
{
    public readonly ProcedureCollection? InboundProcedures;
    public readonly string InitialDirectionEnumValue;
    public readonly EndPointNames Names;
    public readonly ProcedureCollection? OutboundProcedures;

    public EndPoint
    (
        EndPointTypeId type,
        ContractNode contract,
        EndPointNames names
    )
    {
        Names = names;
        EndPointDirection initialDirection = type.GetInitialDirection();
        InitialDirectionEnumValue = initialDirection.GetEnumValueCode();

        InboundProcedures = type switch
        {
            EndPointTypeId.Client => contract.ClientProcedures,
            EndPointTypeId.Server => contract.ServerProcedures,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        OutboundProcedures = type switch
        {
            EndPointTypeId.Client => contract.ServerProcedures,
            EndPointTypeId.Server => contract.ClientProcedures,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}