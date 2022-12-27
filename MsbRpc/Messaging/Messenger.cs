using JetBrains.Annotations;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Sockets;

namespace MsbRpc.Messaging;

[PublicAPI]
public class Messenger : IDisposable
{
    /// <summary>
    ///     receive delegate for asynchronous listen method
    ///     that is invoked each time a message is received
    /// </summary>
    /// <param name="message">the received message</param>
    /// <returns>true if listening should be continued, otherwise false</returns>
    public delegate ValueTask<bool> ReceiveAsyncDelegate(ArraySegment<byte> message, CancellationToken cancellationToken);

    public delegate bool ReceiveDelegate(ArraySegment<byte> message);

    public enum ListenReturnCode
    {
        /// <summary>
        ///     listening stopped by connection close
        /// </summary>
        ConnectionClosed,

        /// <summary>
        ///     listening stopped because the operation was canceled
        /// </summary>
        OperationCanceled,

        /// <summary>
        ///     listening stopped because the receive delegate returned true
        /// </summary>
        OperationDiscontinued
    }

    private const int CountSize = PrimitiveSerializer.IntSize;
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
    public Task<ListenReturnCode> ListenLongRunning(RecycledBuffer buffer, ReceiveDelegate receive)
    {
        return Task.Factory.StartNew
            (
                () => Listen(buffer, receive),
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default
            );
    }

    public ListenReturnCode Listen(RecycledBuffer buffer, ReceiveDelegate receive)
    {
        while (true)
        {
            ReceiveMessageResult receiveResult = ReceiveMessage(buffer);
            switch (receiveResult.ReturnCode)
            {
                case ReceiveMessageReturnCode.Success:
                    if (receive(receiveResult.Message))
                    {
                        return ListenReturnCode.OperationDiscontinued;
                    }
                    break;
                case ReceiveMessageReturnCode.ConnectionClosed:
                    return ListenReturnCode.ConnectionClosed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketReceiveException</throws>
    [PublicAPI]
    public async Task<ListenReturnCode> ListenAsync(RecycledBuffer buffer, ReceiveAsyncDelegate receiveAsync, CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ReceiveMessageResult receiveResult = await ReceiveMessageAsync(buffer, cancellationToken);
                switch (receiveResult.ReturnCode)
                {
                    case ReceiveMessageReturnCode.Success:
                        if (await receiveAsync(receiveResult.Message, cancellationToken))
                        {
                            return ListenReturnCode.OperationDiscontinued;
                        }

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

    /// <throws>SocketReceiveException</throws>
    public ReceiveMessageResult ReceiveMessage(RecycledBuffer buffer)
    {
        bool hasReceivedCount = _socket.ReceiveAll(_receiveCountSegment);
        if (!hasReceivedCount)
        {
            return new ReceiveMessageResult(BufferUtility.Empty, ReceiveMessageReturnCode.ConnectionClosed);
        }

        int messageLength = _receiveCountSegment.ReadInt();

        ArraySegment<byte> bytesSegment = buffer.Get(messageLength);

        if (messageLength == 0)
        {
            return new ReceiveMessageResult(bytesSegment, ReceiveMessageReturnCode.Success);
        }

        _socket.ReceiveAll(bytesSegment);

        return new ReceiveMessageResult
        (
            bytesSegment,
            ReceiveMessageReturnCode.Success
        );
    }
    
    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketReceiveException</throws>
    [PublicAPI]
    public async ValueTask<ReceiveMessageResult> ReceiveMessageAsync(RecycledBuffer buffer, CancellationToken cancellationToken)
    {
        bool hasReceivedCount = await _socket.ReceiveAllAsync(_receiveCountSegment, cancellationToken);
        if (!hasReceivedCount)
        {
            return new ReceiveMessageResult(BufferUtility.Empty, ReceiveMessageReturnCode.ConnectionClosed);
        }

        int messageLength = _receiveCountSegment.ReadInt();

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
    public async ValueTask SendMessageAsync(ArraySegment<byte> message, CancellationToken cancellationToken)
    {
        _sendCountSegment.WriteInt(message.Count);
        await _socket.SendAsync(_sendCountSegment, cancellationToken);
        await _socket.SendAsync(message, cancellationToken);
    }

    /// <throws>SocketSendException</throws>
    public async void SendMessage(ArraySegment<byte> message)
    {
        _sendCountSegment.WriteInt(message.Count);
        _socket.Send(_sendCountSegment);
        _socket.Send(message);
    }
}