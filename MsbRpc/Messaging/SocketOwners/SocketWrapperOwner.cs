using MsbRpc.Messaging.Sockets;

namespace MsbRpc.Messaging.SocketOwners;

public abstract class SocketWrapperOwner
{
    protected SocketWrapperOwner(SocketWrapper socket) => Socket = socket;
    
    protected SocketWrapper Socket { get; }

    protected void DisposeSocket()
    {
        Socket.Dispose();
    }
}