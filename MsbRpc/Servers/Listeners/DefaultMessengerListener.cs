#region

using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Servers.Listeners.Concurrent;
using MsbRpc.Servers.Listeners.Connections;
using MsbRpc.Servers.Listeners.Connections.Generic;

#endregion

namespace MsbRpc.Servers.Listeners;

public class DefaultMessengerListener : ScheduledMessengerListener<int>
{
    public DefaultMessengerListener(MessengerListenerConfiguration configuration)
        : base(configuration, new IntIdentifiedConnectionTaskRegistry()) { }

    public static async ValueTask<Messenger> ConnectAsync(IPEndPoint remoteEndPoint, RpcBuffer buffer, ILogger? logger = null)
        => await ConnectAsync(remoteEndPoint, buffer, CreateUnIdentifiedConnectionRequest, logger);

    public static async ValueTask<Messenger> ConnectAsync(IPEndPoint remoteEndPoint, RpcBuffer buffer, int id, ILogger? logger = null)
        => await ConnectAsync(remoteEndPoint, buffer, CreateIdentifiedConnectionRequest, id, logger);

    private static ConnectionRequest<int> CreateIdentifiedConnectionRequest(int id) => new IntIdentifiedConnectionRequest(id);

    private static ConnectionRequest<int> CreateUnIdentifiedConnectionRequest() => new IntIdentifiedConnectionRequest();

    protected override ConnectionRequest<int> ReadConnectionRequest(Message message) => new IntIdentifiedConnectionRequest(message);
}