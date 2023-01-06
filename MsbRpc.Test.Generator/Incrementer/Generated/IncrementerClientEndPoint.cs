// ReSharper disable RedundantBaseQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable InlineTemporaryVariable

// ReSharper disable once CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
namespace Incrementer.Generated;


public class IncrementerClientEndPoint : MsbRpc.EndPoints.OutboundEndPoint<IncrementerClientEndPoint, IncrementerProcedure>
{
    [JetBrains.Annotations.UsedImplicitly]
    private readonly Microsoft.Extensions.Logging.ILogger<IncrementerClientEndPoint> _logger;

    public static async System.Threading.Tasks.ValueTask<IncrementerClientEndPoint> ConnectAsync
    (
        System.Net.IPAddress address,
        int port,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultInitialBufferSize
    )
        => await ConnectAsync(new System.Net.IPEndPoint(address, port), loggerFactory, initialBufferSize);

    public static async System.Threading.Tasks.ValueTask<IncrementerClientEndPoint> ConnectAsync
    (
        System.Net.IPEndPoint serverEndPoint,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultInitialBufferSize
    )
    {
        Microsoft.Extensions.Logging.ILogger<IncrementerClientEndPoint> logger 
            = MsbRpc.Utility.LoggerFactoryExtensions.CreateLoggerOptional<IncrementerClientEndPoint>(loggerFactory);
        MsbRpc.Messaging.Messenger messenger = await MsbRpc.Messaging.MessengerFactory.ConnectAsync(serverEndPoint, logger);
        return new IncrementerClientEndPoint(messenger, logger, initialBufferSize);
    }

    private IncrementerClientEndPoint
    (
        MsbRpc.Messaging.Messenger messenger,
        Microsoft.Extensions.Logging.ILogger<IncrementerClientEndPoint> logger,
        int initialBufferSize
    ) : base
    (
        messenger,
        logger,
        initialBufferSize
    )
        => _logger = logger;

    public async System.Threading.Tasks.ValueTask<int> IncrementAsync(int value, System.Threading.CancellationToken cancellationToken)
    {
        base.AssertIsOperable();
        
        const int valueArgumentSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        MsbRpc.Serialization.Buffers.Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.Increment), constantArgumentSizeSum);
        
        MsbRpc.Serialization.Buffers.BufferWriter writer = request.GetWriter();

        writer.Write(value);

        MsbRpc.Serialization.Buffers.Response responseMessage = await base.SendRequestAsync(request, cancellationToken);
        MsbRpc.Serialization.Buffers.BufferReader responseReader = responseMessage.GetReader();

        int result = responseReader.ReadInt();

        return result;
    }

    public async System.Threading.Tasks.ValueTask StoreAsync(int value, System.Threading.CancellationToken cancellationToken)
    {
        base.AssertIsOperable();

        const int valueArgumentSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        MsbRpc.Serialization.Buffers.Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.Store), constantArgumentSizeSum);
        
        MsbRpc.Serialization.Buffers.BufferWriter writer = request.GetWriter();

        writer.Write(value);

        await base.SendRequestAsync(request, cancellationToken);
    }

    public async System.Threading.Tasks.ValueTask IncrementStoredAsync(System.Threading.CancellationToken cancellationToken)
    {
        base.AssertIsOperable();
        
        MsbRpc.Serialization.Buffers.Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.IncrementStored), 0);

        await base.SendRequestAsync(request, cancellationToken);
    }
    
    public async System.Threading.Tasks.ValueTask<int> GetStoredAsync(System.Threading.CancellationToken cancellationToken)
    {
        base.AssertIsOperable();
        
        MsbRpc.Serialization.Buffers.Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.GetStored));

        MsbRpc.Serialization.Buffers.Response response = await base.SendRequestAsync(request, cancellationToken);
        MsbRpc.Serialization.Buffers.BufferReader responseReader = response.GetReader();

        int result = responseReader.ReadInt();

        return result;
    }
    
    public async System.Threading.Tasks.ValueTask FinishAsync(System.Threading.CancellationToken cancellationToken)
    {
        base.AssertIsOperable();

        MsbRpc.Serialization.Buffers.Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.End));
        await base.SendRequestAsync(request, cancellationToken);
    }

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();
    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);
    protected override int GetId(IncrementerProcedure procedure) => procedure.GetId();
}