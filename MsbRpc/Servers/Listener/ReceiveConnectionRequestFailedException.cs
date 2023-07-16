#region

using System;
using MsbRpc.Messaging;

#endregion

namespace MsbRpc.Servers.Listener;

public class ReceiveConnectionRequestFailedException : Exception
{
    public ReceiveConnectionRequestFailedException(ReceiveReturnCode receiveReturnCode) : base
    (
        $"The initial connection message could not be transmitted,"
        + $" and the connection is therefore discarded. The receive return code is: {receiveReturnCode.GetName()}"
    ) { }
}