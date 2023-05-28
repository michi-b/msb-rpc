using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MsbRpc.Disposable;
using MsbRpc.Sockets.Exceptions;

namespace MsbRpc.Sockets;

[PublicAPI]
public class RpcSocket : MarkedDisposable
{
    private readonly Socket _socket;
    public readonly int Port;

    public bool IsConnected => _socket.Connected;

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
        if ((socketError != SocketError.Success || bytesSent != bytes.Count) && !IsDisposed)
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

        if (IsDisposed)
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
        if (error != SocketError.Success && !IsDisposed)
        {
            throw new RpcSocketReceiveException(this, buffer.Count, count);
        }

        return count;
    }

    /// <exception cref="RpcSocketSendException"></exception>
    public async ValueTask SendAsync(ArraySegment<byte> bytes)
    {
        try
        {
            int bytesSent = await _socket.SendAsync(bytes, SocketFlags.None);
            if (bytesSent != bytes.Count)
            {
                throw new RpcSocketSendException(this, bytes.Count, bytesSent);
            }
        }
        catch (Exception exception)
        {
            throw new RpcSocketSendException(this, exception);
        }
    }

    /// <exception cref="RpcSocketReceiveException"></exception>
    public async ValueTask<bool> ReceiveAllAsync(ArraySegment<byte> buffer)
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

    /// <summary>
    ///     disposes the underlying socket
    /// </summary>
    /// <param name="disposing"></param>
    [PublicAPI]
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _socket.Dispose();
        }
    }

    /// <exception cref="RpcSocketReceiveException"></exception>
    private async ValueTask<int> ReceiveAsync(ArraySegment<byte> buffer)
    {
        try
        {
            return await _socket.ReceiveAsync(buffer, SocketFlags.None);
        }
        catch (Exception exception)
        {
            throw new RpcSocketReceiveException(this, exception);
        }
    }

    private void CancelOperation()
    {
        Dispose();
    }
}