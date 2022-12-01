using Microsoft.Extensions.Logging;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;

public class IncrementerServerEndPoint : RpcEndPoint<IncrementerServerProcedure, IncrementerClientProcedure>
{
    private readonly IIncrementerServer _incrementer;

    public IncrementerServerEndPoint
    (
        Messenger messenger,
        IIncrementerServer incrementer,
        ILoggerFactory? loggerFactory = null,
        int bufferSize = DefaultBufferSize
    )
        : base
        (
            messenger,
            Direction.Inbound,
            loggerFactory?.CreateLogger<IncrementerServerEndPoint>(),
            bufferSize
        )
        => _incrementer = incrementer;

    protected override string GetName(IncrementerServerProcedure procedure) => procedure.GetName();

    protected override ArraySegment<byte> HandleRequest(IncrementerServerProcedure serverProcedure, ArraySegment<byte> arguments)
    {
        return serverProcedure switch
        {
            IncrementerServerProcedure.Increment => Increment(arguments),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override Direction GetDirectionAfterHandling(IncrementerServerProcedure serverProcedure)
    {
        return serverProcedure switch
        {
            IncrementerServerProcedure.Increment => Direction.Inbound,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override Direction GetDirectionAfterCalling(IncrementerClientProcedure procedure)
        => throw new NoProceduresDefinedException(this, Direction.Outbound);

    private ArraySegment<byte> Increment(ArraySegment<byte> argumentsBuffer)
    {
        //read
        var reader = new BufferReader(argumentsBuffer);
        int value = reader.ReadInt32();

        //execute        
        int result = _incrementer.Increment(value);

        //return
        ArraySegment<byte> resultBuffer = GetResponseMemory(PrimitiveSerializer.Int32Size);
        var writer = new BufferWriter(resultBuffer);
        writer.Write(result);
        return resultBuffer;
    }
}