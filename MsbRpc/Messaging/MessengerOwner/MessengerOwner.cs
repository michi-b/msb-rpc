namespace MsbRpc.Messaging.SocketOwners;

public abstract class MessengerOwner
{
    protected MessengerOwner(Messenger.Messenger messenger) => Messenger = messenger;
    
    protected Messenger.Messenger Messenger { get; }

    protected void DisposeSocket()
    {
        Messenger.Dispose();
    }
}