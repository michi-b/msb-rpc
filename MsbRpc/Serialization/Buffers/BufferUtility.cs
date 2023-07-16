#region

using System;
using JetBrains.Annotations;

#endregion

namespace MsbRpc.Serialization.Buffers;

public static class BufferUtility
{
    [PublicAPI] public static readonly ArraySegment<byte> Empty = new(ByteArrayUtility.Empty, 0, 0);

    public static Message CreateMessage(int count) => new(new byte[count + Message.Offset], count);

    public static ArraySegment<byte> Create(int count) => new(new byte[count], 0, count);
}