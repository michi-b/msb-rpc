﻿using Microsoft.Extensions.Logging;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.ManualRpcTest.Incrementer.Generated;

public class IncrementerServerEndPoint : RpcEndPoint<IncrementerServerProcedure, UndefinedProcedure>
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
    protected override string GetName(UndefinedProcedure procedure) => throw new InvalidOperationException();

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

    private ArraySegment<byte> Increment(ArraySegment<byte> argumentsBuffer)
    {
        
        // Read request arguments.

        var reader = new BufferReader(argumentsBuffer);
        
        int valueArgument = reader.ReadInt32();

        // Execute procedure.
        int result = _incrementer.Increment(valueArgument);

        // Send response.

        const int resultSize = PrimitiveSerializer.Int32Size;
        
        ArraySegment<byte> resultBuffer = GetResponseMemory(resultSize);
        
        var writer = new BufferWriter(resultBuffer);
        
        writer.Write(result);
        
        return resultBuffer;
    }
}