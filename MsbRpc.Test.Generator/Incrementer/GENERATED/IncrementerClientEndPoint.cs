// ReSharper disable once CheckNamespace

using System.Threading;
using MsbRpc.Serialization.Buffers;

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
            CreateLogger<IncrementerClientEndPoint>(loggerFactory),
            initialBufferSize
        ) { }

    public async System.Threading.Tasks.ValueTask<int> IncrementAsync(int value, CancellationToken cancellationToken)
    {
        // Write request arguments

        const int valueArgumentSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

        const int constantArgumentSizeSum = valueArgumentSize;

        Request request = Buffer.GetRequest(constantArgumentSizeSum, GetId(IncrementerProcedure.Increment));
        
        MsbRpc.Serialization.Buffers.BufferWriter writer = request.GetWriter();

        writer.Write(value);

        // Send request.

        const IncrementerProcedure procedure = IncrementerProcedure.Increment;

        Message responseMessage = base.SendRequest(request);
        BufferReader responseReader = responseMessage.GetReader();

        // Read response.        

        int response = responseReader.ReadInt();

        return response;
    }

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();
    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);
    protected override int GetId(IncrementerProcedure procedure) => procedure.GetId();
    protected override bool GetIsFinal(IncrementerProcedure procedure) => procedure.GetIsFinal();
}