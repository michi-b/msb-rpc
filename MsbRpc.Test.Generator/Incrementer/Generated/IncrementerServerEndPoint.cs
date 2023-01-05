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

    protected override MsbRpc.Serialization.Buffers.Response Execute(IncrementerProcedure procedure, MsbRpc.Serialization.Buffers.Request request)
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

    private MsbRpc.Serialization.Buffers.Response GetStored()
    {
        int result = this.Implementation.GetStored();

        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        MsbRpc.Serialization.Buffers.Response response = Buffer.GetResponse(this.Implementation.RandToCompletion, resultSize);
        MsbRpc.Serialization.Buffers.BufferWriter responseWriter = response.GetWriter();

        responseWriter.Write(result);

        return response;
    }

    private MsbRpc.Serialization.Buffers.Response Increment(MsbRpc.Serialization.Buffers.Request request)
    {
        MsbRpc.Serialization.Buffers.BufferReader requestReader = request.GetReader();
        
        // Read request arguments.
        int valueArgument = requestReader.ReadInt();

        // Execute procedure.
        int result = this.Implementation.Increment(valueArgument);

        // Send response.
        const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        MsbRpc.Serialization.Buffers.Response response = Buffer.GetResponse(this.Implementation.RandToCompletion, resultSize);
        MsbRpc.Serialization.Buffers.BufferWriter responseWriter = response.GetWriter();

        responseWriter.Write(result);

        return response;
    }

    private MsbRpc.Serialization.Buffers.Response Store(MsbRpc.Serialization.Buffers.Request request)
    {
        MsbRpc.Serialization.Buffers.BufferReader requestReader = request.GetReader();
        
        int valueArgument = requestReader.ReadInt();

        this.Implementation.Store(valueArgument);

        return this.Buffer.GetResponse(this.Implementation.RandToCompletion);
    }

    private MsbRpc.Serialization.Buffers.Response IncrementStored()
    {
        this.Implementation.IncrementStored();
        return this.Buffer.GetResponse(this.Implementation.RandToCompletion);
    }

    private MsbRpc.Serialization.Buffers.Response End()
    {
        this.Implementation.Finish();
        return this.Buffer.GetResponse(this.Implementation.RandToCompletion);
    }

    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();
}