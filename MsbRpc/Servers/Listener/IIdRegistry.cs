namespace MsbRpc.Servers.Listener;

public interface IIdRegistry<TItem>
{
    public int Push(TItem item, int maxRetries);
    public bool TryPop(int id, out TItem item);
}