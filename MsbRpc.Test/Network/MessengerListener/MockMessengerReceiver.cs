using System.Collections.Concurrent;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listener;

namespace MsbRpc.Test.Network.MessengerListener;

public class MockMessengerReceiver : IMessengerReceiver
{
    private readonly ConcurrentQueue<Messenger> _messengers = new ConcurrentQueue<Messenger>();
    
    public ConcurrentQueue<Messenger> Messengers => _messengers;

    public void AcceptUnIdentified(Messenger messenger)
    {
        _messengers.Enqueue(messenger);
    }
}