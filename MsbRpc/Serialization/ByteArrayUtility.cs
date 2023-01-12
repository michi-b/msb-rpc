using System;
using JetBrains.Annotations;
using MsbRpc.EndPoints;
using MsbRpc.EndPoints.Configuration;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Serialization;

public static class ByteArrayUtility
{
    public const int DefaultSize = EndPointConfiguration.DefaultInitialSize;

    [PublicAPI] public static readonly byte[] Empty = Array.Empty<byte>();

    [PublicAPI]
    public static string ToString(params byte[] bytes) => bytes.CreateContentString();
}