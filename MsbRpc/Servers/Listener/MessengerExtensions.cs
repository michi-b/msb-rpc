using System.Threading.Tasks;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Servers.Listener;

public static class MessengerExtensions
{
    public static async ValueTask<InitialConnectionMessage> ReceiveInitialConnectionMessageAsync(this Messenger messenger, RpcBuffer buffer)
    {
        ReceiveResult receiveResult = await messenger.ReceiveAsync(buffer);
        ReceiveReturnCode receiveReturnCode = receiveResult.ReturnCode;

        if (receiveReturnCode != ReceiveReturnCode.Success)
        {
            throw new InitialConnectionMessageReceiveFailedException(receiveReturnCode);
        }

        return InitialConnectionMessage.Read(receiveResult.Message);
    }

    public static async ValueTask SendInitialConnectionMessage(this Messenger messenger, InitialConnectionMessage target, RpcBuffer buffer)
    {
        Message message = target.Write(buffer);
        await messenger.SendAsync(message);
    }
}