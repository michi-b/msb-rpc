#region

using System;
using JetBrains.Annotations;

#endregion

namespace MsbRpc.Serialization;

public static class MemoryUtility
{
    [PublicAPI] public static readonly byte[] Empty = Array.Empty<byte>();

    [PublicAPI]
    public static string ToString(params byte[] bytes) => bytes.CreateContentString();
}