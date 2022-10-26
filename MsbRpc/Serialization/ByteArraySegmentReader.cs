using static MsbRpc.Serialization.Primitives.PrimitiveSerializer;

namespace MsbRpc.Serialization;

public struct ByteArraySegmentReader
{
    private readonly ArraySegment<byte> _segment;
    private int _position;

    public ByteArraySegmentReader(ArraySegment<byte> segment)
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
    
    public byte ReadByte() => _segment.ReadByte(PostIncrementPosition(ByteSize));
    
    public sbyte ReadSByte() => _segment.ReadSByte(PostIncrementPosition(SByteSize));
    
    public short ReadInt16() => _segment.ReadInt16(PostIncrementPosition(Int16Size));
    
    public ushort ReadUInt16() => _segment.ReadUInt16(PostIncrementPosition(UInt16Size));
    
    public int ReadInt32() => _segment.ReadInt32(PostIncrementPosition(Int32Size));
    
    public uint ReadUInt32() => _segment.ReadUInt32(PostIncrementPosition(UInt32Size));
    
    public long ReadInt64() => _segment.ReadInt64(PostIncrementPosition(Int64Size));
    
    public ulong ReadUInt64() => _segment.ReadUInt64(PostIncrementPosition(UInt64Size));
    
    public float ReadSingle() => _segment.ReadSingle(PostIncrementPosition(SingleSize));
    
    public double ReadDouble() => _segment.ReadDouble(PostIncrementPosition(DoubleSize));

    public decimal ReadDecimal() => _segment.ReadDecimal(PostIncrementPosition(DecimalSize));
}