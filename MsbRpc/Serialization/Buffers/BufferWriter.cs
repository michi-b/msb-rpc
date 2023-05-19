using System;
using JetBrains.Annotations;
using MsbRpc.Attributes;
using static MsbRpc.Serialization.Primitives.PrimitiveSerializer;

namespace MsbRpc.Serialization.Buffers;

public struct BufferWriter
{
    public delegate void WriteDelegate<in TValue>(ref BufferWriter writer, TValue value);

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

    [MayBeUsedByGeneratedCode]
    public void Write(byte value)
    {
        _buffer.WriteByte(value, PostIncrementPosition(ByteSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(sbyte value)
    {
        _buffer.WriteSbyte(value, PostIncrementPosition(SbyteSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(bool value)
    {
        _buffer.WriteBool(value, PostIncrementPosition(BoolSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(char value)
    {
        _buffer.WriteChar(value, PostIncrementPosition(CharSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(int value)
    {
        _buffer.WriteInt(value, PostIncrementPosition(IntSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(long value)
    {
        _buffer.WriteLong(value, PostIncrementPosition(LongSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(short value)
    {
        _buffer.WriteShort(value, PostIncrementPosition(ShortSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(uint value)
    {
        _buffer.WriteUint(value, PostIncrementPosition(UintSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(ulong value)
    {
        _buffer.WriteUlong(value, PostIncrementPosition(UlongSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(ushort value)
    {
        _buffer.WriteUshort(value, PostIncrementPosition(UshortSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(float value)
    {
        _buffer.WriteFloat(value, PostIncrementPosition(FloatSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(double value)
    {
        _buffer.WriteDouble(value, PostIncrementPosition(DoubleSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(decimal value)
    {
        _buffer.WriteDecimal(value, PostIncrementPosition(DecimalSize));
    }

    [MayBeUsedByGeneratedCode]
    public void Write(string value)
    {
        StringSerializer.Write(ref this, value);
    }

    [MayBeUsedByGeneratedCode]
    public void WriteNullable<TValue>(TValue? value, WriteDelegate<TValue> writeValue) where TValue : struct
        => NullableSerializer<TValue>.Write(ref this, value, writeValue);

    [PublicAPI]
    public ArraySegment<byte> GetWriteSegment(int count) => _buffer.GetOffsetSubSegment(PostIncrementPosition(count), count);

    public void WriteCustom<TValue>(TValue value, WriteDelegate<TValue> writeDelegate) => writeDelegate(ref this, value);
}