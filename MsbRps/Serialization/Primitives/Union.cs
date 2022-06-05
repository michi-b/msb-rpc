using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MsbRps.Serialization.Primitives;

[StructLayout(LayoutKind.Explicit)]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
internal struct Union
{
    private static readonly bool IsLittleEndian = BitConverter.IsLittleEndian;

    [FieldOffset(0)] private Int32 _int32Value;
    [FieldOffset(0)] private Char _charValue;
    [FieldOffset(0)] private Decimal _decimalValue;

    [FieldOffset(0)] private readonly byte _byte0;
    [FieldOffset(1)] private readonly byte _byte1;
    [FieldOffset(2)] private readonly byte _byte2;
    [FieldOffset(3)] private readonly byte _byte3;
    [FieldOffset(4)] private readonly byte _byte4;
    [FieldOffset(5)] private readonly byte _byte5;
    [FieldOffset(6)] private readonly byte _byte6;
    [FieldOffset(7)] private readonly byte _byte7;
    [FieldOffset(8)] private readonly byte _byte8;
    [FieldOffset(9)] private readonly byte _byte9;
    [FieldOffset(10)] private readonly byte _byte10;
    [FieldOffset(11)] private readonly byte _byte11;
    [FieldOffset(12)] private readonly byte _byte12;
    [FieldOffset(13)] private readonly byte _byte13;
    [FieldOffset(14)] private readonly byte _byte14;
    [FieldOffset(15)] private readonly byte _byte15;

    private void Write2Bytes(byte[] buffer, int offset)
    {
        if (IsLittleEndian)
        {
            buffer[offset] = _byte0;
            buffer[offset + 1] = _byte1;
        }
        else
        {
            buffer[offset] = _byte1;
            buffer[offset + 1] = _byte0;
        }
    }

    private void Write4Bytes(byte[] buffer, int offset)
    {
        if (IsLittleEndian)
        {
            buffer[offset] = _byte0;
            buffer[offset + 1] = _byte1;
            buffer[offset + 2] = _byte2;
            buffer[offset + 3] = _byte3;
        }
        else
        {
            buffer[offset] = _byte3;
            buffer[offset + 1] = _byte2;
            buffer[offset + 2] = _byte1;
            buffer[offset + 3] = _byte0;
        }
    }

    private void Write8Bytes(byte[] buffer, int offset)
    {
        if (IsLittleEndian)
        {
            buffer[offset] = _byte0;
            buffer[offset + 1] = _byte1;
            buffer[offset + 2] = _byte2;
            buffer[offset + 3] = _byte3;
            buffer[offset + 4] = _byte4;
            buffer[offset + 5] = _byte5;
            buffer[offset + 6] = _byte6;
            buffer[offset + 7] = _byte7;
        }
        else
        {
            buffer[offset] = _byte7;
            buffer[offset + 1] = _byte6;
            buffer[offset + 2] = _byte5;
            buffer[offset + 3] = _byte4;
            buffer[offset + 4] = _byte3;
            buffer[offset + 5] = _byte2;
            buffer[offset + 6] = _byte1;
            buffer[offset + 7] = _byte0;
        }
    }

    private void Write16Bytes(byte[] buffer, int offset)
    {
        if (IsLittleEndian)
        {
            buffer[offset] = _byte0;
            buffer[offset + 1] = _byte1;
            buffer[offset + 2] = _byte2;
            buffer[offset + 3] = _byte3;
            buffer[offset + 4] = _byte4;
            buffer[offset + 5] = _byte5;
            buffer[offset + 6] = _byte6;
            buffer[offset + 7] = _byte7;
            buffer[offset + 8] = _byte8;
            buffer[offset + 9] = _byte9;
            buffer[offset + 10] = _byte10;
            buffer[offset + 11] = _byte11;
            buffer[offset + 12] = _byte12;
            buffer[offset + 13] = _byte13;
            buffer[offset + 14] = _byte14;
            buffer[offset + 15] = _byte15;
        }
        else
        {
            buffer[offset] = _byte15;
            buffer[offset + 1] = _byte14;
            buffer[offset + 2] = _byte13;
            buffer[offset + 3] = _byte12;
            buffer[offset + 4] = _byte11;
            buffer[offset + 5] = _byte10;
            buffer[offset + 6] = _byte9;
            buffer[offset + 7] = _byte8;
            buffer[offset + 8] = _byte7;
            buffer[offset + 9] = _byte6;
            buffer[offset + 10] = _byte5;
            buffer[offset + 11] = _byte4;
            buffer[offset + 12] = _byte3;
            buffer[offset + 13] = _byte2;
            buffer[offset + 14] = _byte1;
            buffer[offset + 15] = _byte0;
        }
    }

    public void WriteInt32(Int32 value, byte[] buffer, int offset)
    {
        _int32Value = value;
        Write4Bytes(buffer, offset);
    }

    public void WriteChar(Char value, byte[] buffer, int offset)
    {
        _charValue = value;
        Write2Bytes(buffer, offset);
    }

    public void WriteDecimal(Decimal value, byte[] buffer, int offset)
    {
        _decimalValue = value;
        Write16Bytes(buffer, offset);
    }
}