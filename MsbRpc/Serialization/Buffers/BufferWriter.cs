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
        Buffer.WriteSbyte(value, PostIncrementPosition(SbyteSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(bool value)
    {
        Buffer.WriteBool(value, PostIncrementPosition(BoolSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(char value)
    {
        Buffer.WriteChar(value, PostIncrementPosition(CharSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(int value)
    {
        Buffer.WriteInt(value, PostIncrementPosition(IntSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(long value)
    {
        Buffer.WriteLong(value, PostIncrementPosition(LongSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(short value)
    {
        Buffer.WriteShort(value, PostIncrementPosition(ShortSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(uint value)
    {
        Buffer.WriteUint(value, PostIncrementPosition(UintSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(ulong value)
    {
        Buffer.WriteUlong(value, PostIncrementPosition(UlongSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(ushort value)
    {
        Buffer.WriteUshort(value, PostIncrementPosition(UshortSize));
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public void Write(float value)
    {
        Buffer.WriteFloat(value, PostIncrementPosition(FloatSize));
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
}