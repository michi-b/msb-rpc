// ReSharper disable CheckNamespace

using System;
using MsbRpc.Serialization.Buffers;

namespace Incrementer.Generated;

public class IncrementerServerEndPoint : MsbRpc.EndPoints.InboundEndPoint<IncrementerProcedure, IIncrementer>
{
    public IncrementerServerEndPoint
    (
        MsbRpc.Messaging.Messenger messenger,
        IIncrementer implementation,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultInitialBufferSize
    ) : base
    (
        messenger,
        implementation,
        CreateLogger<IncrementerServerEndPoint>(loggerFactory),
        initialBufferSize
    ) { }

    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.GetProcedure(procedureId);

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();

    protected override bool GetStopsListening(IncrementerProcedure procedure) => procedure.GetStopsListening();

    protected override ArraySegment<byte> Execute(IncrementerProcedure procedure, BufferReader arguments)
    {
        return procedure switch
        {
            IncrementerProcedure.Increment => this.Increment(arguments),
            IncrementerProcedure.Store => Store(arguments),
            IncrementerProcedure.IncrementStored => IncrementStored(arguments),
            IncrementerProcedure.GetStored => GetStored(arguments),
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    private ArraySegment<byte> GetStored(BufferReader arguments)
    {
        int result = Implementation.GetStored();

        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        BufferWriter resultWriter = this.GetResultWriter(resultSize);

        resultWriter.Write(result);

        return resultWriter.Buffer;
    }

    private ArraySegment<byte> Increment(BufferReader arguments)
    {
        // Read request arguments.
        int valueArgument = arguments.ReadInt();

        // Execute procedure.
        int result = Implementation.Increment(valueArgument);

        // Send response.
        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        BufferWriter resultWriter = this.GetResultWriter(resultSize);

        resultWriter.Write(result);

        return resultWriter.Buffer;
    }

    private ArraySegment<byte> Store(BufferReader arguments)
    {
        int valueArgument = arguments.ReadInt();

        this.Implementation.Store(valueArgument);

        return BufferUtility.Empty;
    }

    private ArraySegment<byte> IncrementStored(BufferReader arguments)
    {
        this.Implementation.IncrementStored();

        return BufferUtility.Empty;
    }
}