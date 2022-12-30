using System;
using JetBrains.Annotations;
using MsbRpc.Utility;
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

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public byte ReadByte() => _buffer.ReadByte(PostIncrementPosition(ByteSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public sbyte ReadSbyte() => _buffer.ReadSbyte(PostIncrementPosition(SbyteSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public bool ReadBool() => _buffer.ReadBool(PostIncrementPosition(BoolSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public char ReadChar() => _buffer.ReadChar(PostIncrementPosition(CharSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public int ReadInt() => _buffer.ReadInt(PostIncrementPosition(IntSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public long ReadLong() => _buffer.ReadLong(PostIncrementPosition(LongSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public short ReadShort() => _buffer.ReadShort(PostIncrementPosition(ShortSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public uint ReadUint() => _buffer.ReadUint(PostIncrementPosition(UintSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public ulong ReadUlong() => _buffer.ReadUlong(PostIncrementPosition(UlongSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public ushort ReadUshort() => _buffer.ReadUshort(PostIncrementPosition(UshortSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public float ReadFloat() => _buffer.ReadFloat(PostIncrementPosition(FloatSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public double ReadDouble() => _buffer.ReadDouble(PostIncrementPosition(DoubleSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public decimal ReadDecimal() => _buffer.ReadDecimal(PostIncrementPosition(DecimalSize));
}