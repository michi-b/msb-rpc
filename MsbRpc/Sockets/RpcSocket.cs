using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using MsbRpc.Sockets.Exceptions;

namespace MsbRpc.Sockets;

public partial class RpcSocket : IRpcSocket
{
    private readonly Socket _socket;
    private bool _isDisposed;

    public RpcSocket(Socket socket)
    {
        if (socket.ProtocolType != ProtocolType.Tcp
            || socket.SocketType != SocketType.Stream
            || !socket.Connected)
        {
            throw new InvalidRpcSocketConstructorSocketException(socket, nameof(socket));
        }

        _socket = socket;
    }

    /// <inheritdoc cref="IRpcSocket.SendAsync" />
    public async Task SendAsync(ArraySegment<byte> bytes, CancellationToken cancellationToken)
    {
        using CancellationTokenRegistration cancellationRegistration = RegisterCancellation(cancellationToken);
        try
        {
            await SendAsync(bytes);
        }
        catch (RpcSocketSendException)
        {
            ThrowIfOperationIsCancelled(cancellationToken);
            throw;
        }
        catch (Exception exception)
        {
            ThrowIfOperationIsCancelled(cancellationToken);
            throw new RpcSocketSendException(this, exception);
        }
    }

    /// <inheritdoc cref="IRpcSocket.ReceiveAsync" />
    public async Task<bool> ReceiveAsync(ArraySegment<byte> bytes, CancellationToken cancellationToken)
    {
        using CancellationTokenRegistration cancellationRegistration = RegisterCancellation(cancellationToken);

        try
        {
            return await ReceiveAsync(bytes);
        }
        catch (RpcSocketReceiveException)
        {
            ThrowIfOperationIsCancelled(cancellationToken);
            throw;
        }
        catch (Exception exception)
        {
            ThrowIfOperationIsCancelled(cancellationToken);
            throw new RpcSocketReceiveException(this, exception);
        }
    }

    /// <summary>
    ///     disposes the underlying socket
    ///     this is done on any encountered exception
    ///     harsh, but there is no other way to make the socket operations cancellable
    ///     and it probably means the socket should no longer be used anyway,
    ///     without reconnecting at least, which is not in the scope of this class
    /// </summary>
    public void Dispose()
    {
        if (!_isDisposed)
        {
            _socket.Dispose();
            _isDisposed = true;
        }
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Critical, Message = "Socket is not connected or is not a TCP streaming socket.")]
    static partial void LogMisconfiguredSocket(ILogger logger);

    /// <exception cref="RpcSocketSendException"></exception>
    private async Task SendAsync(ArraySegment<byte> bytes)
    {
        int bytesSent = await _socket.SendAsync(bytes, SocketFlags.None);
        if (bytesSent != bytes.Count)
        {
            Dispose();
            throw new RpcSocketSendException(this, bytes.Count, bytesSent);
        }
    }

    /// <exception cref="RpcSocketReceiveException"></exception>
    private async Task<bool> ReceiveAsync(ArraySegment<byte> bytes)
    {
        int receivedCount = 0;
        int length = bytes.Count;
        while (receivedCount < length)
        {
            int remainingCount = length - receivedCount;
            var remaining = new ArraySegment<byte>(bytes.Array!, bytes.Offset + receivedCount, remainingCount);
            int count = await _socket.ReceiveAsync(remaining, SocketFlags.None);

            if (count == 0) //no bytes received means connection closed
            {
                break;
            }

            receivedCount += count;
        }

        if (receivedCount == length)
        {
            return true;
        }

        if (receivedCount == 0)
        {
            //0 is a sometimes expected result, as it just means that the remote closed the connection
            return false;
        }

        //The remote closing the connection before sending an expected amount of bytes? Now THAT's unexpected!
        Dispose();
        throw new RpcSocketReceiveException(this, length, receivedCount);
    }

    private void ThrowIfOperationIsCancelled(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            Dispose();
            throw new OperationCanceledException("Sending operation was canceled, which closed the socket as a result", cancellationToken);
        }
    }

    private CancellationTokenRegistration RegisterCancellation(CancellationToken cancellationToken) => cancellationToken.Register(CancelOperation);

    private void CancelOperation()
    {
        Dispose();
    }
}