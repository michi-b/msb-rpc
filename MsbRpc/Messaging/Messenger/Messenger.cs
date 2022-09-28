using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using JetBrains.Annotations;
using MsbRpc.Buffer;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Messaging.Messenger;

[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public abstract class Messenger : IDisposable
{
    [PublicAPI] public const int DefaultCapacity = 1024;

    private readonly ArraySegment<byte> _countBuffer;

    private readonly byte[] _countBytes = new byte[PrimitiveSerializer.Int32Size];
    private readonly Socket _socket;
    private bool _disposed;
    private PrimitiveSerializer _primitiveSerializer;

    /// <param name="connectedSocket">
    ///     a connected socket that this wrapper can take ownership of
    ///     (it may no longer be used otherwise and will be disposed along with the wrapper)
    /// </param>
    /// <param name="buffer">the buffer this messenger uses for memory allocation</param>
    /// <param name="capacity">
    ///     capacity of the internal receive buffer, should be the maximum amount of expected bytes per
    ///     message
    /// </param>
    /// <exception cref="ArgumentException">if the socket is not connected</exception>
    protected Messenger(Socket connectedSocket, IBuffer buffer, int capacity = DefaultCapacity)
    {
        if (!connectedSocket.Connected)
        {
            throw new ArgumentException("socket needs to be connected for this constructor", nameof(connectedSocket));
        }

        _countBuffer = new ArraySegment<byte>(_countBytes);
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

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [PublicAPI]
    public async Task<SendMessageReturnCode> SendMessageAsync(ArraySegment<byte> message)
    {
        int messageLength = message.Count;

        _primitiveSerializer.WriteInt32(messageLength, _countBuffer.Array!);

        int sendCountSendCount = await _socket.SendAsync(_countBuffer, SocketFlags.None);

        if (sendCountSendCount != PrimitiveSerializer.Int32Size)
        {
            return SendMessageReturnCode.Fail;
        }

        int sendMessageSendCount = await _socket.SendAsync(message, SocketFlags.None);

        return sendMessageSendCount == messageLength ? SendMessageReturnCode.Success : SendMessageReturnCode.Fail;
    }

    [PublicAPI]
    protected async Task<ReceiveMessageResult> ReceiveMessageAsync(IBuffer buffer)
    {
        switch (await ReceiveFixedLengthAsync(_countBuffer))
        {
            case ReceiveFixedSizeReturnCode.Success:
                break;
            case ReceiveFixedSizeReturnCode.ConnectionClosed:
                return new ReceiveMessageResult(Memory.Empty, ReceiveMessageReturnCode.ConnectionClosed);
            case ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly:
                return new ReceiveMessageResult(Memory.Empty, ReceiveMessageReturnCode.ConnectionClosedUnexpectedly);
            default:
                throw new ArgumentOutOfRangeException();
        }

        int messageLength = PrimitiveSerializer.ReadInt32(_countBuffer);

        ArraySegment<byte> bytes = buffer.Borrow(messageLength);

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

    private async Task<ReceiveFixedSizeReturnCode> ReceiveFixedLengthAsync(ArraySegment<byte> buffer)
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