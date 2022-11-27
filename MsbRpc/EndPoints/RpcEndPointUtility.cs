namespace MsbRpc.EndPoints;

public class RpcEndPointUtility
{
    internal static readonly State[] BufferRecyclableStates =
    {
        State.IdleInbound,
        State.IdleOutbound
    };
}