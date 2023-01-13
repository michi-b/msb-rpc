using System;
using MsbRpc.Attributes;
using static MsbRpc.Serialization.Primitives.PrimitiveSerializer;

namespace MsbRpc.Serialization.Buffers;

public struct BufferWriter
{
    private int _position;
    private readonly ArraySegment<byte> _buffer;

    public BufferWriter(ArraySegment<byte> buffer, int position = 0)
    {
        _buffer = buffer;
        _position = position;
    }

    private int PostIncrementPosition(int increment)
    {
        int position = _position;
        _position += increment;
        return position;
    }

    [MayBeUsedByGenerator]
    public void Write(byte value)
    {
        _buffer.WriteByte(value, PostIncrementPosition(ByteSize));
    }

    [MayBeUsedByGenerator]
    public void Write(sbyte value)
    {
        _buffer.WriteSbyte(value, PostIncrementPosition(SbyteSize));
    }

    [MayBeUsedByGenerator]
    public void Write(bool value)
    {
        _buffer.WriteBool(value, PostIncrementPosition(BoolSize));
    }

    [MayBeUsedByGenerator]
    public void Write(char value)
    {
        _buffer.WriteChar(value, PostIncrementPosition(CharSize));
    }

    [MayBeUsedByGenerator]
    public void Write(int value)
    {
        _buffer.WriteInt(value, PostIncrementPosition(IntSize));
    }

    [MayBeUsedByGenerator]
    public void Write(long value)
    {
        _buffer.WriteLong(value, PostIncrementPosition(LongSize));
    }

    [MayBeUsedByGenerator]
    public void Write(short value)
    {
        _buffer.WriteShort(value, PostIncrementPosition(ShortSize));
    }

    [MayBeUsedByGenerator]
    public void Write(uint value)
    {
        _buffer.WriteUint(value, PostIncrementPosition(UintSize));
    }

    [MayBeUsedByGenerator]
    public void Write(ulong value)
    {
        _buffer.WriteUlong(value, PostIncrementPosition(UlongSize));
    }

    [MayBeUsedByGenerator]
    public void Write(ushort value)
    {
        _buffer.WriteUshort(value, PostIncrementPosition(UshortSize));
    }

    [MayBeUsedByGenerator]
    public void Write(float value)
    {
        _buffer.WriteFloat(value, PostIncrementPosition(FloatSize));
    }

    [MayBeUsedByGenerator]
    public void Write(double value)
    {
        _buffer.WriteDouble(value, PostIncrementPosition(DoubleSize));
    }

    [MayBeUsedByGenerator]
    public void Write(decimal value)
    {
        _buffer.WriteDecimal(value, PostIncrementPosition(DecimalSize));
    }
}