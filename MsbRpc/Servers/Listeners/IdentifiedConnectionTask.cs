#region

using MsbRpc.Messaging;
using MsbRpc.Servers.Listeners.Connections;

#endregion

namespace MsbRpc.Servers.Listeners;

public class IdentifiedConnectionTask
{
    private readonly ConnectionTask _connectionTask;
    private readonly MessengerListener _owner;
    public int Id { get; }

    internal IdentifiedConnectionTask(ConnectionTask connectionTask, int id, MessengerListener owner)
    {
        _connectionTask = connectionTask;
        _owner = owner;
        Id = id;
    }

    public Messenger Await(int millisecondsTimeout = ConnectionTask.DefaultMillisecondsTimeout)
    {
        Messenger messenger = _connectionTask.Await(millisecondsTimeout);
        _owner.LogAcceptedNewIdentifiedConnection(Id);
        return messenger;
    }
}