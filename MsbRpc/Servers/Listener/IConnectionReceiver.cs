#region

using MsbRpc.Messaging;

#endregion

namespace MsbRpc.Servers.Listener;

public interface IConnectionReceiver
{
    public void AcceptUnIdentified(Messenger messenger);
}