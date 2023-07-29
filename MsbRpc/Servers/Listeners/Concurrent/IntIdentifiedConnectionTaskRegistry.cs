#region

using MsbRpc.Serialization.Primitives;

#endregion

namespace MsbRpc.Servers.Listeners.Concurrent;

public class IntIdentifiedConnectionTaskRegistry : ConnectionTaskRegistry<int>
{
    private int _nextId;

    protected override int IdSize { get; } = PrimitiveSerializer.IntSize;
    protected override int AcquireNewId() => _nextId++;
}