using JetBrains.Annotations;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Sockets;

namespace MsbRpc.Messaging;

public class Messenger : IDisposable
{
    public enum ListenReturnCode
    {
        ConnectionClosed,
        OperationCanceled
    }

    private const int CountSize = PrimitiveSerializer.Int32Size;
    private readonly ArraySegment<byte> _receiveCountSegment = BufferUtility.Create(CountSize);
    private readonly ArraySegment<byte> _sendCountSegment = BufferUtility.Create(CountSize);
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
    /// <throws>SocketReceiveException</throws>
    [PublicAPI]
    public async Task<ListenReturnCode> ListenAsync
        (RecycledBuffer buffer, Func<ArraySegment<byte>, CancellationToken, Task> receiveAsync, CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ReceiveMessageResult receiveResult = await ReceiveMessageAsync(buffer, cancellationToken);
                switch (receiveResult.ReturnCode)
                {
                    case ReceiveMessageReturnCode.Success:
                        await receiveAsync(receiveResult.Message, cancellationToken);
                        break;
                    case ReceiveMessageReturnCode.ConnectionClosed:
                        return ListenReturnCode.ConnectionClosed;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return ListenReturnCode.OperationCanceled;
        }
        catch (OperationCanceledException)
        {
            return ListenReturnCode.OperationCanceled;
        }
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketReceiveException</throws>
    [PublicAPI]
    public async Task<ReceiveMessageResult> ReceiveMessageAsync(RecycledBuffer buffer, CancellationToken cancellationToken)
    {
        bool hasReceivedCount = await _socket.ReceiveAllAsync(_receiveCountSegment, cancellationToken);
        if (!hasReceivedCount)
        {
            return new ReceiveMessageResult(BufferUtility.Empty, ReceiveMessageReturnCode.ConnectionClosed);
        }

        int messageLength = _receiveCountSegment.ReadInt32();

        ArraySegment<byte> bytesSegment = buffer.Get(messageLength);

        if (messageLength == 0)
        {
            return new ReceiveMessageResult(bytesSegment, ReceiveMessageReturnCode.Success);
        }

        await _socket.ReceiveAllAsync(bytesSegment, cancellationToken);

        return new ReceiveMessageResult
        (
            bytesSegment,
            ReceiveMessageReturnCode.Success
        );
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketSendException</throws>
    [PublicAPI]
    public async Task SendMessageAsync(ArraySegment<byte> message, CancellationToken cancellationToken)
    {
        _sendCountSegment.WriteInt32(message.Count);
        await _socket.SendAsync(_sendCountSegment, cancellationToken);
        await _socket.SendAsync(message, cancellationToken);
    }
}