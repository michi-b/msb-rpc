using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Input;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;

public class IncrementerServer : RpcEndPoint<IncrementerServerProcedure, IncrementerClientProcedure>
{
    private readonly IIncrementer _incrementer;

    public IncrementerServer(Messenger messenger, IIncrementer incrementer, int bufferSize = DefaultBufferSize)
        : base(messenger, Direction.Inbound, bufferSize)
        => _incrementer = incrementer;

    protected override ArraySegment<byte> HandleRequest(IncrementerServerProcedure serverProcedure, ArraySegment<byte> arguments)
    {
        return serverProcedure switch
        {
            IncrementerServerProcedure.Increment => Increment(arguments),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    protected override Direction GetDirectionAfterRequest(IncrementerServerProcedure serverProcedure)
    {
        return serverProcedure switch
        {
            IncrementerServerProcedure.Increment => Direction.Inbound,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

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