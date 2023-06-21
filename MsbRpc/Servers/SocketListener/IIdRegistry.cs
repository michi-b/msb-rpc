namespace MsbRpc.Servers.SocketListener;

public interface IIdRegistry<TItem>
{
    public int Push(TItem item, int maxRetries);
    public bool TryPop(int id, out TItem item);
}