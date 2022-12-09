using Microsoft.Extensions.Logging;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.ManualRpcTest.Incrementer.Generated;

public class IncrementerClientEndPoint : RpcEndPoint<IncrementerClientProcedure, IncrementerServerProcedure>
{
    public IncrementerClientEndPoint
    (
        Messenger messenger,
        ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultBufferSize
    )
        : base
        (
            messenger,
            Direction.Outbound,
            loggerFactory?.CreateLogger<IncrementerClientEndPoint>(),
            initialBufferSize
        ) { }

    public async Task<int> IncrementAsync(int value, CancellationToken cancellationToken)
    {
        EnterCalling();

        const IncrementerServerProcedure procedure = IncrementerServerProcedure.Increment;

        ArraySegment<byte> requestBytes = GetRequestMemory(PrimitiveSerializer.Int32Size);

        var writer = new BufferWriter(requestBytes);
        writer.Write(value);

        ArraySegment<byte> responseBytes = await SendRequest(procedure, requestBytes, cancellationToken);

        var reader = new BufferReader(responseBytes);
        int response = reader.ReadInt32();

        ExitCalling(procedure);

        return response;
    }

    protected override string GetName(IncrementerClientProcedure procedure) => procedure.GetName();
    protected override string GetName(IncrementerServerProcedure procedure) => procedure.GetName();

    protected override ArraySegment<byte> HandleRequest(IncrementerClientProcedure procedure, ArraySegment<byte> arguments)
        => throw new NoProceduresDefinedException(this, Direction.Inbound);

    protected override Direction GetDirectionAfterHandling(IncrementerClientProcedure procedure) 
        => throw new NoProceduresDefinedException(this, Direction.Inbound);

    protected override Direction GetDirectionAfterCalling(IncrementerServerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerServerProcedure.Increment => Direction.Outbound,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}