using System.Net.Sockets;
using MsbRpc.Concurrent;

namespace MsbRpc;

public static class NetworkUtility
{
    public const int DynamicPortRangeMin = 49152;
    public const int DynamicPortRangeMax = 65535;
    public const int DefaultSocketSendBufferSize = 8192;
    public const int DefaultSocketReceiveBufferSize = 8192;

    public static UniqueIntProvider CreateUniquePortProvider(bool shuffle) => new(DynamicPortRangeMin, DynamicPortRangeMax, shuffle);

    public static Socket CreateTcpSocket
    (
        AddressFamily addressFamily,
        int sendBufferSize = DefaultSocketSendBufferSize,
        int receiveBufferSize = DefaultSocketReceiveBufferSize
    )
    {
        Socket socket = new(addressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.SendBufferSize = sendBufferSize;
        socket.ReceiveBufferSize = receiveBufferSize;
        return socket;
    }
}