using MsbRpc.Messaging;

namespace MsbRpc.Servers.Listener;

public interface IConnectionReceiver
{
    public void AcceptUnIdentified(Messenger messenger);
}