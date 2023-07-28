using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Servers.Listeners.Concurrent;
using MsbRpc.Servers.Listeners.Connections;
using MsbRpc.Servers.Listeners.Connections.Generic;

namespace MsbRpc.Servers.Listeners;

public class DefaultMessengerListener : ScheduledMessengerListener<int>
{
    public DefaultMessengerListener(MessengerListenerConfiguration configuration)
        : base(configuration, new IntIdentifiedItemRegistry<ConnectionTask>()) { }

    public static async ValueTask<Messenger> ConnectAsync(IPEndPoint remoteEndPoint, RpcBuffer buffer, ILogger? logger = null)
    {
        Messenger messenger = await Messenger.ConnectAsync(remoteEndPoint, logger);
        await messenger.SendAsync(new IntIdentifiedConnectionRequest().GetMessage(buffer));
        var connectionResult = (ConnectionResult)(await messenger.ReceiveMessageAsync(buffer)).GetReader().ReadInt();
        if (connectionResult != ConnectionResult.Okay)
        {
            messenger.Dispose();
            throw new ConnectionFailedException(connectionResult);
        }

        return messenger;
    }

    protected override ConnectionRequest<int> ReadConnectionRequest(Message message) => new IntIdentifiedConnectionRequest(message);
}