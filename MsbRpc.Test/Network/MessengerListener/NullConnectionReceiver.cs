#region

using MsbRpc.Messaging;
using MsbRpc.Servers.Listeners.Connections;

#endregion

namespace MsbRpc.Test.Network.MessengerListener;

/// <summary>
///     a connection receiver that immediately closes received connections
/// </summary>
public class NullConnectionReceiver : IConnectionReceiver
{
    public void Accept(Messenger messenger)
    {
        messenger.Dispose();
    }
}