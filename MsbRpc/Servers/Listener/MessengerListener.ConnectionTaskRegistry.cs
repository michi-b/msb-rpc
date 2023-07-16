#region

using MsbRpc.Messaging;

#endregion

namespace MsbRpc.Servers.Listener;

public partial class MessengerListener
{
    private readonly struct ConnectionTaskRegistry
    {
        private readonly IdentifiedItemRegistry<ConnectionTask> _connectionTasks;
        private readonly MessengerListener _owner;

        public ConnectionTaskRegistry(MessengerListener listener)
        {
            _connectionTasks = new IdentifiedItemRegistry<ConnectionTask>();
            _owner = listener;
        }

        public IdentifiedConnectionTask Schedule()
        {
            ConnectionTask connectionTask = new();
            int id = _connectionTasks.Add(connectionTask);
            return new IdentifiedConnectionTask(connectionTask, id, _owner);
        }

        public void Complete(int id, Messenger messenger) => _connectionTasks.Take(id).Complete(messenger);
    }
}