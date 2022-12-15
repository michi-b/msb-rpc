using System.Diagnostics;
using JetBrains.Annotations;
using MsbRpc.Serialization.Exceptions;

namespace MsbRpc.Serialization.Buffers;

public static partial class BufferExtensions
{
    /// <exception cref="ByteArraySegmentHasNoArrayException"></exception>
    /// <exception cref="OutOfByteArraySegmentBoundsException"></exception>
    [AssertionMethod]
    [Conditional("DEBUG")]
    private static void AssertContains(this ArraySegment<byte> target, int count, int offset)
    {
        target.AssertHasArray();
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