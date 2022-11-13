using JetBrains.Annotations;
using MsbRpc.Messaging;
using MsbRpc.Serialization;
using MsbRpc.Serialization.ByteArraySegment;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc;

public class SequentialListener
{
    private readonly RecycledBuffer _buffer;
    private readonly Messenger _messenger;
    private readonly ISequentialRpcReceiver _receiver;

    private SequentialListener(Messenger messenger, ISequentialRpcReceiver receiver, int initialBufferSize)
    {
        if (initialBufferSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialBufferSize), initialBufferSize, "must be positive");
        }

        _buffer = new RecycledBuffer(initialBufferSize);

        _messenger = messenger;
        _receiver = receiver;
    }

    [PublicAPI]
    public Task<Messenger.ListenReturnCode> ListenAsync(CancellationToken cancellationToken)
        => _messenger.ListenAsync(Allocate, Receive, cancellationToken);

    [PublicAPI]
    public static Task<Messenger.ListenReturnCode> ListenAsync
    (
        Messenger messenger,
        ISequentialRpcReceiver receiver,
        int initialBufferSize,
        CancellationToken cancellationToken
    )
    {
        SequentialListener listener = new(messenger, receiver, initialBufferSize);
        return listener.ListenAsync(cancellationToken);
    }

    [PublicAPI]
    public static Task<Messenger.ListenReturnCode> ListenAsync
    (
        Messenger messenger,
        ISequentialRpcReceiver receiver,
        CancellationToken cancellationToken
    )
        => ListenAsync(messenger, receiver, 0, cancellationToken);

    private ArraySegment<byte> Allocate(int count) => _buffer.Get(count);

    private async Task Receive(ArraySegment<byte> message, CancellationToken cancellationToken)
    {
        int procedureId = message.ReadInt32();
        ArraySegment<byte> arguments = message.GetSubSegment(PrimitiveSerializer.Int32Size);
        ArraySegment<byte> result = _receiver.Receive(procedureId, arguments, _buffer);
        await _messenger.SendMessageAsync(result, cancellationToken);
    }
}