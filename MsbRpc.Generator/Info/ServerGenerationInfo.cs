using System;

namespace MsbRpc.Generator.Info;

internal readonly struct ServerGenerationInfo : IEquatable<ServerGenerationInfo>
{
    public bool Equals(ServerGenerationInfo other) => true;

    public override bool Equals(object? obj) => obj is ServerGenerationInfo other && Equals(other);

    public override int GetHashCode() => 0;
}