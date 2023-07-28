#region

using MsbRpc.Messaging;

#endregion

namespace MsbRpc.Servers.Listeners.Connections;

public interface IConnectionReceiver
{
    public void Accept(Messenger messenger);
}