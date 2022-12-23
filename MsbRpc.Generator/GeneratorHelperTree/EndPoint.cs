using System.Collections.Immutable;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GeneratorHelperTree;

public readonly struct EndPoint
{
    public readonly ContractNode Contract;
    public readonly ProcedureNode[] InboundProcedures;
    public readonly EndPointNames Names;
    public readonly ProcedureNode[] OutboundProcedures;

    public EndPoint
    (
        ContractNode contract,
        ref EndPointInfo info,
        ProcedureNode[] inboundProcedures,
        ref EndPointInfo remoteInfo,
        ProcedureNode[] outboundProcedures
    )
    {
        Contract = contract;
        InboundProcedures = inboundProcedures;
        OutboundProcedures = outboundProcedures;
        Names = new EndPointNames(this);
    }
}