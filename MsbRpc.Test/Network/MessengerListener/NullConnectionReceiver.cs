#region

using MsbRpc.Messaging;
using MsbRpc.Servers.Listener;

#endregion

namespace MsbRpc.Test.Network.MessengerListener;

/// <summary>
///     a connection receiver that immediately closes received connections
/// </summary>
public class NullConnectionReceiver : IConnectionReceiver
{
    public void AcceptUnIdentified(Messenger messenger)
    {
        messenger.Dispose();
    }
}