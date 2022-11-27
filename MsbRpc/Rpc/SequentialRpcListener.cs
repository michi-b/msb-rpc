using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Rpc;

public class SequentialRpcListener
{
    private readonly RecycledBuffer _buffer;
    private readonly Messenger _messenger;
    private readonly ISequentialRpcReceiver _rpcReceiver;

    public SequentialRpcListener(Messenger messenger, ISequentialRpcReceiver rpcReceiver, int initialBufferSize = BufferUtility.DefaultSize)
    {
        _messenger = messenger;
        _rpcReceiver = rpcReceiver;
        _buffer = new RecycledBuffer(initialBufferSize);
    }

    public async Task<Messenger.ListenReturnCode> ListenAsync(CancellationToken cancellationToken)
        => await _messenger.ListenAsync(_buffer, ReceiveAsync, cancellationToken);

    private async Task ReceiveAsync(ArraySegment<byte> message, CancellationToken cancellationToken)
    {
        int id = message.ReadInt32();
        ArraySegment<byte> result = _rpcReceiver.Receive(id, message.GetSubSegment(PrimitiveSerializer.Int32Size), _buffer);
        await _messenger.SendMessageAsync(result, cancellationToken);
    }
}