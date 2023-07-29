#region

using System;
using MsbRpc.Sockets;

#endregion

namespace MsbRpc.Exceptions;

public class ConnectionClosedException : Exception
{
    public ConnectionClosedException(RpcSocket socket)
        : base(GetMessage(socket)) { }

    private static string GetMessage(RpcSocket socket) => $"The connection {socket} was closed unexpectedly.";
}