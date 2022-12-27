// ReSharper disable InlineTemporaryVariable

// ReSharper disable RedundantNameQualifier

namespace MsbRpcTest.ManualRpcTest.Incrementer.Generated;

public class IncrementerClientEndPoint : MsbRpc.EndPoints.RpcEndPoint<MsbRpc.EndPoints.UndefinedProcedure, IncrementerServerProcedure>
{
    public IncrementerClientEndPoint
    (
        MsbRpc.Messaging.Messenger messenger,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultBufferSize
    ) : base
        (
            messenger,
            MsbRpc.EndPoints.EndPointDirection.Outbound,
            loggerFactory != null
                ? Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger<IncrementerClientEndPoint>(loggerFactory)
                : new Microsoft.Extensions.Logging.Abstractions.NullLogger<IncrementerClientEndPoint>(),
            initialBufferSize
        ) { }

    public async System.Threading.Tasks.ValueTask<int> IncrementAsync(int value, CancellationToken cancellationToken)
    {
        EnterCalling();

        // Write request arguments

        const int valueArgumentSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        const int constantArgumentSizeSum = valueArgumentSize;

        MsbRpc.Serialization.Buffers.BufferWriter writer = GetRequestWriter(constantArgumentSizeSum);

        writer.Write(value);

        // Send request.

        const IncrementerServerProcedure procedure = IncrementerServerProcedure.Increment;

        MsbRpc.Serialization.Buffers.BufferReader responseReader = await SendRequestAsync(procedure, writer.Buffer, cancellationToken);

        // Read response.        

        int response = responseReader.ReadInt();

        ExitCalling(procedure);

        return response;
    }

    protected override string GetName(IncrementerServerProcedure procedure) => procedure.GetName();

    protected override bool GetInvertsDirection(IncrementerServerProcedure procedure) => procedure.GetInvertsDirection();
}