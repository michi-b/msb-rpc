namespace MsbRpc.EndPoints;

public enum State
{
    IdleInbound,
    IdleOutbound,
    Listening,
    Calling,
    Disposed
}