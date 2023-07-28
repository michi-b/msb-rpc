namespace MsbRpc.Servers.Listeners.Concurrent;

public interface IConcurrentIdentifiedItemRegistry<TId, TItem>
{
    TId Add(TItem item);
    TItem Take(TId id);
}