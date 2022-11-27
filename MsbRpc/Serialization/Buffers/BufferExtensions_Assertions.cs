using System.Diagnostics;
using JetBrains.Annotations;
using MsbRpc.Serialization.Exceptions;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Buffers;

public static partial class BufferExtensions
{
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertContains<TPrimitive>(this ArraySegment<byte> target, int offset) where TPrimitive : struct
    {
        Debug.Assert(offset >= 0);
        target.AssertHasArray();
        target.AssertEndsAtOrBefore<TPrimitive>(offset);
    }

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertEndsAtOrBefore<TPrimitive>(this ArraySegment<byte> target, int offset) where TPrimitive : struct
    {
        int primitiveSize = PrimitiveSerializer.SizeOf<TPrimitive>();
        target.AssertEndsAtOrBefore(offset, primitiveSize);
    }

    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertEndsAtOrBefore(this ArraySegment<byte> target, int offset, int count)
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
}