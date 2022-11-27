using MsbRpc.Messaging;

namespace MsbRpc.EndPoints;

public abstract class RpcClient<TProcedureId> : RpcEndPoint<TProcedureId> where TProcedureId : Enum
{
    // servers always start in outbound state
    protected RpcClient(Messenger messenger, int initialBufferSize = DefaultBufferSize) : base
        (messenger, Direction.Outbound, initialBufferSize) { }
}