using System;
using JetBrains.Annotations;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Serialization;

public static class ByteArrayUtility
{
    public const int DefaultSize = BufferUtility.DefaultSize;

    [PublicAPI] public static readonly byte[] Empty = Array.Empty<byte>();

    [PublicAPI]
    public static string ToString(params byte[] bytes) => bytes.CreateContentString();
}