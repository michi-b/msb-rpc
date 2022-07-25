using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
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
    private readonly byte[] _buffer;

    private readonly SemaphoreSlim _bufferLock = new(1);

    private readonly byte[] _countBuffer = new byte[PrimitiveSerializer.Int32Size];
    private readonly Socket _socket;
    private PrimitiveSerializer _primitiveSerializer;

    private SemaphoreSlim _availableMessagesCount = new(0);
    private ConcurrentQueue<byte[]> _availableMessages = new ConcurrentQueue<byte[]>();

    public int SendBufferSize => _socket.SendBufferSize;
    public int ReceiveBufferSize => _socket.ReceiveBufferSize;

    private SemaphoreSlim _listenLock = new(0);
    private SemaphoreSlim _messagesAvailable = new(0);

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

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<ListenReturnCode> Listen()
    {
        if (await _listenLock.WaitAsync(0))
        {
            try
            {
                while (true) //listens until the connection is closed by the remote
                {
                    ReceiveMessageResult receiveMessageResult = await ReceiveMessageAsync();
                    switch (receiveMessageResult.MessageReturnCode)
                    {
                        case ReceiveMessageReturnCode.Success:
                            _availableMessages.Enqueue(receiveMessageResult.Bytes);
                            _availableMessagesCount.Release();
                            continue;
                        case ReceiveMessageReturnCode.ConnectionClosed:
                            return ListenReturnCode.ConnectionClosed;
                        case ReceiveMessageReturnCode.ConnectionClosedUnexpectedly:
                            return ListenReturnCode.ConnectionClosedUnexpectedly;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            finally
            {
                _listenLock.Release();
            }
        }
        else
        {
            throw new AlreadyListeningException();
        }
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected async Task<ReceiveMessageResult> ReceiveMessageAsync()
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

        return new ReceiveMessageResult(bytes, await ReceiveFixedLengthAsync(bytes) switch
        {
            ReceiveFixedSizeReturnCode.Success => ReceiveMessageReturnCode.Success,
            ReceiveFixedSizeReturnCode.ConnectionClosed => ReceiveMessageReturnCode.ConnectionClosedUnexpectedly,
            ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly => ReceiveMessageReturnCode.ConnectionClosedUnexpectedly,
            _ => throw new ArgumentOutOfRangeException()
        });
    }

    protected async Task<SendMessageReturnCode> SendMessageAsync(int length) => await SendMessageAsync(length, length);

    /// <summary>
    ///     do not use!!! exposed for testing unexpected connection loss only
    /// </summary>
    /// <param name="indicatedLength">value of the length header of the message</param>
    /// <param name="actualLength">how many bytes are actually sent</param>
    /// <see cref="SendMessageAsync(int)" />
    protected async Task<SendMessageReturnCode> SendMessageAsync(int indicatedLength, int actualLength)
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

    private async Task<ReceiveFixedSizeReturnCode> ReceiveFixedLengthAsync(byte[] buffer)
    {
        int totalCount = 0;
        int length = buffer.Length;
        while (totalCount < length)
        {
            int totalRemainingCount = length - totalCount;
            var arraySegment = new ArraySegment<byte>(buffer, totalCount, totalRemainingCount);
            int count = await _socket.ReceiveAsync(arraySegment, SocketFlags.None);

            if (count == 0) //no bytes received means connection closed
            {
                break;
            }

            totalCount += count;
        }

        return totalCount == length //order of these checks might be important, as 0 could be the expected indicatedLength
            ? ReceiveFixedSizeReturnCode.Success
            : totalCount == 0
                ? ReceiveFixedSizeReturnCode.ConnectionClosed
                : ReceiveFixedSizeReturnCode.ConnectionClosedUnexpectedly;
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

    protected async Task<byte[]> AwaitNextMessage(CancellationToken cancellationToken)
    {
        await _messagesAvailable.WaitAsync(cancellationToken);
        return ConsumeNextMessage();
    }

        /// <exception cref="NoMessageAvailableException"></exception>
    protected byte[] ConsumeNextMessage()
    {
        if (_availableMessages.TryDequeue(out var message))
        {
            return message;
        }
        else
        {
            throw new NoMessageAvailableException();
        }
    }

    protected bool IsMessageAvailable => _messagesAvailable.CurrentCount > 0;

    #endregion
}