#region

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Disposable;
using MsbRpc.Logging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Servers.Listener;
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
    public readonly int Port;

    public bool IsConnected => _socket.IsConnected;

    public Messenger(Socket socket)
    {
        _socket = new RpcSocket(socket);
        Port = ((IPEndPoint)socket.RemoteEndPoint).Port;
    }

    public Messenger(RpcSocket socket)
    {
        _socket = socket;
        Port = socket.Port;
    }

    public static async ValueTask<Messenger> ConnectAsync
    (
        IPAddress ipAddress,
        int port,
        RpcBuffer buffer,
        ILoggerFactory? loggerFactory = null
    )
    {
        IPEndPoint serverEndPoint = new(ipAddress, port);
        return await ConnectAsync(serverEndPoint, buffer, loggerFactory);
    }

    public static async ValueTask<Messenger> ConnectAsync
    (
        IPEndPoint serverEndPoint,
        RpcBuffer buffer,
        ILoggerFactory? loggerFactory = null
    )
    {
        Messenger result = await ConnectAsync(serverEndPoint, loggerFactory);

        await result.SendConnectionRequest(ConnectionRequest.UnIdentified, buffer);

        return result;
    }

    public static ValueTask<Messenger> ConnectAsync
    (
        IPAddress ipAddress,
        int port,
        int identifiedConnectionRequestId,
        RpcBuffer buffer,
        ILoggerFactory? loggerFactory = null
    )
        => ConnectAsync(new IPEndPoint(ipAddress, port), identifiedConnectionRequestId, buffer, loggerFactory);

    public static async ValueTask<Messenger> ConnectAsync
    (
        IPEndPoint serverEndPoint,
        int identifiedConnectionRequestId,
        RpcBuffer buffer,
        ILoggerFactory? loggerFactory = null
    )
    {
        Messenger result = await ConnectAsync(serverEndPoint, loggerFactory);

        await result.SendConnectionRequest(new ConnectionRequest(identifiedConnectionRequestId), buffer);

        return result;
    }

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

    private static async Task<Messenger> ConnectAsync(IPEndPoint serverEndPoint, ILoggerFactory? loggerFactory)
    {
        Socket socket = new(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            await socket.ConnectAsync(serverEndPoint);
        }
        catch (Exception exception)
        {
            if (loggerFactory != null)
            {
                LogConnectionFailed(loggerFactory.CreateLogger<Messenger>(), serverEndPoint, exception);
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