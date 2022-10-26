using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable CS0649

namespace MsbRpc.Serialization.Primitives;

public static partial class PrimitiveSerializer
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    [SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")]
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct ByteConverter8
    {
        [FieldOffset(0)] private readonly Double _doubleValue;
        [FieldOffset(0)] private readonly Int64 _int64Value;
        [FieldOffset(0)] private readonly UInt64 _uint64Value;

        [FieldOffset(0)] private readonly byte _byte0;
        [FieldOffset(1)] private readonly byte _byte1;
        [FieldOffset(2)] private readonly byte _byte2;
        [FieldOffset(3)] private readonly byte _byte3;
        [FieldOffset(4)] private readonly byte _byte4;
        [FieldOffset(5)] private readonly byte _byte5;
        [FieldOffset(6)] private readonly byte _byte6;
        [FieldOffset(7)] private readonly byte _byte7;

        public ByteConverter8(Double value)
        {
            Unsafe.SkipInit(out this);
            _doubleValue = value;
        }

        public ByteConverter8(Int64 value)
        {
            Unsafe.SkipInit(out this);
            _int64Value = value;
        }

        public ByteConverter8(UInt64 value)
        {
            Unsafe.SkipInit(out this);
            _uint64Value = value;
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
    }
}