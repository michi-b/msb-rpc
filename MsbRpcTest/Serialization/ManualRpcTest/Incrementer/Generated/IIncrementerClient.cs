using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;

public class IncrementerClient : RpcClient<IncrementerProcedure>
{
    public IncrementerClient(Messenger messenger, int initialBufferSize = DefaultBufferSize) : base(messenger, initialBufferSize) { }

    public async Task<int> IncrementAsync(int value, CancellationToken cancellationToken)
    {
        EnterSending();

        ArraySegment<byte> requestBytes = GetRequestMemory(PrimitiveSerializer.Int32Size);

        var writer = new BufferWriter(requestBytes);
        requestBytes.WriteInt32(value);

        ArraySegment<byte> responseBytes = await SendRequest(IncrementerProcedure.Increment, requestBytes, cancellationToken);

        var reader = new BufferReader(responseBytes);
        int response = reader.ReadInt32();

        ExitSending();

        return response;
    }

    protected override ArraySegment<byte> HandleRequest(IncrementerProcedure procedure, ArraySegment<byte> arguments)
    {
        return procedure switch
        {
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    protected override Direction GetDirectionAfterRequest(IncrementerProcedure procedure)
    {
        return procedure switch
        {
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }
}