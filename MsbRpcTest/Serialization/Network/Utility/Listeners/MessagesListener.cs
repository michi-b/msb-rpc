using System.Diagnostics.CodeAnalysis;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.Serialization.Network.Utility.Listeners;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class MessagesListener
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
        List<ArraySegment<byte>> messages = new();
        var buffer = new RecycledBuffer();
        messenger.Listen
        (
            buffer,
            (message) =>
            {
                messages.Add(message.Copy());
                return false;
            }
        );
        return messages;
    }

    public static async Task<List<ArraySegment<byte>>> ListenAsync(Messenger messenger, CancellationToken cancellationToken)
    {
        List<ArraySegment<byte>> messages = new();
        var buffer = new RecycledBuffer();
        await messenger.ListenAsync
        (
            buffer,
            (message, _) =>
            {
                messages.Add(message.Copy());
                return ValueTask.FromResult(false);
            },
            cancellationToken
        );
        return messages;
    }
}