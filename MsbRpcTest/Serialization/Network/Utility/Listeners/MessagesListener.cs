using System.Diagnostics.CodeAnalysis;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.Serialization.Network.Utility.Listeners;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class MessagesListener
{
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
                return Task.CompletedTask;
            },
            cancellationToken
        );
        return messages;
    }
}