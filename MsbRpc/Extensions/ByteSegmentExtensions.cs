#region

using System;

#endregion

namespace MsbRpc.Extensions;

public static class ByteSegmentExtensions
{
    public static ArraySegment<byte> Copy(this ArraySegment<byte> target)
    {
        byte[] bytes = new byte[target.Count];
        Buffer.BlockCopy(target.Array!, target.Offset, bytes, 0, target.Count);
        return new ArraySegment<byte>(bytes);
    }
}