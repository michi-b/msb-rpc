using System;
using JetBrains.Annotations;

namespace MsbRpc.Serialization.Buffers;

public static class BufferUtility
{
    public const int DefaultSize = 1024;

    [PublicAPI] public static readonly ArraySegment<byte> Empty = new(ByteArrayUtility.Empty, 0, 0);

    [PublicAPI]
    public static ArraySegment<byte> Create(int count) => new(new byte[count]);

    public static ArraySegment<byte> Create(byte value) => new(new[] { value });
}