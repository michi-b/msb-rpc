using JetBrains.Annotations;
using MsbRpc.Serialization;
using MsbRpc.Serialization.ByteArraySegment;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Sockets;

namespace MsbRpc.Messaging;

public class Messenger : IDisposable
{
    private const int CountSize = PrimitiveSerializer.Int32Size;
    private readonly ArraySegment<byte> _receiveCountSegment = Memory.Create(CountSize);
    private readonly ArraySegment<byte> _sendCountSegment = Memory.Create(CountSize);
    private readonly RpcSocket _socket;
    private bool _disposed;

    public Messenger(RpcSocket socket) => _socket = socket;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _socket.Dispose();

        _disposed = true;
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketSendException</throws>
    [PublicAPI]
    public async Task SendMessage(ArraySegment<byte> message, CancellationToken cancellationToken)
    {
        _sendCountSegment.WriteInt32(message.Count);
        await _socket.SendAsync(_sendCountSegment, cancellationToken);
        await _socket.SendAsync(message, cancellationToken);
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketReceiveException</throws>
    [PublicAPI]
    public async Task<ReceiveMessageResult> ReceiveMessageAsync(Func<int, byte[]> allocate, CancellationToken cancellationToken)
    {
        bool hasReceivedCount = await _socket.ReceiveAsync(_receiveCountSegment, cancellationToken);
        if (!hasReceivedCount)
        {
            return new ReceiveMessageResult(Memory.EmptySegment, ReceiveMessageReturnCode.ConnectionClosed);
        }

        int messageLength = _receiveCountSegment.ReadInt32();

        byte[] bytes = allocate(messageLength);
        var bytesSegment = new ArraySegment<byte>(bytes, 0, messageLength);

        if (messageLength == 0)
        {
            return new ReceiveMessageResult(bytesSegment, ReceiveMessageReturnCode.Success);
        }

        await _socket.ReceiveAsync(bytesSegment, cancellationToken);

        return new ReceiveMessageResult
        (
            bytesSegment,
            ReceiveMessageReturnCode.Success
        );
    }
}