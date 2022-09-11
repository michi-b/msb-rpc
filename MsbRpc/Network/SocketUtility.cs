using System.Net;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace MsbRpc.Messaging;

public static class SocketUtility
{
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

    public static async Task<Socket> Connect(EndPoint remote,
        int sendBufferSize = DefaultSocketSendBufferSize,
        int receiveBufferSize = DefaultSocketReceiveBufferSize
    )
    {
        Socket socket = SocketUtility.CreateTcpSocket(remote.AddressFamily, sendBufferSize, receiveBufferSize);
        await socket.ConnectAsync(remote);
        return socket;
    }

    [PublicAPI] public const int DefaultSocketSendBufferSize = 8192;
    [PublicAPI] public const int DefaultSocketReceiveBufferSize = 8192;
}