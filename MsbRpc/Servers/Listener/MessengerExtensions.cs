#region

using System.Threading.Tasks;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

#endregion

namespace MsbRpc.Servers.Listener;

public static class MessengerExtensions
{
    public static async ValueTask<ConnectionRequest> ReceiveConnectionRequestAsync(this Messenger messenger, RpcBuffer buffer)
    {
        ReceiveResult receiveResult = await messenger.ReceiveAsync(buffer);
        ReceiveReturnCode receiveReturnCode = receiveResult.ReturnCode;

        if (receiveReturnCode != ReceiveReturnCode.Success)
        {
            throw new ReceiveConnectionRequestFailedException(receiveReturnCode);
        }

        return ConnectionRequest.Read(receiveResult.Message);
    }

    public static async ValueTask SendConnectionRequest(this Messenger messenger, ConnectionRequest target, RpcBuffer buffer)
    {
        Message message = target.Write(buffer);
        await messenger.SendAsync(message);
    }
}