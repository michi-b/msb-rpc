using System.Diagnostics.CodeAnalysis;
using Decimal = System.Decimal;

namespace MsbRps.Serialization.Primitives;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class PrimitiveSerializer
{
    private Union _union;
    private readonly int[] _decimalBits = new int[4];

    #region Boolean

    public static void WriteBoolean(Boolean value, byte[] buffer, int offset = 0)
    {
        buffer[offset] = value ? (byte)1 : (byte)0;
    }

    public static Boolean ReadBoolean(byte[] buffer, int offset = 0) => buffer[offset] == 1;

    #endregion

    #region Byte

    public static void WriteByte(Byte value, byte[] buffer, int offset = 0) => buffer[offset] = value;

    public static Byte ReadByte(byte[] buffer, int offset = 0) => buffer[offset];

    #endregion

    #region SByte

    public static void WriteSByte(SByte value, byte[] buffer, int offset = 0) => buffer[offset] = (Byte)value;

    public static SByte ReadSByte(byte[] buffer, int offset = 0) => (sbyte)buffer[offset];

    #endregion

    #region Char

    public void WriteChar(Char value, byte[] buffer, int offset = 0)
    {
        _union.WriteChar(value, buffer, offset);
    }

    public static Char ReadChar(byte[] buffer, int offset = 0) => BitConverter.ToChar(buffer, offset);

    #endregion

    #region Decimal
    public void WriteDecimal(Decimal value, byte[] buffer, int offset = 0)
    {
        int[] bits = Decimal.GetBits(value);
        WriteInt32(bits[0], buffer, offset);
        WriteInt32(bits[1], buffer, offset + 4);
        WriteInt32(bits[2], buffer, offset + 8);
        WriteInt32(bits[3], buffer, offset + 12);
    }

    public Decimal ReadDecimal(byte[] buffer, int offset = 0)
    {

        _decimalBits[0] = ReadInt32(buffer, offset);
        _decimalBits[1] = ReadInt32(buffer, offset + 4);
        _decimalBits[2] = ReadInt32(buffer, offset + 8);
        _decimalBits[3] = ReadInt32(buffer, offset + 12);
        return new decimal(_decimalBits);
    }

    #endregion

    #region Int32

    public void WriteInt32(Int32 value, byte[] buffer, int offset = 0)
    {
        _union.WriteInt32(value, buffer, offset);
    }

    public static int ReadInt32(byte[] buffer, Int32 offset = 0) => BitConverter.ToInt32(buffer, offset);

    #endregion
}