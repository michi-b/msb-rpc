using System.Threading.Tasks;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Servers.Listener;

public static class MessengerExtensions
{
    public static async Task<InitialConnectionMessage> ReceiveInitialConnectionMessageAsync(this Messenger messenger)
    {
        RpcBuffer buffer = new(InitialConnectionMessage.MessageMaxSize);

        ReceiveResult receiveResult = await messenger.ReceiveMessageAsync(buffer);
        ReceiveReturnCode receiveReturnCode = receiveResult.ReturnCode;

        if (receiveReturnCode != ReceiveReturnCode.Success)
        {
            throw new InitialConnectionMessageReceiveFailedException(receiveReturnCode);
        }

        Message message = receiveResult.Message;

        return new InitialConnectionMessage(message.Buffer);
    }
}