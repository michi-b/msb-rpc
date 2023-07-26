using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Servers.Listener;

public class RunningIntegerIdentifiedItemRegistry<TItem> : ConcurrentIdentifiedItemRegistry<int, TItem>
{
    private int _nextId;
    protected override int IdSize { get; } = PrimitiveSerializer.IntSize;
    protected override int AcquireNewId() => _nextId++;
}