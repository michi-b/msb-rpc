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

    protected override Message Execute(IncrementerProcedure procedure, Request request)
    {
        return procedure switch
        {
            IncrementerProcedure.Increment => this.Increment(request),
            IncrementerProcedure.Store => Store(request),
            IncrementerProcedure.IncrementStored => IncrementStored(),
            IncrementerProcedure.GetStored => GetStored(),
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    private Message GetStored()
    {
        int result = Implementation.GetStored();

        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        Message message = Buffer.GetMessage(resultSize);
        BufferWriter resultWriter = message.GetWriter();

        resultWriter.Write(result);

        return message;
    }

    private Message Increment(Request request)
    {
        BufferReader requestReader = request.GetReader();
        
        // Read request arguments.
        int valueArgument = requestReader.ReadInt();

        // Execute procedure.
        int result = Implementation.Increment(valueArgument);

        // Send response.
        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        Message message = Buffer.GetMessage(resultSize);
        BufferWriter resultWriter = message.GetWriter();

        resultWriter.Write(result);

        return message;
    }

    private Message Store(Request request)
    {
        BufferReader requestReader = request.GetReader();
        
        int valueArgument = requestReader.ReadInt();

        this.Implementation.Store(valueArgument);

        return Message.Empty;
    }

    private Message IncrementStored()
    {
        this.Implementation.IncrementStored();

        return Message.Empty;
    }

    protected override int GetId(IncrementerProcedure procedure) => procedure.GetId();
    
    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();

    protected override bool GetIsFinal(IncrementerProcedure procedure) => procedure.GetIsFinal();
}