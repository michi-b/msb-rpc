﻿using System.Diagnostics.CodeAnalysis;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffer;

namespace MsbRpcTest.Serialization.Network.Utility.Listeners;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class MessagesListener
{
    public static async Task<List<ArraySegment<byte>>> ListenAsync(Messenger messenger, CancellationToken cancellationToken)
    {
        List<ArraySegment<byte>> messages = new();
        await messenger.ListenAsync
        (
            BufferUtility.Create,
            (message, _) =>
            {
                messages.Add(message);
                return Task.CompletedTask;
            },
            cancellationToken
        );
        return messages;
    }
}