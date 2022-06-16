using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using MsbRpc.Exceptions;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public abstract class RpcSocket : IDisposable
{
    private readonly byte[] _countBuffer = new byte[sizeof(Int32)];
    private readonly Socket? _socket;
    private byte[] _buffer;
    private PrimitiveSerializer _primitiveSerializer;

    protected RpcSocket(Socket socket, int initialBufferSize)
    {
        _buffer = new byte[initialBufferSize];
        _socket = socket;
    }

    protected  async Task ListenAsync(Action<int> accept, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            accept(await AcceptAsync());
        }
    }

    protected  async Task<int> AcceptAsync()
    {
        await ReceiveAsync(_countBuffer, PrimitiveSerializer.Int32Size);
        Int32 count = PrimitiveSerializer.ReadInt32(_countBuffer);
        await ReceiveAsync(count);
        return count;
    }

    protected async Task SendAsync(int count)
    {
        AssertSocketOwnership();
        _primitiveSerializer.WriteInt32(count, _countBuffer);
        await _socket.SendAsync(new ArraySegment<byte>(_countBuffer, 0, count), SocketFlags.None);
        await _socket.SendAsync(new ArraySegment<byte>(_buffer, 0, count), SocketFlags.None);
    }

    public void Dispose()
    {
        _socket?.Dispose();
    }

    protected  async Task ReceiveAsync(int count)
    {
        Reserve(count);
        await ReceiveAsync(_buffer, count);
    }

    private void Reserve(int size)
    {
        if (_buffer.Length < size)
        {
            _buffer = new byte[size];
        }
    }

    private async Task ReceiveAsync(byte[] buffer, int count)
    {
        AssertSocketOwnership();
        int totalBytesReceivedCount = 0;
        while (totalBytesReceivedCount < count)
        {
            int totalBytesRemainingCount = count - totalBytesReceivedCount;
            var arraySegment = new ArraySegment<byte>(buffer, totalBytesReceivedCount, totalBytesRemainingCount);
            int bytesReceivedCount = await _socket.ReceiveAsync(arraySegment, SocketFlags.None);
            if (bytesReceivedCount == 0)
            {
                throw new ConnectionClosedException();
            }

            totalBytesReceivedCount += bytesReceivedCount;
        }
    }

    private void AssertSocketOwnership()
    {
        if (_socket == null)
        {
            throw new SocketOwnershipLostException();
        }
    }

    protected abstract void ReadMessage(int size);

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