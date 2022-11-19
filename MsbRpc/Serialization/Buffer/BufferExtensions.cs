// ReSharper disable BuiltInTypeReferenceStyle

using System.Diagnostics;
using JetBrains.Annotations;

namespace MsbRpc.Serialization.Buffer;

public static partial class BufferExtensions
{
    [PublicAPI]
    public static ArraySegment<byte> GetSubSegment(this ArraySegment<byte> target, int offset) => target.GetSubSegment(offset, target.Count - offset);

    [PublicAPI]
    public static ArraySegment<byte> GetSubSegment(this ArraySegment<byte> target, int offset, int count)
    {
        Debug.Assert(offset > 0);
        Debug.Assert(count > 0);
        Debug.Assert(target.Array != null);
        target.AssertEndsAtOrBefore(offset, count);
        return new ArraySegment<byte>(target.Array!, target.Offset + offset, count);
    }

    public static ArraySegment<byte> ExtractSubSegmentCopy(this ArraySegment<byte> source, int count) => source.ExtractSubSegmentCopy(0, count);

    public static ArraySegment<byte> ExtractSubSegmentCopy(this ArraySegment<byte> source, int sourceOffset, int count)
    {
        var ret = new ArraySegment<byte>(new byte[count], 0, count);
        System.Buffer.BlockCopy(source.Array!, source.Offset + sourceOffset, ret.Array!, 0, count);
        return ret;
    }

    public static string CreateContentString(this ArraySegment<byte> target)
    {
        byte[] array = target.Array!;
        return array.CreateContentString(target.Offset, target.Count);
    }

    public static BufferReader CreateReader(this ArraySegment<byte> target) => new(target);

    public static BufferWriter CreateWriter(this ArraySegment<byte> target) => new(target);
}