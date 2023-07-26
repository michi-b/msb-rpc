#region

using System;
using System.Threading.Tasks;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

#endregion

namespace MsbRpc.Servers.Listener;

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

    public static async ValueTask SendConnectionRequestAsync<TId>(this Messenger messenger, ConnectionRequest<TId> target, RpcBuffer buffer, Action<BufferWriter,TId> writeId) where TId : struct
    {
        Message message = target.WriteMessage(buffer, writeId);
        await messenger.SendAsync(message);
    }
}