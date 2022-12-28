using System.Diagnostics.CodeAnalysis;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.Test.Network.Utility.Listeners;

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
            message =>
            {
                messages.Add(message.Copy());
                return false;
            }
        );
        return messages;
    }
}