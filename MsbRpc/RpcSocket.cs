using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public abstract class RpcSocket : IDisposable
{
    public enum ReceiveCountReturnType
    {
        Success = 0,
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2
    }

    public enum ReceiveOperationReturnType
    {
        Success = 0,
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedlyAfterCount = 2,
        ConnectionClosedUnexpectedlyBeforeCount = 3
    }

    public enum ListenReturnType
    {
        Canceled = 0,
        ConnectionClosed = 1,
        ConnectionClosedUnexpectedly = 2
    }

    public const int DefaultCapacity = 1024;

    private readonly byte[] _countBuffer = new byte[sizeof(Int32)];
    private readonly Socket _socket;
    private byte[] _buffer;
    private PrimitiveSerializer _primitiveSerializer;

    protected RpcSocket(AddressFamily addressFamily, int capacity = DefaultCapacity)
        : this(NetworkUtility.CreateTcpSocket(addressFamily), capacity) { }

    protected RpcSocket(Socket socket, int capacity = DefaultCapacity)
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

    public readonly struct ReceiveResult
    {
        public ReceiveResult(int count, ReceiveOperationReturnType returnType)
        {
            Count = count;
            ReturnType = returnType;
        }

        public int Count { get; }
        public ReceiveOperationReturnType ReturnType { get; }
    }

    protected async Task<ListenReturnType> ListenAsync(Action<int> accept, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ReceiveResult receiveResult = await ReceiveAsync();
            switch (receiveResult.ReturnType)
            {
                case ReceiveOperationReturnType.Success:
                    accept(receiveResult.Count);
                    break;
                case ReceiveOperationReturnType.ConnectionClosed:
                    return ListenReturnType.ConnectionClosed;
                case ReceiveOperationReturnType.ConnectionClosedUnexpectedlyAfterCount:
                case ReceiveOperationReturnType.ConnectionClosedUnexpectedlyBeforeCount:
                    return ListenReturnType.ConnectionClosedUnexpectedly;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return ListenReturnType.Canceled;
    }

    protected async Task<ReceiveResult> ReceiveAsync()
    {
        switch (await ReceiveCountAsync(_countBuffer, PrimitiveSerializer.Int32Size))
        {
            case ReceiveCountReturnType.Success:
                break;
            case ReceiveCountReturnType.ConnectionClosed:
                return new ReceiveResult(0, ReceiveOperationReturnType.ConnectionClosed);
            case ReceiveCountReturnType.ConnectionClosedUnexpectedly:
                return new ReceiveResult(0, ReceiveOperationReturnType.ConnectionClosedUnexpectedlyBeforeCount);
            default:
                throw new ArgumentOutOfRangeException();
        }

        Int32 count = PrimitiveSerializer.ReadInt32(_countBuffer);

        return await ReceiveCountAsync(count) switch
        {
            ReceiveCountReturnType.Success => new ReceiveResult(count, ReceiveOperationReturnType.Success),
            ReceiveCountReturnType.ConnectionClosed => new ReceiveResult(count, ReceiveOperationReturnType.ConnectionClosedUnexpectedlyAfterCount),
            ReceiveCountReturnType.ConnectionClosedUnexpectedly => new ReceiveResult
                (count, ReceiveOperationReturnType.ConnectionClosedUnexpectedlyAfterCount),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected async Task SendAsync(int count)
    {
        _primitiveSerializer.WriteInt32(count, _countBuffer);
        int bytesSent = await _socket.SendAsync(new ArraySegment<byte>(_countBuffer, 0, PrimitiveSerializer.Int32Size), SocketFlags.None);
        Debug.Assert(bytesSent == PrimitiveSerializer.Int32Size);

        bytesSent = await _socket.SendAsync(new ArraySegment<byte>(_buffer, 0, count), SocketFlags.None);
        Debug.Assert(bytesSent == count);
    }

    private async Task<ReceiveCountReturnType> ReceiveCountAsync(int count)
    {
        Reserve(count);
        return await ReceiveCountAsync(_buffer, count);
    }

    protected void Reserve(int size)
    {
        if (_buffer.Length < size)
        {
            _buffer = new byte[size];
        }
    }

    private async Task<ReceiveCountReturnType> ReceiveCountAsync(byte[] buffer, int count)
    {
        int totalBytesReceivedCount = 0;
        while (totalBytesReceivedCount < count)
        {
            int totalBytesRemainingCount = count - totalBytesReceivedCount;
            var arraySegment = new ArraySegment<byte>(buffer, totalBytesReceivedCount, totalBytesRemainingCount);
            int bytesReceivedCount = await _socket.ReceiveAsync(arraySegment, SocketFlags.None);

            if (bytesReceivedCount == 0) //no bytes received means connection closed
            {
                break;
            }

            totalBytesReceivedCount += bytesReceivedCount;
        }

        return totalBytesReceivedCount == count //order of these checks is important, as 0 can be the expected count
            ? ReceiveCountReturnType.Success
            : totalBytesReceivedCount == 0
                ? ReceiveCountReturnType.ConnectionClosed
                : ReceiveCountReturnType.ConnectionClosedUnexpectedly;
    }

    #region Read Primitives

    protected Boolean ReadBoolean(int offset = 0) => PrimitiveSerializer.ReadBoolean(_buffer, offset);
    protected Byte ReadByte(int offset = 0) => PrimitiveSerializer.ReadByte(_buffer, offset);
    protected Char ReadChar(int offset = 0) => PrimitiveSerializer.ReadChar(_buffer, offset);
    protected Decimal ReadDecimal(int offset = 0) => _primitiveSerializer.ReadDecimal(_buffer, offset);
    protected Double ReadDouble(int offset = 0) => PrimitiveSerializer.ReadDouble(_buffer, offset);
    protected Int16 ReadInt16(int offset = 0) => PrimitiveSerializer.ReadInt16(_buffer, offset);
    protected Int32 ReadInt32(int offset = 0) => PrimitiveSerializer.ReadInt32(_buffer, offset);
    protected Int64 ReadInt64(int offset = 0) => PrimitiveSerializer.ReadInt64(_buffer, offset);
    protected SByte ReadSByte(int offset = 0) => PrimitiveSerializer.ReadSByte(_buffer, offset);
    protected UInt16 ReadUInt16(int offset = 0) => PrimitiveSerializer.ReadUInt16(_buffer, offset);
    protected UInt32 ReadUInt32(int offset = 0) => PrimitiveSerializer.ReadUInt32(_buffer, offset);
    protected UInt64 ReadUInt64(int offset = 0) => PrimitiveSerializer.ReadUInt64(_buffer, offset);

    #endregion

    #region Write Primitives

    protected void WriteBoolean(Boolean value, int offset = 0) => PrimitiveSerializer.WriteBoolean(value, _buffer, offset);
    protected void WriteByte(Byte value, int offset = 0) => PrimitiveSerializer.WriteByte(value, _buffer, offset);
    protected void WriteChar(Char value, int offset = 0) => _primitiveSerializer.WriteChar(value, _buffer, offset);
    protected void WriteDecimal(Decimal value, int offset = 0) => _primitiveSerializer.WriteDecimal(value, _buffer, offset);
    protected void WriteDouble(Double value, int offset = 0) => _primitiveSerializer.WriteDouble(value, _buffer, offset);
    protected void WriteInt16(Int16 value, int offset = 0) => _primitiveSerializer.WriteInt16(value, _buffer, offset);
    protected void WriteInt32(Int32 value, int offset = 0) => _primitiveSerializer.WriteInt32(value, _buffer, offset);
    protected void WriteSByte(SByte value, int offset = 0) => PrimitiveSerializer.WriteSByte(value, _buffer, offset);
    protected void WriteUInt16(UInt16 value, int offset = 0) => _primitiveSerializer.WriteUInt16(value, _buffer, offset);
    protected void WriteUint32(UInt32 value, int offset = 0) => _primitiveSerializer.WriteUInt32(value, _buffer, offset);
    protected void WriteUint64(UInt64 value, int offset = 0) => _primitiveSerializer.WriteUInt64(value, _buffer, offset);

    #endregion
}