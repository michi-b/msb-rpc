namespace MsbRpcTest.ManualRpcTest.Incrementer.Generated;

public class IncrementerServerEndPoint : MsbRpc.EndPoints.RpcEndPoint<IncrementerServerProcedure, MsbRpc.EndPoints.UndefinedProcedure>
{
    private readonly IIncrementerServer _incrementerServer;

    public IncrementerServerEndPoint
    (
        MsbRpc.Messaging.Messenger messenger,
        IIncrementerServer incrementerServer,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultBufferSize
    )
        : base
        (
            messenger,
            MsbRpc.EndPoints.EndPointDirection.Inbound,
            loggerFactory != null
                ? Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger<IncrementerServerEndPoint>(loggerFactory)
                : new Microsoft.Extensions.Logging.Abstractions.NullLogger<IncrementerServerEndPoint>(),
            initialBufferSize
        ) => _incrementerServer = incrementerServer;

    protected override MsbRpc.Serialization.Buffers.BufferWriter HandleRequest
    (
        IncrementerServerProcedure serverProcedure,
        MsbRpc.Serialization.Buffers.BufferReader arguments
    )
    {
        return serverProcedure switch
        {
            IncrementerServerProcedure.Increment => Increment(arguments),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private MsbRpc.Serialization.Buffers.BufferWriter Increment(MsbRpc.Serialization.Buffers.BufferReader argumentsReader)
    {
        // Read request arguments.
        int valueArgument = argumentsReader.ReadInt();

        // Execute procedure.
        int result = _incrementerServer.Increment(valueArgument);

        // Send response.
        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.Int32Size;

        MsbRpc.Serialization.Buffers.BufferWriter responseWriter = GetResponseWriter(resultSize);

        responseWriter.Write(result);

        return responseWriter;
    }

    protected override string GetName(IncrementerServerProcedure procedure) => procedure.GetName();

    protected override bool GetInvertsDirection(IncrementerServerProcedure procedure) => procedure.GetInvertsDirection();
}