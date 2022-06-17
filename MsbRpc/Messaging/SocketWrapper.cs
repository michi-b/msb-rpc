﻿using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Messaging;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public abstract class SocketWrapper : IDisposable
{
    public enum ReceiveFixedSizeReturnCode
    {
        Success = 0,
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2
    }

    public const int DefaultCapacity = 1024;

    private readonly byte[] _countBuffer = new byte[sizeof(int)];
    private readonly Socket _socket;
    private byte[] _buffer;
    private PrimitiveSerializer _primitiveSerializer;

    protected SocketWrapper(AddressFamily addressFamily, int capacity = DefaultCapacity)
        : this(NetworkUtility.CreateTcpSocket(addressFamily), capacity) { }

    protected SocketWrapper(Socket socket, int capacity = DefaultCapacity)
    {
        _buffer = new byte[capacity];
        _socket = socket;
    }

    public async Task ConnectAsync(IPEndPoint ep)
    {
        await _socket.ConnectAsync(ep);
    }

    public void Close()
    {
        _socket.Close();
    }

    public void Dispose()
    {
        _socket.Dispose();
    }

    protected async Task<ListenForMessagesReturnCode> ListenForMessagesAsync(Action<int> onMessageReceived, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ReceiveMessageResult receiveMessageResult = await ReceiveMessageAsync();
            switch (receiveMessageResult.MessageReturnCode)
            {
                case ReceiveMessageReturnCode.Success:
                    onMessageReceived(receiveMessageResult.Count);
                    break;
                case ReceiveMessageReturnCode.ConnectionClosed:
                    return ListenForMessagesReturnCode.ConnectionClosed;
                case ReceiveMessageReturnCode.ConnectionClosedUnexpectedly:
                    return ListenForMessagesReturnCode.ConnectionClosedUnexpectedly;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return ListenForMessagesReturnCode.Canceled;
    }

    protected async Task<ReceiveMessageResult> ReceiveMessageAsync()
    {
        switch (await ReceiveFixedSizeAsync(_countBuffer, PrimitiveSerializer.Int32Size))
        {
            case ReceiveFixedSizeReturnCode.Success:
                break;
            case ReceiveFixedSizeReturnCode.ConnectionClosed:
                return new ReceiveMessageResult(0, ReceiveMessageReturnCode.ConnectionClosed);
            case ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly:
                return new ReceiveMessageResult(0, ReceiveMessageReturnCode.ConnectionClosedUnexpectedly);
            default:
                throw new ArgumentOutOfRangeException();
        }

        int count = PrimitiveSerializer.ReadInt32(_countBuffer);
        return count == 0
            ? new ReceiveMessageResult(0, ReceiveMessageReturnCode.Success)
            : await ReceiveFixedSizeAsync(count) switch
            {
                ReceiveFixedSizeReturnCode.Success
                    => new ReceiveMessageResult(count, ReceiveMessageReturnCode.Success),
                ReceiveFixedSizeReturnCode.ConnectionClosed
                    => new ReceiveMessageResult(count, ReceiveMessageReturnCode.ConnectionClosedUnexpectedly),
                ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly
                    => new ReceiveMessageResult(count, ReceiveMessageReturnCode.ConnectionClosedUnexpectedly),
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    protected async Task<SendMessageReturnCode> SendMessageAsync(int size)
    {
        _primitiveSerializer.WriteInt32(size, _countBuffer);

        int count = await _socket.SendAsync(new ArraySegment<byte>(_countBuffer, 0, PrimitiveSerializer.Int32Size), SocketFlags.None);

        if (count != PrimitiveSerializer.Int32Size)
        {
            return SendMessageReturnCode.Fail;
        }

        count = await _socket.SendAsync(new ArraySegment<byte>(_buffer, 0, size), SocketFlags.None);

        return count == size ? SendMessageReturnCode.Success : SendMessageReturnCode.Fail;
    }

    private async Task<ReceiveFixedSizeReturnCode> ReceiveFixedSizeAsync(int size)
    {
        Reserve(size);
        return await ReceiveFixedSizeAsync(_buffer, size);
    }

    private async Task<ReceiveFixedSizeReturnCode> ReceiveFixedSizeAsync(byte[] buffer, int size)
    {
        int totalCount = 0;
        while (totalCount < size)
        {
            int totalRemainingCount = size - totalCount;
            var arraySegment = new ArraySegment<byte>(buffer, totalCount, totalRemainingCount);
            int count = await _socket.ReceiveAsync(arraySegment, SocketFlags.None);

            if (count == 0) //no bytes received means connection closed
            {
                break;
            }

            totalCount += count;
        }

        return totalCount == size //order of these checks might be important, as 0 could be the expected size
            ? ReceiveFixedSizeReturnCode.Success
            : totalCount == 0
                ? ReceiveFixedSizeReturnCode.ConnectionClosed
                : ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly;
    }

    protected void Reserve(int size)
    {
        if (_buffer.Length < size)
        {
            _buffer = new byte[size];
        }
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