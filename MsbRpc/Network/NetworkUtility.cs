using JetBrains.Annotations;
using MsbRpc.Concurrent;

namespace MsbRpc;

public static class NetworkUtility
{
    [PublicAPI] public const int DynamicPortRangeMin = 49152;

    [PublicAPI] public const int DynamicPortRangeMax = 65535;

    public static UniqueIntProvider CreateUniquePortProvider(bool shuffle) => new(DynamicPortRangeMin, DynamicPortRangeMax, shuffle);
}