using MsbRpc;
using MsbRpc.Serialization;
using MsbRpc.Serialization.ByteArraySegment;
using MsbRpc.Serialization.Primitives;
using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.ManualImplementation.Input;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.ManualImplementation.Generated;

public class IncrementerRpcReceiver : ISequentialRpcReceiver
{
    private readonly IIncrementer _incrementer;
    public IncrementerRpcReceiver(IIncrementer incrementer) => _incrementer = incrementer;

    public ArraySegment<byte> Receive(int procedureId, ArraySegment<byte> arguments, RecycledBuffer recycledBuffer)
    {
        return (IncrementerProcedure)procedureId switch
        {
            IncrementerProcedure.Increment => Increment(arguments, recycledBuffer),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private ArraySegment<byte> Increment(ArraySegment<byte> arguments, RecycledBuffer recycledBuffer)
    {
        //read
        var reader = new SequentialReader(arguments);
        int value = reader.ReadInt32();

        //execute        
        int result = _incrementer.Increment(value);

        //return
        ArraySegment<byte> resultSegment = recycledBuffer.Get(PrimitiveSerializer.Int32Size);
        var writer = new SequentialWriter(resultSegment);
        writer.Write(result);
        return resultSegment;
    }
}