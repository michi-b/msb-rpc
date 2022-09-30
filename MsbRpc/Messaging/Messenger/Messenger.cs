using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using JetBrains.Annotations;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Messaging.Messenger;

[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class Messenger : IDisposable
{
    [PublicAPI] public const int DefaultCapacity = 1024;

    private readonly byte[] _countBuffer;
    private readonly ArraySegment<byte> _countBufferSegment;

    private readonly byte[] _countBytes = new byte[PrimitiveSerializer.Int32Size];
    private readonly Socket _socket;
    private bool _disposed;
    private PrimitiveSerializer _primitiveSerializer;

    /// <param name="connectedSocket">
    ///     a connected socket that this wrapper can take ownership of
    ///     (it may no longer be used otherwise and will be disposed along with the wrapper)
    /// </param>
    /// <exception cref="ArgumentException">if the socket is not connected</exception>
    public Messenger(Socket connectedSocket)
    {
        Debug.Assert(connectedSocket.Connected, "socket needs to be connected for this constructor");
        
        _countBuffer = new byte[PrimitiveSerializer.Int32Size];
        _countBufferSegment = new ArraySegment<byte>(_countBytes);
        _socket = connectedSocket;
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
    public async Task<SendMessageReturnCode> SendMessageAsync(byte[] message) => await SendMessageAsync(new ArraySegment<byte>(message));

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [PublicAPI]
    public async Task<SendMessageReturnCode> SendMessageAsync(ArraySegment<byte> message)
    {
        int messageLength = message.Count;

        _primitiveSerializer.WriteInt32(messageLength, _countBuffer);

        int sendCountSendCount = await _socket.SendAsync(_countBufferSegment, SocketFlags.None);

        if (sendCountSendCount != PrimitiveSerializer.Int32Size)
        {
            return SendMessageReturnCode.Fail;
        }

        int sendMessageSendCount = await _socket.SendAsync(message, SocketFlags.None);

        return sendMessageSendCount == messageLength ? SendMessageReturnCode.Success : SendMessageReturnCode.Fail;
    }

    [PublicAPI]
    public async Task<ReceiveMessageResult> ReceiveMessageAsync()
    {
        switch (await ReceiveFixedLengthAsync(_countBuffer))
        {
            case ReceiveFixedSizeReturnCode.Success:
                break;
            case ReceiveFixedSizeReturnCode.ConnectionClosed:
                return new ReceiveMessageResult(Array.Empty<byte>(), ReceiveMessageReturnCode.ConnectionClosed);
            case ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly:
                return new ReceiveMessageResult(Array.Empty<byte>(), ReceiveMessageReturnCode.ConnectionClosedUnexpectedly);
            default:
                throw new ArgumentOutOfRangeException();
        }

        int messageLength = PrimitiveSerializer.ReadInt32(_countBuffer);

        byte[] bytes = new byte[messageLength];

        if (messageLength == 0)
        {
            return new ReceiveMessageResult(bytes, ReceiveMessageReturnCode.Success);
        }

        return new ReceiveMessageResult
        (
            bytes,
            await ReceiveFixedLengthAsync(bytes) switch
            {
                ReceiveFixedSizeReturnCode.Success => ReceiveMessageReturnCode.Success,
                ReceiveFixedSizeReturnCode.ConnectionClosed => ReceiveMessageReturnCode.ConnectionClosedUnexpectedly,
                ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly => ReceiveMessageReturnCode.ConnectionClosedUnexpectedly,
                _ => throw new ArgumentOutOfRangeException()
            }
        );
    }

    private async Task<ReceiveFixedSizeReturnCode> ReceiveFixedLengthAsync(byte[] buffer)
    {
        int receivedCount = 0;
        int length = buffer.Length;
        while (receivedCount < length)
        {
            int totalRemainingCount = length - receivedCount;
            var arraySegment = new ArraySegment<byte>(buffer, receivedCount, totalRemainingCount);
            int count = await _socket.ReceiveAsync(arraySegment, SocketFlags.None);

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

    private enum ReceiveFixedSizeReturnCode
    {
        Success = 0,
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2
    }
}