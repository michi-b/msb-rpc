#region

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Disposable;
using MsbRpc.Exceptions;
using MsbRpc.Logging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Sockets;

#endregion

namespace MsbRpc.Messaging;

[PublicAPI]
public class Messenger : MarkedDisposable
{
    private const int CountSize = PrimitiveSerializer.IntSize;
    private static readonly ReceiveResult ClosedConnectionReceiveResult = new(Message.Empty, ReceiveReturnCode.ConnectionClosed);
    private static readonly ReceiveResult DisposedConnectionReceiveResult = new(Message.Empty, ReceiveReturnCode.ConnectionDisposed);
    private static readonly ReceiveResult EmptyReceiveResult = new(Message.Empty, ReceiveReturnCode.Success);
    private readonly RpcSocket _socket;

    public bool IsConnected => _socket.IsConnected;

    public Messenger(Socket socket) => _socket = new RpcSocket(socket);

    public Messenger(RpcSocket socket) => _socket = socket;

    public ListenReturnCode Listen(RpcBuffer buffer, Func<Message, bool> receive)
    {
        while (true)
        {
            ReceiveResult receiveResult = Receive(buffer);
            switch (receiveResult.ReturnCode)
            {
                case ReceiveReturnCode.Success:
                    if (receive(receiveResult.Message))
                    {
                        return ListenReturnCode.OperationDiscontinued;
                    }

                    break;
                case ReceiveReturnCode.ConnectionClosed:
                    return ListenReturnCode.ConnectionClosed;
                case ReceiveReturnCode.ConnectionDisposed:
                    return ListenReturnCode.ConnectionDisposed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public async ValueTask<ListenReturnCode> ListenAsync(RpcBuffer buffer, Func<Message, bool> receive, CancellationToken cancellationToken)
    {
        while (true)
        {
            ReceiveResult receiveResult = await ReceiveAsync(buffer);
            switch (receiveResult.ReturnCode)
            {
                case ReceiveReturnCode.Success:
                    if (receive(receiveResult.Message))
                    {
                        return ListenReturnCode.OperationDiscontinued;
                    }

                    break;
                case ReceiveReturnCode.ConnectionClosed:
                    return ListenReturnCode.ConnectionClosed;
                case ReceiveReturnCode.ConnectionDisposed:
                    return ListenReturnCode.ConnectionDisposed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    /// <throws>SocketReceiveException</throws>
    public ReceiveResult Receive(RpcBuffer buffer)
    {
        try
        {
            if (TryReceiveCount(buffer, out int count))
            {
                Message message = buffer.GetMessage(count);

                if (count == 0)
                {
                    return EmptyReceiveResult;
                }

                _socket.ReceiveAll(message.Buffer);

                return new ReceiveResult(message, ReceiveReturnCode.Success);
            }
        }
        catch (ObjectDisposedException)
        {
            if (IsDisposed)
            {
                return DisposedConnectionReceiveResult;
            }

            throw;
        }

        return ClosedConnectionReceiveResult;
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketReceiveException</throws>
    /// <throws>ConnectionClosedException</throws>
    /// <throws>ConnectionDisposedException</throws>
    public async ValueTask<Message> ReceiveMessageAsync(RpcBuffer buffer)
    {
        ReceiveResult receiveResult = await ReceiveAsync(buffer);
        return receiveResult.ReturnCode switch
        {
            ReceiveReturnCode.Success => receiveResult.Message,
            ReceiveReturnCode.ConnectionClosed => throw new ConnectionClosedException(_socket),
            ReceiveReturnCode.ConnectionDisposed => throw new ConnectionDisposedException(_socket),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketReceiveException</throws>
    [PublicAPI]
    public async ValueTask<ReceiveResult> ReceiveAsync(RpcBuffer buffer)
    {
        try
        {
            ArraySegment<byte> countSegment = buffer.Get(PrimitiveSerializer.IntSize);
            if (await _socket.ReceiveAllAsync(countSegment))
            {
                int count = countSegment.ReadInt();

                if (count == 0)
                {
                    return EmptyReceiveResult;
                }

                Message message = buffer.GetMessage(count);

                await _socket.ReceiveAllAsync(message.Buffer);

                return new ReceiveResult(message, ReceiveReturnCode.Success);
            }
        }
        catch (ObjectDisposedException)
        {
            if (IsDisposed)
            {
                return DisposedConnectionReceiveResult;
            }

            throw;
        }

        return ClosedConnectionReceiveResult;
    }

    /// <throws>OperationCanceledException</throws>
    /// <throws>SocketSendException</throws>
    [PublicAPI]
    public async ValueTask SendAsync(Message message)
    {
        await _socket.SendAsync(message.GetFullMessageBuffer());
    }

    /// <throws>SocketSendException</throws>
    public void Send(Message message)
    {
        _socket.Send(message.GetFullMessageBuffer());
    }

    public static async Task<Messenger> ConnectAsync(IPEndPoint remoteEndPoint, ILogger? logger = null)
    {
        Socket socket = new(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            await socket.ConnectAsync(remoteEndPoint);
        }
        catch (Exception exception)
        {
            if (logger != null)
            {
                LogConnectionFailed(logger, remoteEndPoint, exception);
            }

            throw;
        }

        return new Messenger(new RpcSocket(socket));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _socket.Dispose();
        }
    }

    private ReceiveResult Receive(Message message)
    {
        ArraySegment<byte> buffer = message.Buffer;

        if (buffer.Count == 0)
        {
            return EmptyReceiveResult;
        }

        _socket.ReceiveAll(buffer);

        return new ReceiveResult
        (
            message,
            ReceiveReturnCode.Success
        );
    }

    private async ValueTask<ReceiveResult> ReceiveAsync(Message message)
    {
        ArraySegment<byte> buffer = message.Buffer;

        if (buffer.Count == 0)
        {
            return EmptyReceiveResult;
        }

        await _socket.ReceiveAllAsync(buffer);

        return new ReceiveResult
        (
            message,
            ReceiveReturnCode.Success
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

    private static void LogConnectionFailed(ILogger logger, IPEndPoint endPoint, Exception exception)
    {
        if (logger.IsEnabled(LogLevel.Error))
        {
            logger.LogError(LogEventIds.MessengerConnectionFailed, exception, "Failed to connect messenger to remote endpoint {Endpoint}", endPoint);
        }
    }
}