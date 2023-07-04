using System.Collections.Concurrent;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listener;

namespace MsbRpc.Test.Network.MessengerListener;

public class MockMessengerReceiver : IMessengerReceiver
{
    public ConcurrentQueue<Messenger> Messengers { get; } = new ConcurrentQueue<Messenger>();

    public void AcceptUnIdentified(Messenger messenger)
    {
        Messengers.Enqueue(messenger);
    }
}