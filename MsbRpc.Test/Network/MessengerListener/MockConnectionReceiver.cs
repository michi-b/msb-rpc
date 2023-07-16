#region

using System.Collections.Concurrent;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listener;

#endregion

namespace MsbRpc.Test.Network.MessengerListener;

public class MockConnectionReceiver : IConnectionReceiver
{
    public ConcurrentQueue<Messenger> Messengers { get; } = new();

    public void AcceptUnIdentified(Messenger messenger)
    {
        Messengers.Enqueue(messenger);
    }
}