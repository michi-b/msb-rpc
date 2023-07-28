#region

using MsbRpc.Serialization.Primitives;

#endregion

namespace MsbRpc.Servers.Listeners.Concurrent;

public class IntIdentifiedItemRegistry<TItem> : ConcurrentIdentifiedItemRegistry<int, TItem>
{
    private int _nextId;
    protected override int IdSize { get; } = PrimitiveSerializer.IntSize;
    protected override int AcquireNewId() => _nextId++;
}