using System;
using MsbRpc.Messaging;

namespace MsbRpc.Servers.Listener;

public class InitialConnectionMessageReceiveFailedException : Exception
{
    public InitialConnectionMessageReceiveFailedException(ReceiveReturnCode receiveReturnCode) : base
    (
        $"The initial connection message could not be transmitted,"
        + $" and the connection is therefore discarded. The receive return code is: {receiveReturnCode.GetName()}"
    ) { }
}