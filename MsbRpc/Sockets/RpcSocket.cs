using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MsbRpc.Sockets.Exceptions;

namespace MsbRpc.Sockets;

[PublicAPI]
public class RpcSocket : IRpcSocket
{
    private readonly Socket _socket;
    public readonly int Port;

    private bool _isDisposed;

    public RpcSocket(Socket socket)
    {
        if (socket.ProtocolType != ProtocolType.Tcp
            || socket.SocketType != SocketType.Stream
            || !socket.Connected)
        {
            throw new InvalidRpcSocketConstructorSocketException(socket, nameof(socket));
        }

        var endPoint = socket.LocalEndPoint as IPEndPoint;
        if (endPoint == null)
        {
            throw new InvalidRpcSocketConstructorSocketException(socket, nameof(socket));
        }

        Port = endPoint.Port;

        _socket = socket;
    }

    /// <throws>SocketSendException</throws>
    public void Send(ArraySegment<byte> bytes)
    {
        int bytesSent = _socket.Send(bytes.Array!, bytes.Offset, bytes.Count, SocketFlags.None, out SocketError socketError);
        if ((socketError != SocketError.Success || bytesSent != bytes.Count) && !_isDisposed)
        {
            throw new RpcSocketSendException(this, bytes.Count, bytesSent);
        }
    }

    /// <exception cref="RpcSocketReceiveException"></exception>
    public bool ReceiveAll(ArraySegment<byte> buffer)
    {
        int receivedCount = 0;
        int length = buffer.Count;
        while (receivedCount < length)
        {
            int remainingCount = length - receivedCount;
            var remaining = new ArraySegment<byte>(buffer.Array!, buffer.Offset + receivedCount, remainingCount);
            int count = Receive(remaining);

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

        if (_isDisposed)
        {
            return false;
        }
        //The remote closing the connection before sending an expected amount of bytes? Now THAT's unexpected!

        Dispose();
        throw new RpcSocketReceiveException(this, length, receivedCount);
    }

    public int Receive(ArraySegment<byte> buffer)
    {
        int count = _socket.Receive(buffer.Array!, buffer.Offset, buffer.Count, SocketFlags.None, out SocketError error);
        if (error != SocketError.Success && !_isDisposed)
        {
            throw new RpcSocketReceiveException(this, buffer.Count, count);
        }

        return count;
    }

    /// <inheritdoc cref="IRpcSocket.SendAsync" />
    [PublicAPI]
    public async ValueTask SendAsync(ArraySegment<byte> bytes, CancellationToken cancellationToken)
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

    /// <exception cref="RpcSocketReceiveException">
    ///     Thrown when an error occurs while receiving data,
    ///     for example when some bytes but not all could be read before the connection was closed.
    /// </exception>
    /// <summary>Awaits and fills all bytes of the provided buffer with next bytes read from the socket.</summary>
    /// <param name="buffer">Buffer to receive bytes into.</param>
    /// <param name="cancellationToken">Cancels the operation and disposes the socket.</param>
    /// <returns>
    ///     true if the whole buffer was filled with bytes from the socket,
    ///     false if the connection was closed before any bytes could be read
    /// </returns>
    [PublicAPI]
    public async ValueTask<bool> ReceiveAllAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        using CancellationTokenRegistration cancellationRegistration = RegisterCancellation(cancellationToken);

        try
        {
            return await ReceiveAllAsync(buffer);
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

    /// <exception cref="RpcSocketReceiveException">
    ///     Thrown when an error occurs while receiving data.
    /// </exception>
    /// <summary>Fills the provided buffer with next currently available bytes read from the socket.</summary>
    /// <param name="buffer">Buffer to receive bytes into. It's size limits how much bytes will be received at maximum.</param>
    /// <param name="cancellationToken">Cancels the operation and disposes the socket.</param>
    /// <returns>
    ///     The count of bytes that were received.
    ///     Zero if the connection was closed before any bytes could be read.
    ///     Otherwise any positive number lower than the size of the provided buffer.
    /// </returns>
    [PublicAPI]
    public async ValueTask<int> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        using CancellationTokenRegistration cancellationRegistration = RegisterCancellation(cancellationToken);

        try
        {
            return await ReceiveAsync(buffer);
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
    [PublicAPI]
    public void Dispose()
    {
        if (!_isDisposed)
        {
            GC.SuppressFinalize(this);
            _isDisposed = true;
            _socket.Dispose();
        }
    }

    /// <exception cref="RpcSocketSendException"></exception>
    private async ValueTask SendAsync(ArraySegment<byte> bytes)
    {
        int bytesSent = await _socket.SendAsync(bytes, SocketFlags.None);
        if (bytesSent != bytes.Count)
        {
            Dispose();
            throw new RpcSocketSendException(this, bytes.Count, bytesSent);
        }
    }

    /// <exception cref="RpcSocketReceiveException"></exception>
    private async ValueTask<bool> ReceiveAllAsync(ArraySegment<byte> buffer)
    {
        int receivedCount = 0;
        int length = buffer.Count;
        while (receivedCount < length)
        {
            int remainingCount = length - receivedCount;
            var remaining = new ArraySegment<byte>(buffer.Array!, buffer.Offset + receivedCount, remainingCount);
            int count = await ReceiveAsync(remaining);

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

    private async ValueTask<int> ReceiveAsync(ArraySegment<byte> buffer) => await _socket.ReceiveAsync(buffer, SocketFlags.None);

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