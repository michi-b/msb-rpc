using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using JetBrains.Annotations;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Messaging;

[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class Messenger : IDisposable
{
    private readonly byte[] _receiveCountBytes = new byte[PrimitiveSerializer.Int32Size];
    private readonly ArraySegment<byte> _receiveCountBytesSegment;
    private readonly byte[] _sendCountBytes = new byte[PrimitiveSerializer.Int32Size];

    private readonly Socket _socket;
    private bool _disposed;

    /// <param name="connectedSocket">
    ///     a connected socket that this wrapper can take ownership of
    ///     (it may no longer be used otherwise and will be disposed along with the wrapper)
    /// </param>
    /// <exception cref="ArgumentException">if the socket is not connected</exception>
    public Messenger(Socket connectedSocket)
    {
        Debug.Assert(connectedSocket.Connected, "socket needs to be connected for this constructor");

        _socket = connectedSocket;
        _receiveCountBytesSegment = new ArraySegment<byte>(_receiveCountBytes);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _socket.Dispose();

        _disposed = true;
    }

    [PublicAPI]
    public void SendMessage(ref ArraySegment<byte> message)
    {
        SendCount(message.Count);
        Send(ref message);
    }

    [PublicAPI]
    public async Task<ReceiveMessageResult> ReceiveMessageAsync(Func<int, byte[]> allocate)
    {
        switch (await ReceiveFixedLengthAsync(_receiveCountBytesSegment))
        {
            case ReceiveFixedSizeReturnCode.Success:
                break;
            case ReceiveFixedSizeReturnCode.ConnectionClosed:
                return new ReceiveMessageResult(Memory.EmptySegment, ReceiveMessageReturnCode.ConnectionClosed);
            case ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly:
                return new ReceiveMessageResult(Memory.EmptySegment, ReceiveMessageReturnCode.ConnectionClosedUnexpectedly);
            default:
                throw new ArgumentOutOfRangeException();
        }

        int messageLength = _receiveCountBytes.ReadInt32();

        byte[] bytes = allocate(messageLength);
        var bytesSegment = new ArraySegment<byte>(bytes, 0, messageLength);

        if (messageLength == 0)
        {
            return new ReceiveMessageResult(bytesSegment, ReceiveMessageReturnCode.Success);
        }

        return new ReceiveMessageResult
        (
            bytesSegment,
            await ReceiveFixedLengthAsync(bytesSegment) switch
            {
                ReceiveFixedSizeReturnCode.Success => ReceiveMessageReturnCode.Success,
                ReceiveFixedSizeReturnCode.ConnectionClosed => ReceiveMessageReturnCode.ConnectionClosedUnexpectedly,
                ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly => ReceiveMessageReturnCode.ConnectionClosedUnexpectedly,
                _ => throw new ArgumentOutOfRangeException()
            }
        );
    }

    private void SendCount(int count)
    {
        _sendCountBytes.WriteInt32(count);
        Send(_sendCountBytes);
    }

    private async Task<ReceiveFixedSizeReturnCode> ReceiveFixedLengthAsync(ArraySegment<byte> bytes)
    {
        int receivedCount = 0;
        int length = bytes.Count;
        while (receivedCount < length)
        {
            int remainingCount = length - receivedCount;
            var remaining = new ArraySegment<byte>(bytes.Array!, bytes.Offset + receivedCount, remainingCount);
            int count = await ReceiveAsync(remaining);
            if (count == 0) //no bytes received means connection closed
            {
                break;
            }

            receivedCount += count;
        }

        return receivedCount == length //order of these checks might be important, as 0 could be the expected indicatedLength
            ? ReceiveFixedSizeReturnCode.Success
            : receivedCount == 0
                ? ReceiveFixedSizeReturnCode.ConnectionClosed
                : ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly;
    }

    private void Send(ref ArraySegment<byte> bytes)
    {
        _socket.Send(bytes.Array!, bytes.Offset, bytes.Count, SocketFlags.None);
    }

    private void Send(byte[] bytes)
    {
        _socket.Send(bytes);
    }

    private Task<int> ReceiveAsync(ArraySegment<byte> bytes) => _socket.ReceiveAsync(bytes, SocketFlags.None);

    private enum ReceiveFixedSizeReturnCode
    {
        Success = 0,
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2
    }
}