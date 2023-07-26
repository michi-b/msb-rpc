#region

using MsbRpc.Messaging;

#endregion

namespace MsbRpc.Servers.Listener;

public interface IConnectionReceiver
{
    public void Accept(Messenger messenger);
}