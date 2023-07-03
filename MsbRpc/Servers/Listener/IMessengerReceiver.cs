using MsbRpc.Messaging;

namespace MsbRpc.Servers.Listener;

public interface IMessengerReceiver
{
    public void AcceptUnIdentified(Messenger messenger);
}