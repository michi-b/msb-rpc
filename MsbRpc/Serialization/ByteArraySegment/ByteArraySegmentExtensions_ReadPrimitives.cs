using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.ByteArraySegment;

public static partial class ByteArraySegmentExtensions
{
    #region Boolean

    public static bool ReadBoolean(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        return target.Array!.ReadBoolean(target.Offset + offset);
    }

    public static void WriteBoolean(this ArraySegment<byte> target, bool value, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        target.Array!.WriteBoolean(value, offset);
    }

    #endregion

    #region Byte

    public static byte ReadByte(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        return target.Array!.ReadByte(target.Offset + offset);
    }

    public static void WriteByte(this ArraySegment<byte> target, byte value, int offset = 0)
    {
        target.AssertContains<byte>(offset);
        target.Array!.WriteByte(value, offset);
    }

    #endregion

    #region SByte

    public static sbyte ReadSByte(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<sbyte>(offset);
        return target.Array!.ReadSByte(target.Offset + offset);
    }

    public static void WriteSByte(this ArraySegment<byte> target, sbyte value, int offset = 0)
    {
        target.AssertContains<sbyte>(offset);
        target.Array!.WriteSByte(value, offset);
    }

    #endregion

    #region Char

    public static char ReadChar(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<char>(offset);
        return target.Array!.ReadChar(target.Offset + offset);
    }

    public static void WriteChar(this ArraySegment<byte> target, char value, int offset = 0)
    {
        target.AssertContains<char>(offset);
        target.Array!.WriteChar(value, offset);
    }

    #endregion

    #region Decimal

    public static decimal ReadDecimal(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<decimal>(offset);
        return target.Array!.ReadDecimal(target.Offset + offset);
    }

    public static void WriteDecimal(this ArraySegment<byte> target, decimal value, int offset = 0)
    {
        target.AssertContains<decimal>(offset);
        target.Array!.WriteDecimal(value, offset);
    }

    #endregion

    #region Double

    public static double ReadDouble(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<double>(offset);
        return target.Array!.ReadDouble(target.Offset + offset);
    }

    public static void WriteDouble(this ArraySegment<byte> target, double value, int offset = 0)
    {
        target.AssertContains<double>(offset);
        target.Array!.WriteDouble(value, offset);
    }

    #endregion

    #region Single

    public static float ReadSingle(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<float>(offset);
        return target.Array!.ReadSingle(target.Offset + offset);
    }

    public static void WriteSingle(this ArraySegment<byte> target, float value, int offset = 0)
    {
        target.AssertContains<float>(offset);
        target.Array!.WriteSingle(value, offset);
    }

    #endregion

    #region Int32

    public static int ReadInt32(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<int>(offset);
        return target.Array!.ReadInt32(target.Offset + offset);
    }

    public static void WriteInt32(this ArraySegment<byte> target, int value, int offset = 0)
    {
        target.AssertContains<int>(offset);
        target.Array!.WriteInt32(value, offset);
    }

    #endregion

    #region Uint32

    public static uint ReadUInt32(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<uint>(offset);
        return target.Array!.ReadUInt32(target.Offset + offset);
    }

    public static void WriteUInt32(this ArraySegment<byte> target, uint value, int offset = 0)
    {
        target.AssertContains<uint>(offset);
        target.Array!.WriteUInt32(value, offset);
    }

    #endregion

    #region Int64

    public static long ReadInt64(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<long>(offset);
        return target.Array!.ReadInt64(target.Offset + offset);
    }

    public static void WriteInt64(this ArraySegment<byte> target, long value, int offset = 0)
    {
        target.AssertContains<long>(offset);
        target.Array!.WriteInt64(value, offset);
    }

    #endregion

    #region UInt64

    public static ulong ReadUInt64(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<ulong>(offset);
        return target.Array!.ReadUInt64(target.Offset + offset);
    }

    public static void WriteUInt64(this ArraySegment<byte> target, ulong value, int offset = 0)
    {
        target.AssertContains<ulong>(offset);
        target.Array!.WriteUInt64(value, offset);
    }

    #endregion

    #region Int16

    public static short ReadInt16(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<short>(offset);
        return target.Array!.ReadInt16(target.Offset + offset);
    }

    public static void WriteInt16(this ArraySegment<byte> target, short value, int offset = 0)
    {
        target.AssertContains<short>(offset);
        target.Array!.WriteInt16(value, offset);
    }

    #endregion

    #region UInt16

    public static ushort ReadUInt16(this ArraySegment<byte> target, int offset = 0)
    {
        target.AssertContains<ushort>(offset);
        return target.Array!.ReadUInt16(target.Offset + offset);
    }

    public static void WriteUInt16(this ArraySegment<byte> target, ushort value, int offset = 0)
    {
        target.AssertContains<ushort>(offset);
        target.Array!.WriteUInt16(value, offset);
    }

    #endregion
}