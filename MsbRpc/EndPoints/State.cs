namespace MsbRpc.EndPoints;

public enum State
{
    IdleInbound,
    IdleOutbound,
    ListeningForRequests,
    SendingRequest,
    Disposed
}