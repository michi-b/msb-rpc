#region

using System;
using System.Diagnostics;
using JetBrains.Annotations;
using MsbRpc.Attributes;
using static MsbRpc.Serialization.Primitives.PrimitiveSerializer;

#endregion

namespace MsbRpc.Serialization.Buffers;

[DebuggerDisplay("Position = {_position}, Length = {_buffer.Count}")]
public struct BufferReader
{
    public delegate TValue ReadDelegate<out TValue>(ref BufferReader reader);

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

    [MayBeUsedByGeneratedCode]
    public TValue ReadCustom<TValue>(ReadDelegate<TValue> read) => read(ref this);

    [MayBeUsedByGeneratedCode]
    public byte ReadByte() => _buffer.ReadByte(PostIncrementPosition(ByteSize));

    [MayBeUsedByGeneratedCode]
    public sbyte ReadSbyte() => _buffer.ReadSbyte(PostIncrementPosition(SbyteSize));

    [MayBeUsedByGeneratedCode]
    public bool ReadBool() => _buffer.ReadBool(PostIncrementPosition(BoolSize));

    [MayBeUsedByGeneratedCode]
    public char ReadChar() => _buffer.ReadChar(PostIncrementPosition(CharSize));

    [MayBeUsedByGeneratedCode]
    public int ReadInt() => _buffer.ReadInt(PostIncrementPosition(IntSize));

    [MayBeUsedByGeneratedCode]
    public long ReadLong() => _buffer.ReadLong(PostIncrementPosition(LongSize));

    [MayBeUsedByGeneratedCode]
    public short ReadShort() => _buffer.ReadShort(PostIncrementPosition(ShortSize));

    [MayBeUsedByGeneratedCode]
    public uint ReadUint() => _buffer.ReadUint(PostIncrementPosition(UintSize));

    [MayBeUsedByGeneratedCode]
    public ulong ReadUlong() => _buffer.ReadUlong(PostIncrementPosition(UlongSize));

    [MayBeUsedByGeneratedCode]
    public ushort ReadUshort() => _buffer.ReadUshort(PostIncrementPosition(UshortSize));

    [MayBeUsedByGeneratedCode]
    public float ReadFloat() => _buffer.ReadFloat(PostIncrementPosition(FloatSize));

    [MayBeUsedByGeneratedCode]
    public double ReadDouble() => _buffer.ReadDouble(PostIncrementPosition(DoubleSize));

    [MayBeUsedByGeneratedCode]
    public decimal ReadDecimal() => _buffer.ReadDecimal(PostIncrementPosition(DecimalSize));

    [MayBeUsedByGeneratedCode]
    public string ReadString() => StringSerializer.ReadString(ref this);

    [MayBeUsedByGeneratedCode]
    public TValue? ReadNullable<TValue>(ReadDelegate<TValue> readValue) where TValue : struct => NullableSerializer<TValue>.Read(ref this, readValue);

    [PublicAPI]
    public ArraySegment<byte> ReadSegment(int count) => _buffer.GetOffsetSubSegment(PostIncrementPosition(count), count);
}