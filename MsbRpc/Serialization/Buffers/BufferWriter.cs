using JetBrains.Annotations;
using MsbRpc.Utility;
using static MsbRpc.Serialization.Primitives.PrimitiveSerializer;

namespace MsbRpc.Serialization.Buffers;

public struct BufferWriter
{
    private int _position;

    public BufferWriter(ArraySegment<byte> segment, int position = 0)
    {
        Buffer = segment;
        _position = position;
    }

    private int PostIncrementPosition(int increment)
    {
        int position = _position;
        _position += increment;
        return position;
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public ArraySegment<byte> Buffer { get; }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(byte value)
    {
        Buffer.WriteByte(value, PostIncrementPosition(ByteSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(sbyte value)
    {
        Buffer.WriteSByte(value, PostIncrementPosition(SByteSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(short value)
    {
        Buffer.WriteInt16(value, PostIncrementPosition(Int16Size));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(ushort value)
    {
        Buffer.WriteUInt16(value, PostIncrementPosition(UInt16Size));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(int value)
    {
        Buffer.WriteInt32(value, PostIncrementPosition(Int32Size));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(uint value)
    {
        Buffer.WriteUInt32(value, PostIncrementPosition(UInt32Size));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(long value)
    {
        Buffer.WriteInt64(value, PostIncrementPosition(Int64Size));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(ulong value)
    {
        Buffer.WriteUInt64(value, PostIncrementPosition(UInt64Size));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(float value)
    {
        Buffer.WriteSingle(value, PostIncrementPosition(SingleSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(double value)
    {
        Buffer.WriteDouble(value, PostIncrementPosition(DoubleSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(decimal value)
    {
        Buffer.WriteDecimal(value, PostIncrementPosition(DecimalSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(bool value)
    {
        Buffer.WriteBoolean(value, PostIncrementPosition(BooleanSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(char value)
    {
        Buffer.WriteChar(value, PostIncrementPosition(CharSize));
    }
}