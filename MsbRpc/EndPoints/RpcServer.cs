using MsbRpc.Messaging;

namespace MsbRpc.EndPoints;

public abstract class RpcServer<TProcedureId> : RpcEndPoint<TProcedureId> where TProcedureId : Enum
{
    // servers always start in inbound state
    protected RpcServer(Messenger messenger, int initialBufferSize = DefaultBufferSize) : base
        (messenger, Direction.Inbound, initialBufferSize) { }
}