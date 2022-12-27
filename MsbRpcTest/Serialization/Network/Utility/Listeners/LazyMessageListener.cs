using System.Diagnostics.CodeAnalysis;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.Serialization.Network.Utility.Listeners;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class LazyMessagesListener
{
    public static Task<List<ArraySegment<byte>>> Listen(Messenger messenger, TaskCreationOptions options)
    {
        return Task.Factory.StartNew
        (
            () => Listen(messenger),
            CancellationToken.None,
            options,
            TaskScheduler.Default
        );
    }
    
    public static List<ArraySegment<byte>> Listen(Messenger messenger)
    {
        ReceiveMessageResult receiveMessageResult;
        List<ArraySegment<byte>> messages = new();
        var buffer = new RecycledBuffer();
        while ((receiveMessageResult = messenger.ReceiveMessage(buffer)).ReturnCode
               == ReceiveMessageReturnCode.Success)
        {
            messages.Add(receiveMessageResult.Message.Copy());
        }
        return messages;
    }    
    
    public static async Task<List<ArraySegment<byte>>> ListenAsync(Messenger messenger, CancellationToken cancellationToken)
    {
        ReceiveMessageResult receiveMessageResult;
        List<ArraySegment<byte>> messages = new();
        var buffer = new RecycledBuffer();
        while ((receiveMessageResult = await messenger.ReceiveMessageAsync(buffer, cancellationToken)).ReturnCode
               == ReceiveMessageReturnCode.Success)
        {
            messages.Add(receiveMessageResult.Message.Copy());
        }

        return messages;
    }
}