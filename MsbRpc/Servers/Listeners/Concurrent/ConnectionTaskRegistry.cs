#region

using System.Collections.Generic;
using MsbRpc.Servers.Listeners.Connections;

#endregion

namespace MsbRpc.Servers.Listeners.Concurrent;

public abstract class ConnectionTaskRegistry<TId> :
    ConcurrentIdentifiedItemRegistry<TId, ConnectionTask>,
    IConnectionTaskRegistry<TId> where TId : struct
{
    KeyValuePair<TId, ConnectionTask> IConnectionTaskRegistry<TId>.Add(ConnectionTask item)
    {
        ConnectionTask connectionTask = new();
        TId id = Add(connectionTask);
        return new KeyValuePair<TId, ConnectionTask>(id, connectionTask);
    }
}