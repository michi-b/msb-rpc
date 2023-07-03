using MsbRpc.Messaging;
using MsbRpc.Servers.Listener;

namespace MsbRpc.Test.Network.MessengerListener;

/// <summary>
/// a connection receiver that immediately closes received connections
/// </summary>
public class NullMessengerReceiver : IMessengerReceiver
{
    public void AcceptUnIdentified(Messenger messenger)
    {
        messenger.Dispose();
    }
}