using System;
using JetBrains.Annotations;

namespace MsbRpc.Serialization;

public static class ByteArrayUtility
{
    [PublicAPI] public static readonly byte[] Empty = Array.Empty<byte>();

    [PublicAPI]
    public static string ToString(params byte[] bytes) => bytes.CreateContentString();
}