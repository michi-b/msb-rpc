using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable CS0649

namespace MsbRpc.Serialization.Primitives;

public static partial class PrimitiveSerializer
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameterInConstructor")]
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct ByteConverter16
    {
        [FieldOffset(0)] private readonly Decimal _decimalValue;

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

        public ByteConverter16(Decimal value)
        {
            Unsafe.SkipInit(out this);
            _decimalValue = value;
        }

        public ByteConverter16(byte[] buffer, int offset)
        {
            Unsafe.SkipInit(out this);
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

        public void Write(byte[] buffer, int offset)
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

        public Decimal Read() => _decimalValue;
    }
}