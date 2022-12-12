// ReSharper disable InlineTemporaryVariable

using Microsoft.Extensions.Logging;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.ManualRpcTest.Incrementer.Generated;

public class IncrementerClientEndPoint : RpcEndPoint<UndefinedProcedure, IncrementerServerProcedure>
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

        // Write request arguments

        const int valueArgumentSize = PrimitiveSerializer.Int32Size;

        const int constantArgumentSizeSum = valueArgumentSize;

        ArraySegment<byte> requestBytes = GetRequestMemory(constantArgumentSizeSum);

        var writer = new BufferWriter(requestBytes);

        writer.Write(value);

        // Send request.

        const IncrementerServerProcedure procedure = IncrementerServerProcedure.Increment;
        ArraySegment<byte> responseBytes = await SendRequest(procedure, requestBytes, cancellationToken);

        // Read response.        

        var reader = new BufferReader(responseBytes);

        int response = reader.ReadInt32();

        ExitCalling(procedure);

        return response;
    }

    protected override string GetName(IncrementerServerProcedure procedure) => procedure.GetName();

    protected override Direction GetDirectionAfterCalling(IncrementerServerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerServerProcedure.Increment => Direction.Outbound,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}