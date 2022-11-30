using MsbRpc.EndPoints;
using MsbRpc.EndPoints.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;

public class IncrementerClient : RpcEndPoint<IncrementerClientProcedure, IncrementerServerProcedure>
{
    public IncrementerClient(Messenger messenger, int initialBufferSize = DefaultBufferSize) : base
        (messenger, Direction.Outbound, initialBufferSize) { }

    public async Task<int> IncrementAsync(int value, CancellationToken cancellationToken)
    {
        EnterSending();

        ArraySegment<byte> requestBytes = GetRequestMemory(PrimitiveSerializer.Int32Size);

        var writer = new BufferWriter(requestBytes);
        writer.Write(value);

        ArraySegment<byte> responseBytes = await SendRequest(IncrementerServerProcedure.Increment, requestBytes, cancellationToken);

        var reader = new BufferReader(responseBytes);
        int response = reader.ReadInt32();

        ExitSending();

        return response;
    }

    protected override ArraySegment<byte> HandleRequest(IncrementerClientProcedure procedure, ArraySegment<byte> arguments)
        => throw new ThereAreNoRequestsDefinedException();

    protected override Direction GetDirectionAfterRequest(IncrementerClientProcedure procedure) => throw new ThereAreNoRequestsDefinedException();
}