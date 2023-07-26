namespace MsbRpc.Servers.Listener;

public interface IConcurrentIdentifiedItemRegistry<TId, TItem>
{
    int MaxMessageSize { get; }
    TId Add(TItem item);
    TItem Take(TId id);
}