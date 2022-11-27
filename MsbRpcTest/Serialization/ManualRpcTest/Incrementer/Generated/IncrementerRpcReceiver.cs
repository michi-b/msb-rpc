using MsbRpc;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Input;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;

public class IncrementerRpcReceiver : ISequentialRpcReceiver
{
    private readonly IIncrementer _incrementer;
    public IncrementerRpcReceiver(IIncrementer incrementer) => _incrementer = incrementer;

    public ArraySegment<byte> Receive(int procedureId, ArraySegment<byte> arguments, RecycledBuffer argumentsBuffer)
    {
        return (IncrementerProcedure)procedureId switch
        {
            IncrementerProcedure.Increment => Increment(arguments, argumentsBuffer),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private ArraySegment<byte> Increment(ArraySegment<byte> arguments, RecycledBuffer argumentsBuffer)
    {
        //read
        var reader = new BufferReader(arguments);
        int value = reader.ReadInt32();

        //execute        
        int result = _incrementer.Increment(value);

        //return
        ArraySegment<byte> resultSegment = argumentsBuffer.Get(PrimitiveSerializer.Int32Size);
        var writer = new BufferWriter(resultSegment);
        writer.Write(result);
        return resultSegment;
    }
}