// ReSharper disable BuiltInTypeReferenceStyle

using System.Diagnostics;
using JetBrains.Annotations;
using MsbRpc.Serialization.Exceptions;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization;

public static class ByteArraySegmentExtensions
{
    #region Boolean

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Boolean ReadBoolean(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        return target.Array!.ReadBoolean(target.Offset + offset);
    }

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteBoolean(this ArraySegment<byte> target, bool value, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        target.Array!.WriteBoolean(value, offset);
    }

    #endregion

    #region Byte

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Byte ReadByte(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        return target.Array!.ReadByte(target.Offset + offset);
    }

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteByte(this ArraySegment<byte> target, byte value, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        target.Array!.WriteByte(value, offset);
    }

    #endregion

    #region SByte

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static SByte ReadSByte(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<sbyte>(offset);
        return target.Array!.ReadSByte(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteSByte(this ArraySegment<byte> target, sbyte value, int offset = 0)
    {
        target.AssertContains<sbyte>(offset);
        target.Array!.WriteSByte(value, offset);
    }

    #endregion

    #region Char

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Char ReadChar(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<char>(offset);
        return target.Array!.ReadChar(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteChar(this ArraySegment<byte> target, char value, int offset = 0)
    {
        target.AssertContains<char>(offset);
        target.Array!.WriteChar(value, offset);
    }

    #endregion

    #region Decimal

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Decimal ReadDecimal(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<decimal>(offset);
        return target.Array!.ReadDecimal(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteDecimal(this ArraySegment<byte> target, decimal value, int offset = 0)
    {
        target.AssertContains<decimal>(offset);
        target.Array!.WriteDecimal(value, offset);
    }

    #endregion

    #region Double

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Double ReadDouble(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<double>(offset);
        return target.Array!.ReadDouble(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteDouble(this ArraySegment<byte> target, double value, int offset = 0)
    {
        target.AssertContains<double>(offset);
        target.Array!.WriteDouble(value, offset);
    }

    #endregion

    #region Single

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Single ReadSingle(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<float>(offset);
        return target.Array!.ReadSingle(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteSingle(this ArraySegment<byte> target, float value, int offset = 0)
    {
        target.AssertContains<float>(offset);
        target.Array!.WriteSingle(value, offset);
    }

    #endregion

    #region Int32

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Int32 ReadInt32(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<int>(offset);
        return target.Array!.ReadInt32(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteInt32(this ArraySegment<byte> target, int value, int offset = 0)
    {
        target.AssertContains<int>(offset);
        target.Array!.WriteInt32(value, offset);
    }

    #endregion

    #region Uint32

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static UInt32 ReadUInt32(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<uint>(offset);
        return target.Array!.ReadUInt32(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteUInt32(this ArraySegment<byte> target, uint value, int offset = 0)
    {
        target.AssertContains<uint>(offset);
        target.Array!.WriteUInt32(value, offset);
    }

    #endregion

    #region Int64

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Int64 ReadInt64(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<long>(offset);
        return target.Array!.ReadInt64(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteInt64(this ArraySegment<byte> target, long value, int offset = 0)
    {
        target.AssertContains<long>(offset);
        target.Array!.WriteInt64(value, offset);
    }

    #endregion

    #region UInt64

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static UInt64 ReadUInt64(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<ulong>(offset);
        return target.Array!.ReadUInt64(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteUInt64(this ArraySegment<byte> target, ulong value, int offset = 0)
    {
        target.AssertContains<ulong>(offset);
        target.Array!.WriteUInt64(value, offset);
    }

    #endregion

    #region Int16

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Int16 ReadInt16(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<short>(offset);
        return target.Array!.ReadInt16(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteInt16(this ArraySegment<byte> target, short value, int offset = 0)
    {
        target.AssertContains<short>(offset);
        target.Array!.WriteInt16(value, offset);
    }

    #endregion

    #region UInt16

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static UInt16 ReadUInt16(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<ushort>(offset);
        return target.Array!.ReadUInt16(target.Offset + offset);
    }
    
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void WriteUInt16(this ArraySegment<byte> target, ushort value, int offset = 0)
    {
        target.AssertContains<ushort>(offset);
        target.Array!.WriteUInt16(value, offset);
    }

    #endregion
    
    #region Assertions

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertContains<TPrimitive>(this ArraySegment<byte> target, int offset) where TPrimitive : struct
    {
        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset must be greater than or equal to zero.");
        }
        target.AssertHasArray();
        target.AssertDoesNotEndBefore<TPrimitive>(offset);
    }

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertDoesNotEndBefore<TPrimitive>(this ArraySegment<byte> target, int offset) where TPrimitive : struct
    {
        int primitiveSize = PrimitiveSerializer.SizeOf<TPrimitive>();
        target.AssertDoesNotEndBefore(offset, primitiveSize);
    }

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertDoesNotEndBefore(this ArraySegment<byte> target, int offset, int count)
    {
        if (offset + count > target.Count)
        {
            throw new OutOfByteArraySegmentBoundsException(target, offset, count);
        }
    }

    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertHasArray(this ArraySegment<byte> target)
    {
        if (target.Array == null)
        {
            throw new ByteArraySegmentHasNoArrayException(nameof(target));
        }
    }

    #endregion
}