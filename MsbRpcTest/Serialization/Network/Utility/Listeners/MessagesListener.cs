using System.Net;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffer;
using MsbRpc.Sockets;

namespace MsbRpcTest.Serialization.Network.Utility.Listeners;

public static class MessagesListener
{
    private static async Task<List<ArraySegment<byte>>> ListenAsync(EndPoint ep, int bufferSize, CancellationToken cancellationToken)
    {
        RpcSocket socket = await NetworkUtility.AcceptAsync(ep, cancellationToken);
        var messenger = new Messenger(socket);
        List<ArraySegment<byte>> messages = new();
        await messenger.ListenAsync
        (
            BufferUtility.Create,
            (buffer, _) =>
            {
                messages.Add(buffer);
                return Task.CompletedTask;
            },
            cancellationToken
        );
        return messages;
    }
}