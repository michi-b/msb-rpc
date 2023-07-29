#region

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
using MsbRpc.Exceptions;
using MsbRpc.Extensions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Servers.Listeners.Concurrent;
using MsbRpc.Servers.Listeners.Connections;
using MsbRpc.Servers.Listeners.Connections.Generic;

#endregion

namespace MsbRpc.Servers.Listeners;

public abstract class ScheduledMessengerListener<TId> : MessengerListener where TId : struct
{
    private readonly RpcBuffer _buffer;
    private readonly Func<Message, ConnectionRequest<TId>> _readConnectionRequest;
    private readonly IConnectionTaskRegistry<TId> _registry;

    protected ScheduledMessengerListener
    (
        MessengerListenerConfiguration configuration,
        IConnectionTaskRegistry<TId> registry
    ) : base(configuration)
    {
        _registry = registry;
        _readConnectionRequest = ReadConnectionRequest;
        _buffer = new RpcBuffer();
    }

    public KeyValuePair<TId, ConnectionTask> Schedule() => _registry.Add(new ConnectionTask());

    protected static async ValueTask<Messenger> ConnectAsync
    (
        IPEndPoint remoteEndPoint,
        RpcBuffer buffer,
        Func<TId, ConnectionRequest<TId>> createIdentifiedConnectionRequest,
        TId id,
        ILogger? logger = null
    )
    {
        Messenger messenger = await Messenger.ConnectAsync(remoteEndPoint, logger);
        await messenger.SendAsync(createIdentifiedConnectionRequest(id).GetMessage(buffer));
        var connectionResult = (ConnectionResult)(await messenger.ReceiveMessageAsync(buffer)).GetReader().ReadInt();
        if (connectionResult != ConnectionResult.Okay)
        {
            messenger.Dispose();
            throw new ConnectionFailedException(connectionResult);
        }

        return messenger;
    }

    protected static async ValueTask<Messenger> ConnectAsync
    (
        IPEndPoint remoteEndPoint,
        RpcBuffer buffer,
        Func<ConnectionRequest<TId>> createUnIdentifiedConnectionRequest,
        ILogger? logger = null
    )
    {
        Messenger messenger = await Messenger.ConnectAsync(remoteEndPoint, logger);
        await messenger.SendAsync(createUnIdentifiedConnectionRequest().GetMessage(buffer));
        var connectionResult = (ConnectionResult)(await messenger.ReceiveMessageAsync(buffer)).GetReader().ReadInt();
        if (connectionResult != ConnectionResult.Okay)
        {
            messenger.Dispose();
            throw new ConnectionFailedException(connectionResult);
        }

        return messenger;
    }

    protected abstract ConnectionRequest<TId> ReadConnectionRequest(Message message);

    /// <inheritdoc cref="MessengerListener.Accept(Messenger)" />
    /// >
    protected override async Task<bool> Accept(Messenger messenger)
    {
        ConnectionRequest<TId> connectionRequest = await messenger.ReceiveConnectionRequestAsync(_buffer, _readConnectionRequest);

        switch (connectionRequest.Type)
        {
            case ConnectionRequestType.UnIdentified:
                return false;
            case ConnectionRequestType.Identified:
                if (connectionRequest.Id == null)
                {
                    throw new InvalidIdentifiedConnectionRequestException<TId>(connectionRequest, "connection message is marked to be identified but has no ID");
                }

                try
                {
                    ConnectionTask connectionTask = _registry.Take(connectionRequest.Id.Value);
                    connectionTask.Complete(messenger);
                    LogCompletedIdentifiedConnectionTask(connectionRequest.Id.Value);
                }
                catch (Exception e)
                {
                    throw new InvalidIdentifiedConnectionRequestException<TId>
                    (
                        connectionRequest,
                        $"registered listen tasks do not contain an entry for the id {connectionRequest.Id}",
                        e
                    );
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ConnectionRequestType));
        }

        return true;
    }

    private void LogCompletedIdentifiedConnectionTask(TId id)
    {
        if (Logger != null)
        {
            LogConfiguration configuration = Configuration.LogCompletedIdentifiedConnectionTask;
            if (Logger.GetIsEnabled(configuration))
            {
                Logger.Log
                (
                    configuration.Level,
                    configuration.Id,
                    "{LoggingName} completed identified connection task with id {ConnectionId}",
                    Configuration.LoggingName,
                    id
                );
            }
        }
    }
}