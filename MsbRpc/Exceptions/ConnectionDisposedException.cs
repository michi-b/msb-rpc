﻿#region

using System;
using MsbRpc.Sockets;

#endregion

namespace MsbRpc.Exceptions;

public class ConnectionDisposedException : Exception
{
    public ConnectionDisposedException(RpcSocket socket)
        : base(GetMessage(socket)) { }

    private static string GetMessage(RpcSocket socket) => $"The connection {socket} is already disposed.";
}