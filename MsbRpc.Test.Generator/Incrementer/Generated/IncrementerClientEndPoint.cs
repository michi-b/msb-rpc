using System.Threading;
using MsbRpc.Serialization.Buffers;
// ReSharper disable RedundantBaseQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable once CheckNamespace
namespace Incrementer.Generated;


public class IncrementerClientEndPoint : MsbRpc.EndPoints.OutboundEndPoint<IncrementerProcedure>
{
    public IncrementerClientEndPoint
    (
        MsbRpc.Messaging.Messenger messenger,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultInitialBufferSize
    ) : base
        (
            messenger,
            MsbRpc.Utility.LoggerFactoryExtensions.TryCreateLogger<IncrementerClientEndPoint>(loggerFactory),
            initialBufferSize
        ) { }

    public async System.Threading.Tasks.ValueTask<int> IncrementAsync(int value, CancellationToken cancellationToken)
    {
        const int valueArgumentSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        Request request = Buffer.GetRequest(constantArgumentSizeSum, GetId(IncrementerProcedure.Increment));
        
        MsbRpc.Serialization.Buffers.BufferWriter writer = request.GetWriter();

        writer.Write(value);

        Message responseMessage = await base.SendRequestAsync(request, cancellationToken);
        BufferReader responseReader = responseMessage.GetReader();

        int response = responseReader.ReadInt();

        return response;
    }
    
    public async System.Threading.Tasks.ValueTask StoreAsync(int value, CancellationToken cancellationToken)
    {
        const int valueArgumentSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        Request request = Buffer.GetRequest(constantArgumentSizeSum, GetId(IncrementerProcedure.Store));
        
        MsbRpc.Serialization.Buffers.BufferWriter writer = request.GetWriter();

        writer.Write(value);

        await base.SendRequestAsync(request, cancellationToken);
    }

    public async System.Threading.Tasks.ValueTask IncrementStored(CancellationToken cancellationToken)
    {
        Request request = Buffer.GetRequest(0, GetId(IncrementerProcedure.IncrementStored));

        await base.SendRequestAsync(request, cancellationToken);
    }
    
    public async System.Threading.Tasks.ValueTask<int> GetStored(CancellationToken cancellationToken)
    {
        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.GetStored));

        Message responseMessage = await base.SendRequestAsync(request, cancellationToken);
        BufferReader responseReader = responseMessage.GetReader();

        int response = responseReader.ReadInt();

        return response;
    }
    
    public async System.Threading.Tasks.ValueTask EndAsync(CancellationToken cancellationToken)
    {
        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.End));
        await base.SendRequestAsync(request, cancellationToken);
        Dispose();
    }

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();
    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);
    protected override int GetId(IncrementerProcedure procedure) => procedure.GetId();
    protected override bool GetClosesCommunication(IncrementerProcedure procedure) => procedure.GetClosesConnection();
}