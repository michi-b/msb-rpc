// ReSharper disable RedundantBaseQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable InlineTemporaryVariable

using System.Net;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Incrementer.Generated;


public class IncrementerClientEndPoint : MsbRpc.EndPoints.OutboundEndPoint<IncrementerClientEndPoint, IncrementerProcedure>
{
    [UsedImplicitly]
    private readonly ILogger<IncrementerClientEndPoint> _logger;

    public static async System.Threading.Tasks.ValueTask<IncrementerClientEndPoint> ConnectAsync
    (
        IPAddress address,
        int port,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultInitialBufferSize
    )
        => await IncrementerClientEndPoint.ConnectAsync(new IPEndPoint(address, port), loggerFactory, initialBufferSize);

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
    {
        _logger = logger;
    }

    public async System.Threading.Tasks.ValueTask<int> IncrementAsync(int value, System.Threading.CancellationToken cancellationToken)
    {
        const int valueArgumentSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        MsbRpc.Serialization.Buffers.Request request = Buffer.GetRequest(constantArgumentSizeSum, GetId(IncrementerProcedure.Increment));
        
        MsbRpc.Serialization.Buffers.BufferWriter writer = request.GetWriter();

        writer.Write(value);

        MsbRpc.Serialization.Buffers.Message responseMessage = await base.SendRequestAsync(request, cancellationToken);
        MsbRpc.Serialization.Buffers.BufferReader responseReader = responseMessage.GetReader();

        int response = responseReader.ReadInt();

        return response;
    }

    public async System.Threading.Tasks.ValueTask StoreAsync(int value, System.Threading.CancellationToken cancellationToken)
    {
        const int valueArgumentSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        MsbRpc.Serialization.Buffers.Request request = Buffer.GetRequest(constantArgumentSizeSum, GetId(IncrementerProcedure.Store));
        
        MsbRpc.Serialization.Buffers.BufferWriter writer = request.GetWriter();

        writer.Write(value);

        await base.SendRequestAsync(request, cancellationToken);
    }

    public async System.Threading.Tasks.ValueTask IncrementStored(System.Threading.CancellationToken cancellationToken)
    {
        MsbRpc.Serialization.Buffers.Request request = Buffer.GetRequest(0, GetId(IncrementerProcedure.IncrementStored));

        await base.SendRequestAsync(request, cancellationToken);
    }
    
    public async System.Threading.Tasks.ValueTask<int> GetStored(System.Threading.CancellationToken cancellationToken)
    {
        MsbRpc.Serialization.Buffers.Request request = Buffer.GetRequest(GetId(IncrementerProcedure.GetStored));

        MsbRpc.Serialization.Buffers.Message responseMessage = await base.SendRequestAsync(request, cancellationToken);
        MsbRpc.Serialization.Buffers.BufferReader responseReader = responseMessage.GetReader();

        int response = responseReader.ReadInt();

        return response;
    }
    
    public async System.Threading.Tasks.ValueTask EndAsync(System.Threading.CancellationToken cancellationToken)
    {
        MsbRpc.Serialization.Buffers.Request request = Buffer.GetRequest(GetId(IncrementerProcedure.End));
        await base.SendRequestAsync(request, cancellationToken);
        Dispose();
    }

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();
    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);
    protected override int GetId(IncrementerProcedure procedure) => procedure.GetId();
    protected override bool GetClosesCommunication(IncrementerProcedure procedure) => procedure.GetClosesConnection();
}