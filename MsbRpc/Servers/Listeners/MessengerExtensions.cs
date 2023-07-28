#region

using System;
using System.Threading.Tasks;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Servers.Listeners.Connections.Generic;

#endregion

namespace MsbRpc.Servers.Listeners;

public static class MessengerExtensions
{
    public static async ValueTask<ConnectionRequest<TId>> ReceiveConnectionRequestAsync<TId>
    (
        this Messenger messenger,
        RpcBuffer buffer,
        Func<Message, ConnectionRequest<TId>> readConnectionRequest
    ) where TId : struct
    {
        ReceiveResult receiveResult = await messenger.ReceiveAsync(buffer);
        ReceiveReturnCode receiveReturnCode = receiveResult.ReturnCode;

        if (receiveReturnCode != ReceiveReturnCode.Success)
        {
            throw new ReceiveConnectionRequestFailedException(receiveReturnCode);
        }

        return readConnectionRequest(receiveResult.Message);
    }
}