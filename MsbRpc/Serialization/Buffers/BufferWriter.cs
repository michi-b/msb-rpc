using System;
using JetBrains.Annotations;
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

    [MayBeUsedByGenerator]
    public void WriteNullable(byte? value)
    {
        _buffer.WriteByteNullable(value, PostIncrementPosition(NullableByteSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(sbyte? value)
    {
        _buffer.WriteSbyteNullable(value, PostIncrementPosition(NullableSbyteSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(bool? value)
    {
        _buffer.WriteBoolNullable(value, PostIncrementPosition(NullableBoolSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(char? value)
    {
        _buffer.WriteCharNullable(value, PostIncrementPosition(NullableCharSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(int? value)
    {
        _buffer.WriteIntNullable(value, PostIncrementPosition(NullableIntSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(long? value)
    {
        _buffer.WriteLongNullable(value, PostIncrementPosition(NullableLongSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(short? value)
    {
        _buffer.WriteShortNullable(value, PostIncrementPosition(NullableShortSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(uint? value)
    {
        _buffer.WriteUintNullable(value, PostIncrementPosition(NullableUintSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(ulong? value)
    {
        _buffer.WriteUlongNullable(value, PostIncrementPosition(NullableUlongSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(ushort? value)
    {
        _buffer.WriteUshortNullable(value, PostIncrementPosition(NullableUshortSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(float? value)
    {
        _buffer.WriteFloatNullable(value, PostIncrementPosition(NullableFloatSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(double? value)
    {
        _buffer.WriteDoubleNullable(value, PostIncrementPosition(NullableDoubleSize));
    }

    [MayBeUsedByGenerator]
    public void WriteNullable(decimal? value)
    {
        _buffer.WriteDecimalNullable(value, PostIncrementPosition(NullableDecimalSize));
    }

    [PublicAPI]
    public ArraySegment<byte> GetWriteSegment(int count) => _buffer.GetOffsetSubSegment(PostIncrementPosition(count), count);
}