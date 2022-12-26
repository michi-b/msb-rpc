using System.Collections.Immutable;

namespace MsbRpc.Generator.Info;

public readonly struct EndPointInfo : IEquatable<EndPointInfo>
{
    public ImmutableArray<ProcedureInfo> Procedures { get; }

    public EndPointInfo(ImmutableArray<ProcedureInfo> procedures) => Procedures = procedures;

    public bool Equals(EndPointInfo other) => Procedures.Equals(other.Procedures);

    public override bool Equals(object? obj) => obj is EndPointInfo other && Equals(other);

    public override int GetHashCode() => Procedures.GetHashCode();

    public bool HasInboundProcedures => Procedures.Length > 0;
}