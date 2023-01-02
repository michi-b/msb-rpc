using System;
using System.Threading;
using System.Threading.Tasks;
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
    public delegate ValueTask<bool> ReceiveAsyncDelegate(Message message, CancellationToken cancellationToken);

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

    private static readonly ReceiveMessageResult ReceiveMessageConnectionClosedResult = new(Message.Empty, ReceiveMessageReturnCode.ConnectionClosed);
    private static readonly ReceiveMessageResult ReceiveEmptyMessageResult = new(Message.Empty, ReceiveMessageReturnCode.Success);
    
    private const int CountSize = PrimitiveSerializer.IntSize;
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

    public ListenReturnCode Listen(RpcBuffer buffer, Func<Message, bool> receive)
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

    /// <throws>SocketReceiveException</throws>
    public ReceiveMessageResult ReceiveMessage(RpcBuffer buffer)
    {
        if(TryReceiveCount(buffer, out int count))
        {

            Message message = buffer.GetMessage(count);

            if (count == 0)
            {
                return ReceiveEmptyMessageResult;
            }

            _socket.ReceiveAll(message.Buffer);

            return new ReceiveMessageResult ( message, ReceiveMessageReturnCode.Success );
        }

        return ReceiveMessageConnectionClosedResult;
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketReceiveException</throws>
    [PublicAPI]
    public async ValueTask<ReceiveMessageResult> ReceiveMessageAsync(RpcBuffer buffer, CancellationToken cancellationToken)
    {
        if (TryReceiveCount(buffer, out int count))
        {
            Message message = buffer.GetMessage(count);

            if (count == 0)
            {
                return ReceiveEmptyMessageResult;
            }

            await _socket.ReceiveAllAsync(message.Buffer, cancellationToken);

            return new ReceiveMessageResult ( message, ReceiveMessageReturnCode.Success );
        }
        
        return ReceiveMessageConnectionClosedResult;
    }

    private ReceiveMessageResult ReceiveMessage(Message message)
    {
        ArraySegment<byte> buffer = message.Buffer;
        
        if (buffer.Count == 0)
        {
            return ReceiveEmptyMessageResult;
        }
        
        _socket.ReceiveAll(buffer);
        
        return new ReceiveMessageResult
        (
            message,
            ReceiveMessageReturnCode.Success
        );
    }
    
    private async ValueTask<ReceiveMessageResult> ReceiveMessageAsync(Message message, CancellationToken cancellationToken)
    {
        ArraySegment<byte> buffer = message.Buffer;
        
        if (buffer.Count == 0)
        {
            return ReceiveEmptyMessageResult;
        }
        
        await _socket.ReceiveAllAsync(buffer, cancellationToken);
        
        return new ReceiveMessageResult
        (
            message,
            ReceiveMessageReturnCode.Success
        );
    }

    private bool TryReceiveCount(RpcBuffer buffer, out int count)
    {
        ArraySegment<byte> countBuffer = buffer.Get(PrimitiveSerializer.IntSize);
        if (_socket.ReceiveAll(countBuffer))
        {
            count = countBuffer.ReadInt();
            return true;
        }
        count = 0;
        return false;
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketSendException</throws>
    [PublicAPI]
    public async ValueTask SendMessageAsync(Message message, CancellationToken cancellationToken)
    {
        await _socket.SendAsync(message.GetFullMessageBuffer(), cancellationToken);
    }

    /// <throws>SocketSendException</throws>
    public void SendMessage(Message message)
    {
        _socket.Send(message.GetFullMessageBuffer());
    }
}