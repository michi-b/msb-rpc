﻿using JetBrains.Annotations;
using MsbRpc.Utility;
using static MsbRpc.Serialization.Primitives.PrimitiveSerializer;

namespace MsbRpc.Serialization.Buffers;

public struct BufferReader
{
    private readonly ArraySegment<byte> _segment;
    private int _position;

    public BufferReader(ArraySegment<byte> segment)
    {
        _position = 0;
        _segment = segment;
    }

    private int PostIncrementPosition(int increment)
    {
        int position = _position;
        _position += increment;
        return position;
    }

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public byte ReadByte() => _segment.ReadByte(PostIncrementPosition(ByteSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public sbyte ReadSbyte() => _segment.ReadSbyte(PostIncrementPosition(SbyteSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public bool ReadBool() => _segment.ReadBool(PostIncrementPosition(BoolSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public char ReadChar() => _segment.ReadChar(PostIncrementPosition(CharSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public int ReadInt() => _segment.ReadInt(PostIncrementPosition(IntSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public long ReadLong() => _segment.ReadLong(PostIncrementPosition(LongSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public short ReadShort() => _segment.ReadShort(PostIncrementPosition(ShortSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public uint ReadUint() => _segment.ReadUint(PostIncrementPosition(UintSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public ulong ReadUlong() => _segment.ReadUlong(PostIncrementPosition(UlongSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public ushort ReadUshort() => _segment.ReadUshort(PostIncrementPosition(UshortSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public float ReadFloat() => _segment.ReadFloat(PostIncrementPosition(FloatSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public double ReadDouble() => _segment.ReadDouble(PostIncrementPosition(DoubleSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public decimal ReadDecimal() => _segment.ReadDecimal(PostIncrementPosition(DecimalSize));
}