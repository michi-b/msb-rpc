using System.Collections.Immutable;

namespace MsbRpc.Generator.Info;

public readonly struct EndPointInfo : IEquatable<EndPointInfo>
{
    public ImmutableArray<ProcedureInfo> InboundProcedures { get; }

    public EndPointInfo(ImmutableArray<ProcedureInfo> inboundProcedures) => InboundProcedures = inboundProcedures;

    public bool Equals(EndPointInfo other) => InboundProcedures.Equals(other.InboundProcedures);

    public override bool Equals(object? obj) => obj is EndPointInfo other && Equals(other);

    public override int GetHashCode() => InboundProcedures.GetHashCode();
    
    public bool HasInboundProcedures => InboundProcedures.Length > 0;
}