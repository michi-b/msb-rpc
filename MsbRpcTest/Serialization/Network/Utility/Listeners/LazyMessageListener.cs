using System.Diagnostics.CodeAnalysis;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffer;

namespace MsbRpcTest.Serialization.Network.Utility.Listeners;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class LazyMessagesListener
{
    public static async Task<List<ArraySegment<byte>>> ListenAsync(Messenger messenger, CancellationToken cancellationToken)
    {
        ReceiveMessageResult receiveMessageResult;
        List<ArraySegment<byte>> messages = new();
        while ((receiveMessageResult = await messenger.ReceiveMessageAsync(BufferUtility.Create, cancellationToken)).ReturnCode
               == ReceiveMessageReturnCode.Success)
        {
            messages.Add(receiveMessageResult.Message);
        }

        return messages;
    }
}