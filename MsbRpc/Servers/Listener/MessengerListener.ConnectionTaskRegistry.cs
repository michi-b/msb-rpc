using MsbRpc.Messaging;
using MsbRpc.Servers.Listener.ConnectionTask;

namespace MsbRpc.Servers.Listener;

public partial class MessengerListener
{
    private readonly struct ConnectionTaskRegistry
    {
        private readonly IdentifiedItemRegistry<ConnectionTask.ConnectionTask> _connectionTasks;
        private readonly MessengerListener _owner;

        public ConnectionTaskRegistry(MessengerListener listener)
        {
            _connectionTasks = new IdentifiedItemRegistry<ConnectionTask.ConnectionTask>();
            _owner = listener;
        }

        public IdentifiedConnectionTask Schedule()
        {
            ConnectionTask.ConnectionTask connectionTask = new();
            int id = _connectionTasks.Add(connectionTask);
            return new IdentifiedConnectionTask(connectionTask, id, _owner);
        }

        public void Complete(int id, Messenger messenger) => _connectionTasks.Take(id).Complete(messenger);
    }
}