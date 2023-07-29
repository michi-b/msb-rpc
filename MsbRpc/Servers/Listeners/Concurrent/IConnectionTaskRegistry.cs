#region

using System.Collections.Generic;
using MsbRpc.Servers.Listeners.Connections;

#endregion

namespace MsbRpc.Servers.Listeners.Concurrent;

public interface IConnectionTaskRegistry<TId> where TId : struct
{
    KeyValuePair<TId, ConnectionTask> Add(ConnectionTask item);
    ConnectionTask Take(TId id);
}