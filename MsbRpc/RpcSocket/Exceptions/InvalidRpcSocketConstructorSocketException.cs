using System.Net.Sockets;

namespace MsbRpc.RpcSocket.Exceptions;

public class InvalidRpcSocketConstructorSocketException : ArgumentException
{
    public InvalidRpcSocketConstructorSocketException(Socket socket, string paramName)
     :base($"Invalid socket was used to construct a RpcSocket. "
           + $"Rpc sockets expect a connected socket of type SocketType.Stream and using ProtocolType.Tcp,"
           + $"but the provided socket was "
           + (socket.Connected ? "{connected}" : "{NOT connected}")
           + $", of type {{SocketType.{socket.SocketType}}} and using {{ProtocolType.{socket.ProtocolType}}}.",
         paramName)
    { }
}