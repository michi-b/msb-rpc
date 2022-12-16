namespace MsbRpcTest.ManualRpcTest.Incrementer.Generated;

public class IncrementerServerEndPoint : MsbRpc.EndPoints.RpcEndPoint<IncrementerServerProcedure, MsbRpc.EndPoints.UndefinedProcedure>
{
    private readonly IIncrementerServer _incrementer;

    public IncrementerServerEndPoint
    (
        MsbRpc.Messaging.Messenger messenger,
        IIncrementerServer incrementer,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int bufferSize = DefaultBufferSize
    )
        : base
        (
            messenger,
            MsbRpc.EndPoints.Direction.Inbound,
            loggerFactory != null ? Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger<IncrementerServerEndPoint>(loggerFactory) : null,
            bufferSize
        )
        => _incrementer = incrementer;

    protected override string GetName(IncrementerServerProcedure procedure) => procedure.GetName();
    protected override string GetName(MsbRpc.EndPoints.UndefinedProcedure procedure) => throw new InvalidOperationException();

    protected override ArraySegment<byte> HandleRequest(IncrementerServerProcedure serverProcedure, MsbRpc.Serialization.Buffers.BufferReader arguments)
    {
        return serverProcedure switch
        {
            IncrementerServerProcedure.Increment => Increment(arguments),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override MsbRpc.EndPoints.Direction GetDirectionAfterHandling(IncrementerServerProcedure serverProcedure)
    {
        return serverProcedure switch
        {
            IncrementerServerProcedure.Increment => MsbRpc.EndPoints.Direction.Inbound,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private ArraySegment<byte> Increment(MsbRpc.Serialization.Buffers.BufferReader argumentsReader)
    {
        
        // Read request arguments.

        int valueArgument = argumentsReader.ReadInt32();

        // Execute procedure.
        int result = _incrementer.Increment(valueArgument);

        // Send response.

        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.Int32Size;
        
        MsbRpc.Serialization.Buffers.BufferWriter responseWriter = GetResponseWriter(resultSize);
        
        responseWriter.Write(result);
        
        return responseWriter.Buffer;
    }
}