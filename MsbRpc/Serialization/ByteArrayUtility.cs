using System;
using JetBrains.Annotations;
using MsbRpc.EndPoints.Configuration;

namespace MsbRpc.Serialization;

public static class ByteArrayUtility
{
    public const int DefaultSize = EndPointConfiguration.DefaultInitialSize;

    [PublicAPI] public static readonly byte[] Empty = Array.Empty<byte>();

    [PublicAPI]
    public static string ToString(params byte[] bytes) => bytes.CreateContentString();
}