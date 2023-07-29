#region

using System.Collections.Concurrent;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listeners.Connections;

#endregion

namespace MsbRpc.Test.Network.Listeners;

public class MockConnectionReceiver : IConnectionReceiver
{
    public ConcurrentQueue<Messenger> Messengers { get; } = new();

    public void Accept(Messenger messenger)
    {
        Messengers.Enqueue(messenger);
    }
}