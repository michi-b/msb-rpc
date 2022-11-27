using JetBrains.Annotations;
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

    [PublicAPI]
    public byte ReadByte() => _segment.ReadByte(PostIncrementPosition(ByteSize));

    [PublicAPI]
    public sbyte ReadSByte() => _segment.ReadSByte(PostIncrementPosition(SByteSize));

    [PublicAPI]
    public short ReadInt16() => _segment.ReadInt16(PostIncrementPosition(Int16Size));

    [PublicAPI]
    public ushort ReadUInt16() => _segment.ReadUInt16(PostIncrementPosition(UInt16Size));

    [PublicAPI]
    public int ReadInt32() => _segment.ReadInt32(PostIncrementPosition(Int32Size));

    [PublicAPI]
    public uint ReadUInt32() => _segment.ReadUInt32(PostIncrementPosition(UInt32Size));

    [PublicAPI]
    public long ReadInt64() => _segment.ReadInt64(PostIncrementPosition(Int64Size));

    [PublicAPI]
    public ulong ReadUInt64() => _segment.ReadUInt64(PostIncrementPosition(UInt64Size));

    [PublicAPI]
    public float ReadSingle() => _segment.ReadSingle(PostIncrementPosition(SingleSize));

    [PublicAPI]
    public double ReadDouble() => _segment.ReadDouble(PostIncrementPosition(DoubleSize));

    [PublicAPI]
    public decimal ReadDecimal() => _segment.ReadDecimal(PostIncrementPosition(DecimalSize));

    [PublicAPI]
    public bool ReadBoolean() => _segment.ReadBoolean(PostIncrementPosition(BooleanSize));

    [PublicAPI]
    public char ReadChar() => _segment.ReadChar(PostIncrementPosition(CharSize));
}