using System.Net.Sockets;
using MsbRpc.Concurrent;

namespace MsbRpc;

public static class NetworkUtility
{
    public const int DynamicPortRangeMin = 49152;
    public const int DynamicPortRangeMax = 65535;

    public static UniqueIntProvider CreateUniquePortProvider(bool shuffle) => new(DynamicPortRangeMin, DynamicPortRangeMax, shuffle);

    public static Socket CreateTcpSocket(AddressFamily addressFamily) => new(addressFamily, SocketType.Stream, ProtocolType.Tcp);
}