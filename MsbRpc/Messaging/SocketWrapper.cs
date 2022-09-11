using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Messaging;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public abstract class SocketWrapper : IDisposable
{
    public const int DefaultCapacity = 1024;


    private readonly byte[] _countBuffer = new byte[PrimitiveSerializer.Int32Size];
    private readonly Socket _socket;
    private byte[] _buffer;
    private PrimitiveSerializer _primitiveSerializer;
    private bool _disposed;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _socket.Dispose();

        _disposed = true;
    }

    /// <param name="connectedSocket">
    /// a connected socket that this wrapper can take ownership of
    /// (it may no longer be used otherwise and will be disposed along with the wrapper)
    /// </param>
    /// <param name="capacity">capacity of the internal receive buffer, should be the maximum amount of expected bytes per message</param>
    /// <exception cref="ArgumentException">if the socket is not connected</exception>
    internal SocketWrapper(Socket connectedSocket, int capacity = DefaultCapacity)
    {
        if (!connectedSocket.Connected)
        {
            throw new ArgumentException("socket needs to be connected for this constructor", nameof(connectedSocket));
        }
        
        _buffer = new byte[capacity];
        _socket = connectedSocket;
    }

    internal void Reserve(int count)
    {
        if (_buffer.Length < count)
        {
            _buffer = new byte[count];
        }
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [PublicAPI]
    internal async Task<SendMessageReturnCode> SendMessageAsync(int length) => await SendMessageAsync(length, length);

    /// <summary>
    ///     do not use!!! exposed for testing unexpected connection loss only
    /// </summary>
    /// <param name="indicatedLength">value of the length header of the message</param>
    /// <param name="actualLength">how many bytes are actually sent</param>
    /// <see cref="SendMessageAsync(int)" />
    internal async Task<SendMessageReturnCode> SendMessageAsync(int indicatedLength, int actualLength)
    {
        _primitiveSerializer.WriteInt32(indicatedLength, _countBuffer);

        int sentCount = await _socket.SendAsync(new ArraySegment<byte>(_countBuffer, 0, PrimitiveSerializer.Int32Size), SocketFlags.None);

        if (sentCount != PrimitiveSerializer.Int32Size)
        {
            return SendMessageReturnCode.Fail;
        }

        sentCount = await _socket.SendAsync(new ArraySegment<byte>(_buffer, 0, actualLength), SocketFlags.None);

        return sentCount == actualLength ? SendMessageReturnCode.Success : SendMessageReturnCode.Fail;
    }

    [PublicAPI]
    internal async Task<ReceiveMessageResult> ReceiveMessageAsync()
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

    #region Read Primitives

    protected bool ReadBoolean(int offset = 0) => PrimitiveSerializer.ReadBoolean(_buffer, offset);
    protected byte ReadByte(int offset = 0) => PrimitiveSerializer.ReadByte(_buffer, offset);
    protected char ReadChar(int offset = 0) => PrimitiveSerializer.ReadChar(_buffer, offset);
    protected decimal ReadDecimal(int offset = 0) => _primitiveSerializer.ReadDecimal(_buffer, offset);
    protected double ReadDouble(int offset = 0) => PrimitiveSerializer.ReadDouble(_buffer, offset);
    protected short ReadInt16(int offset = 0) => PrimitiveSerializer.ReadInt16(_buffer, offset);
    protected int ReadInt32(int offset = 0) => PrimitiveSerializer.ReadInt32(_buffer, offset);
    protected long ReadInt64(int offset = 0) => PrimitiveSerializer.ReadInt64(_buffer, offset);
    protected sbyte ReadSByte(int offset = 0) => PrimitiveSerializer.ReadSByte(_buffer, offset);
    protected ushort ReadUInt16(int offset = 0) => PrimitiveSerializer.ReadUInt16(_buffer, offset);
    protected uint ReadUInt32(int offset = 0) => PrimitiveSerializer.ReadUInt32(_buffer, offset);
    protected ulong ReadUInt64(int offset = 0) => PrimitiveSerializer.ReadUInt64(_buffer, offset);

    #endregion

    #region Write Primitives

    protected void WriteBoolean(bool value, int offset = 0) => PrimitiveSerializer.WriteBoolean(value, _buffer, offset);
    protected void WriteByte(byte value, int offset = 0) => PrimitiveSerializer.WriteByte(value, _buffer, offset);
    protected void WriteChar(char value, int offset = 0) => _primitiveSerializer.WriteChar(value, _buffer, offset);
    protected void WriteDecimal(decimal value, int offset = 0) => _primitiveSerializer.WriteDecimal(value, _buffer, offset);
    protected void WriteDouble(double value, int offset = 0) => _primitiveSerializer.WriteDouble(value, _buffer, offset);
    protected void WriteInt16(short value, int offset = 0) => _primitiveSerializer.WriteInt16(value, _buffer, offset);
    protected void WriteInt32(int value, int offset = 0) => _primitiveSerializer.WriteInt32(value, _buffer, offset);
    protected void WriteSByte(sbyte value, int offset = 0) => PrimitiveSerializer.WriteSByte(value, _buffer, offset);
    protected void WriteUInt16(ushort value, int offset = 0) => _primitiveSerializer.WriteUInt16(value, _buffer, offset);
    protected void WriteUint32(uint value, int offset = 0) => _primitiveSerializer.WriteUInt32(value, _buffer, offset);
    protected void WriteUint64(ulong value, int offset = 0) => _primitiveSerializer.WriteUInt64(value, _buffer, offset);

    #endregion
}