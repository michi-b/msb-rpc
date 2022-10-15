using System.Collections.Concurrent;
using System.Diagnostics;
using JetBrains.Annotations;

namespace MsbRpc.Messaging.Listeners;

public class LazyListener : AListener
{
    private readonly BlockingCollection<ArraySegment<byte>> _available = new(new ConcurrentQueue<ArraySegment<byte>>());

    [PublicAPI] public bool HasMessageAvailable => _available.Count > 0;

    public LazyListener(Messenger messenger) : base(messenger) { }

    [PublicAPI]
    public ArraySegment<byte> ConsumeNextMessage() => _available.Take();

    protected override void ReceiveMessage(ArraySegment<byte> message)
    {
        Debug.Assert(_available != null, nameof(_available) + " != null");
        _available?.Add(message);
    }

    protected override Task<ArraySegment<byte>> Allocate(int count) => Task.FromResult(new ArraySegment<byte>(new byte[count]));
}