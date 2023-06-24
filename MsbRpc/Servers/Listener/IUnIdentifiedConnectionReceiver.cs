using MsbRpc.Messaging;

namespace MsbRpc.Servers.Listener;

public interface IUnIdentifiedConnectionReceiver
{
    internal void AcceptUnIdentified(Messenger messenger);
}