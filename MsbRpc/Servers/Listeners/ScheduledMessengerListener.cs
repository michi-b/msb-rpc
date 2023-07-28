#region

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;
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
    private readonly Func<Message, ConnectionRequest<TId>> _readConnectionRequest;
    private readonly IConcurrentIdentifiedItemRegistry<TId, ConnectionTask> _registry;
    protected RpcBuffer Buffer;

    protected ScheduledMessengerListener
    (
        MessengerListenerConfiguration configuration,
        IConcurrentIdentifiedItemRegistry<TId, ConnectionTask> registry
    ) : base(configuration)
    {
        _registry = registry;
        _readConnectionRequest = ReadConnectionRequest;
        Buffer = new RpcBuffer();
    }

    protected abstract ConnectionRequest<TId> ReadConnectionRequest(Message message);

    protected override async Task<bool> Intercept(Messenger messenger)
    {
        ConnectionRequest<TId> connectionRequest = await messenger.ReceiveConnectionRequestAsync(Buffer, _readConnectionRequest);

        switch (connectionRequest.ConnectionRequestType)
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