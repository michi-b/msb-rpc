// ReSharper disable CheckNamespace

namespace Incrementer.Generated;

public class IncrementerServerEndPoint : MsbRpc.EndPoints.InboundEndPoint<IncrementerServerEndPoint, IncrementerProcedure, IIncrementer>
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
        MsbRpc.Utility.LoggerFactoryExtensions.CreateLoggerOptional<IncrementerServerEndPoint>(loggerFactory),
        initialBufferSize
    ) { }

    protected override MsbRpc.Serialization.Buffers.Message Execute(IncrementerProcedure procedure, MsbRpc.Serialization.Buffers.Request request)
    {
        return procedure switch
        {
            IncrementerProcedure.Increment => this.Increment(request),
            IncrementerProcedure.Store => Store(request),
            IncrementerProcedure.IncrementStored => IncrementStored(),
            IncrementerProcedure.GetStored => GetStored(),
            IncrementerProcedure.End => End(),
            _ => throw new System.ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    private MsbRpc.Serialization.Buffers.Message GetStored()
    {
        int result = Implementation.GetStored();

        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        MsbRpc.Serialization.Buffers.Message message = Buffer.GetMessage(resultSize);
        MsbRpc.Serialization.Buffers.BufferWriter resultWriter = message.GetWriter();

        resultWriter.Write(result);

        return message;
    }

    private MsbRpc.Serialization.Buffers.Message Increment(MsbRpc.Serialization.Buffers.Request request)
    {
        MsbRpc.Serialization.Buffers.BufferReader requestReader = request.GetReader();
        
        // Read request arguments.
        int valueArgument = requestReader.ReadInt();

        // Execute procedure.
        int result = Implementation.Increment(valueArgument);

        // Send response.
        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        MsbRpc.Serialization.Buffers.Message message = Buffer.GetMessage(resultSize);
        MsbRpc.Serialization.Buffers.BufferWriter resultWriter = message.GetWriter();

        resultWriter.Write(result);

        return message;
    }

    private MsbRpc.Serialization.Buffers.Message Store(MsbRpc.Serialization.Buffers.Request request)
    {
        MsbRpc.Serialization.Buffers.BufferReader requestReader = request.GetReader();
        
        int valueArgument = requestReader.ReadInt();

        this.Implementation.Store(valueArgument);

        return MsbRpc.Serialization.Buffers.Message.Empty;
    }

    private MsbRpc.Serialization.Buffers.Message IncrementStored()
    {
        this.Implementation.IncrementStored();

        return MsbRpc.Serialization.Buffers.Message.Empty;
    }

    private MsbRpc.Serialization.Buffers.Message End()
    {
        this.Implementation.End();

        return MsbRpc.Serialization.Buffers.Message.Empty;
    }

    protected override int GetId(IncrementerProcedure procedure) => procedure.GetId();
    
    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();

    protected override bool GetClosesCommunication(IncrementerProcedure procedure) => procedure.GetClosesConnection();
}