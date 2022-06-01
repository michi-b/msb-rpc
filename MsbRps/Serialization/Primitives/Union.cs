using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MsbRps.Serialization.Primitives;

[StructLayout(LayoutKind.Explicit)]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
internal struct Union
{
    [FieldOffset(0)] public Int32 Int32Value;

    [FieldOffset(0)] private byte _byte0;
    [FieldOffset(1)] private byte _byte1;
    [FieldOffset(2)] private byte _byte2;
    [FieldOffset(3)] private byte _byte3;
    [FieldOffset(4)] private byte _byte4;
    [FieldOffset(5)] private byte _byte5;
    [FieldOffset(6)] private byte _byte6;
    [FieldOffset(7)] private byte _byte7;
    [FieldOffset(8)] private byte _byte8;
    [FieldOffset(9)] private byte _byte9;
    [FieldOffset(10)] private byte _byte10;
    [FieldOffset(11)] private byte _byte11;
    [FieldOffset(12)] private byte _byte12;
    [FieldOffset(13)] private byte _byte13;
    [FieldOffset(14)] private byte _byte14;
    [FieldOffset(15)] private byte _byte15;

    internal void Read2Bytes(byte[] buffer, int offset)
    {
        _byte0 = buffer[offset];
        _byte1 = buffer[offset + 1];
    }

    internal void Write2Bytes(byte[] buffer, int offset)
    {
        buffer[offset] = _byte0;
        buffer[offset + 1] = _byte1;
    }

    internal void Read4Bytes(byte[] buffer, int offset)
    {
        _byte0 = buffer[offset];
        _byte1 = buffer[offset + 1];
        _byte2 = buffer[offset + 2];
        _byte3 = buffer[offset + 3];
    }

    internal void Write4Bytes(byte[] buffer, int offset)
    {
        buffer[offset] = _byte0;
        buffer[offset + 1] = _byte1;
        buffer[offset + 2] = _byte2;
        buffer[offset + 3] = _byte3;
    }

    internal void Read8Bytes(byte[] buffer, int offset)
    {
        _byte0 = buffer[offset];
        _byte1 = buffer[offset + 1];
        _byte2 = buffer[offset + 2];
        _byte3 = buffer[offset + 3];
        _byte4 = buffer[offset + 4];
        _byte5 = buffer[offset + 5];
        _byte6 = buffer[offset + 6];
        _byte7 = buffer[offset + 7];
    }

    internal void Write8Bytes(byte[] buffer, int offset)
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

    internal void Read16Bytes(byte[] buffer, int offset)
    {
        _byte0 = buffer[offset];
        _byte1 = buffer[offset + 1];
        _byte2 = buffer[offset + 2];
        _byte3 = buffer[offset + 3];
        _byte4 = buffer[offset + 4];
        _byte5 = buffer[offset + 5];
        _byte6 = buffer[offset + 6];
        _byte7 = buffer[offset + 7];
        _byte8 = buffer[offset + 8];
        _byte9 = buffer[offset + 9];
        _byte10 = buffer[offset + 10];
        _byte11 = buffer[offset + 11];
        _byte12 = buffer[offset + 12];
        _byte13 = buffer[offset + 13];
        _byte14 = buffer[offset + 14];
        _byte15 = buffer[offset + 15];
    }

    internal void Write16Bytes(byte[] buffer, int offset)
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
}