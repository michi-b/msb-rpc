using System.Net.Sockets;
using JetBrains.Annotations;
using MsbRpc.Messaging;

namespace MsbRpc.EndPoints;

public abstract class RpcEndPoint
{
    [PublicAPI] public const int DefaultBufferSize = 1024;

    protected readonly Messenger Messenger;
    private byte[] _buffer;

    protected RpcEndPoint(Socket connectedSocket, int initialBufferSize = DefaultBufferSize)
    {
        Messenger = new Messenger(connectedSocket);
        _buffer = new byte[initialBufferSize];
    }

    /// <summary>
    ///     gets the recycled buffer that can be used to send data through the messenger
    /// </summary>
    /// <param name="count">required minimum size of the buffer</param>
    protected ArraySegment<byte> BorrowBuffer(int count)
    {
        if (_buffer.Length < count)
        {
            _buffer = new byte[count];
        }

        return new ArraySegment<byte>(_buffer, 0, count);
    }
}