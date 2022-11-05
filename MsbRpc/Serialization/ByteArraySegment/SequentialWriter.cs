using JetBrains.Annotations;
using static MsbRpc.Serialization.Primitives.PrimitiveSerializer;

namespace MsbRpc.Serialization.ByteArraySegment;

public struct SequentialWriter
{
    private readonly ArraySegment<byte> _segment;
    private int _position;
    
    public SequentialWriter(ArraySegment<byte> segment)
    {
        _segment = segment;
        _position = 0;
    }
    
    private int PostIncrementPosition(int increment)
    {
        int position = _position;
        _position += increment;
        return position;
    }
    
    [PublicAPI]
    public void Write(byte value)
    {
        _segment.WriteByte(value, PostIncrementPosition(ByteSize));
    }
    
    [PublicAPI]
    public void Write(sbyte value)
    {
        _segment.WriteSByte(value, PostIncrementPosition(SByteSize));
    }

    [PublicAPI]
    public void Write(short value)
    {
        _segment.WriteInt16(value, PostIncrementPosition(Int16Size));
    }
    
    [PublicAPI]
    public void Write(ushort value)
    {
        _segment.WriteUInt16(value, PostIncrementPosition(UInt16Size));
    }
    
    [PublicAPI]
    public void Write(int value)
    {
        _segment.WriteInt32(value, PostIncrementPosition(Int32Size));
    }
    
    [PublicAPI]
    public void Write(uint value)
    {
        _segment.WriteUInt32(value, PostIncrementPosition(UInt32Size));
    }
    
    [PublicAPI]
    public void Write(long value)
    {
        _segment.WriteInt64(value, PostIncrementPosition(Int64Size));
    }
    
    [PublicAPI]
    public void Write(ulong value)
    {
        _segment.WriteUInt64(value, PostIncrementPosition(UInt64Size));
    }
    
    [PublicAPI]
    public void Write(float value)
    {
        _segment.WriteSingle(value, PostIncrementPosition(SingleSize));
    }
    
    [PublicAPI]
    public void Write(double value)
    {
        _segment.WriteDouble(value, PostIncrementPosition(DoubleSize));
    }

    [PublicAPI]
    public void Write(decimal value)
    {
        _segment.WriteDecimal(value, PostIncrementPosition(DecimalSize));
    }
    
    [PublicAPI]
    public void Write(bool value)
    {
        _segment.WriteBoolean(value, PostIncrementPosition(BooleanSize));
    }
    
    [PublicAPI]
    public void Write(char value)
    {
        _segment.WriteChar(value, PostIncrementPosition(CharSize));
    }
}