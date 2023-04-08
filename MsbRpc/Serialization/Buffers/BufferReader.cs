﻿using System;
using JetBrains.Annotations;
using MsbRpc.Attributes;
using static MsbRpc.Serialization.Primitives.PrimitiveSerializer;

namespace MsbRpc.Serialization.Buffers;

public struct BufferReader
{
    private readonly ArraySegment<byte> _buffer;
    private int _position;

    public BufferReader(ArraySegment<byte> buffer)
    {
        _position = 0;
        _buffer = buffer;
    }

    private int PostIncrementPosition(int increment)
    {
        int position = _position;
        _position += increment;
        return position;
    }

    [MayBeUsedByGenerator]
    public byte ReadByte() => _buffer.ReadByte(PostIncrementPosition(ByteSize));

    [MayBeUsedByGenerator]
    public sbyte ReadSbyte() => _buffer.ReadSbyte(PostIncrementPosition(SbyteSize));

    [MayBeUsedByGenerator]
    public bool ReadBool() => _buffer.ReadBool(PostIncrementPosition(BoolSize));

    [MayBeUsedByGenerator]
    public char ReadChar() => _buffer.ReadChar(PostIncrementPosition(CharSize));

    [MayBeUsedByGenerator]
    public int ReadInt() => _buffer.ReadInt(PostIncrementPosition(IntSize));

    [MayBeUsedByGenerator]
    public long ReadLong() => _buffer.ReadLong(PostIncrementPosition(LongSize));

    [MayBeUsedByGenerator]
    public short ReadShort() => _buffer.ReadShort(PostIncrementPosition(ShortSize));

    [MayBeUsedByGenerator]
    public uint ReadUint() => _buffer.ReadUint(PostIncrementPosition(UintSize));

    [MayBeUsedByGenerator]
    public ulong ReadUlong() => _buffer.ReadUlong(PostIncrementPosition(UlongSize));

    [MayBeUsedByGenerator]
    public ushort ReadUshort() => _buffer.ReadUshort(PostIncrementPosition(UshortSize));

    [MayBeUsedByGenerator]
    public float ReadFloat() => _buffer.ReadFloat(PostIncrementPosition(FloatSize));

    [MayBeUsedByGenerator]
    public double ReadDouble() => _buffer.ReadDouble(PostIncrementPosition(DoubleSize));

    [MayBeUsedByGenerator]
    public decimal ReadDecimal() => _buffer.ReadDecimal(PostIncrementPosition(DecimalSize));

    [MayBeUsedByGenerator]
    public byte? ReadByteNullable() => _buffer.ReadByteNullable(PostIncrementPosition(NullableByteSize));

    [MayBeUsedByGenerator]
    public sbyte? ReadSbyteNullable() => _buffer.ReadSbyteNullable(PostIncrementPosition(NullableSbyteSize));

    [MayBeUsedByGenerator]
    public bool? ReadBoolNullable() => _buffer.ReadBoolNullable(PostIncrementPosition(NullableBoolSize));

    [MayBeUsedByGenerator]
    public char? ReadCharNullable() => _buffer.ReadCharNullable(PostIncrementPosition(NullableCharSize));

    [MayBeUsedByGenerator]
    public int? ReadIntNullable() => _buffer.ReadIntNullable(PostIncrementPosition(NullableIntSize));

    [MayBeUsedByGenerator]
    public long? ReadLongNullable() => _buffer.ReadLongNullable(PostIncrementPosition(NullableLongSize));

    [MayBeUsedByGenerator]
    public short? ReadShortNullable() => _buffer.ReadShortNullable(PostIncrementPosition(NullableShortSize));

    [MayBeUsedByGenerator]
    public uint? ReadUintNullable() => _buffer.ReadUintNullable(PostIncrementPosition(NullableUintSize));

    [MayBeUsedByGenerator]
    public ulong? ReadUlongNullable() => _buffer.ReadUlongNullable(PostIncrementPosition(NullableUlongSize));

    [MayBeUsedByGenerator]
    public ushort? ReadUshortNullable() => _buffer.ReadUshortNullable(PostIncrementPosition(NullableUshortSize));

    [MayBeUsedByGenerator]
    public float? ReadFloatNullable() => _buffer.ReadFloatNullable(PostIncrementPosition(NullableFloatSize));

    [MayBeUsedByGenerator]
    public double? ReadDoubleNullable() => _buffer.ReadDoubleNullable(PostIncrementPosition(NullableDoubleSize));

    [MayBeUsedByGenerator]
    public decimal? ReadDecimalNullable() => _buffer.ReadDecimalNullable(PostIncrementPosition(NullableDecimalSize));

    [PublicAPI]
    public ArraySegment<byte> ReadSegment(int count) => _buffer.GetOffsetSubSegment(PostIncrementPosition(count), count);
}