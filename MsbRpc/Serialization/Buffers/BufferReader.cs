using JetBrains.Annotations;
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
    public sbyte ReadSByte() => _segment.ReadSByte(PostIncrementPosition(SByteSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public short ReadInt16() => _segment.ReadInt16(PostIncrementPosition(Int16Size));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public ushort ReadUInt16() => _segment.ReadUInt16(PostIncrementPosition(UInt16Size));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public int ReadInt32() => _segment.ReadInt32(PostIncrementPosition(Int32Size));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public uint ReadUInt32() => _segment.ReadUInt32(PostIncrementPosition(UInt32Size));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public long ReadInt64() => _segment.ReadInt64(PostIncrementPosition(Int64Size));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public ulong ReadUInt64() => _segment.ReadUInt64(PostIncrementPosition(UInt64Size));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public float ReadSingle() => _segment.ReadSingle(PostIncrementPosition(SingleSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public double ReadDouble() => _segment.ReadDouble(PostIncrementPosition(DoubleSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public decimal ReadDecimal() => _segment.ReadDecimal(PostIncrementPosition(DecimalSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public bool ReadBoolean() => _segment.ReadBoolean(PostIncrementPosition(BooleanSize));

    [PublicAPI(Messages.ForUseInGeneratedCode)]
    public char ReadChar() => _segment.ReadChar(PostIncrementPosition(CharSize));
}