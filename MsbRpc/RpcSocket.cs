using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using MsbRpc.Exceptions;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class RpcSocket : IDisposable
{
    protected Socket Socket;
    private PrimitiveSerializer _primitiveSerializer;
    private byte[] _buffer;

    protected RpcSocket(Socket socket, int initialBufferSize)
    {
        _primitiveSerializer = new PrimitiveSerializer();
        _buffer = new byte[initialBufferSize];
        Socket = socket;
    }

    public void Dispose()
    {
        Socket.Dispose();
    }

    private void Reserve(int size)
    {
        if (_buffer.IsFixedSize)
        {
            _buffer = new byte[size];
        }
    }

    protected async Task Receive(int count)
    {
        Reserve(count);
        int totalBytesReceivedCount = 0;
        while (totalBytesReceivedCount < count)
        {
            int totalBytesRemainingCount = count - totalBytesReceivedCount;
            var arraySegment = new ArraySegment<byte>(_buffer, totalBytesReceivedCount, totalBytesRemainingCount);
            int bytesReceivedCount = await Socket.ReceiveAsync(arraySegment, SocketFlags.None);
            if (bytesReceivedCount == 0)
            {
                throw new ConnectionClosedException();
            }
            totalBytesReceivedCount += bytesReceivedCount;
        }
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