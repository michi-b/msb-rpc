// ReSharper disable RedundantBaseQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable InlineTemporaryVariable

// ReSharper disable once CheckNamespace
// ReSharper disable MemberCanBePrivate.Global

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using LoggerFactoryExtensions = MsbRpc.Utility.LoggerFactoryExtensions;

namespace Incrementer.Generated;

public class IncrementerClientEndPoint : OutboundEndPoint<IncrementerClientEndPoint, IncrementerProcedure>
{
    private IncrementerClientEndPoint
    (
        Messenger messenger,
        ILogger<IncrementerClientEndPoint> logger,
        int initialBufferSize
    ) : base
    (
        messenger,
        logger,
        initialBufferSize
    ) { }

    public static async ValueTask<IncrementerClientEndPoint> ConnectAsync
    (
        IPAddress ipAddress,
        int port,
        ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultInitialBufferSize
    )
    {
        ILogger<IncrementerClientEndPoint> logger = LoggerFactoryExtensions.CreateLoggerOptional<IncrementerClientEndPoint>(loggerFactory);
        MsbRpc.Messaging.Messenger messenger = await MessengerFactory.ConnectAsync ( new IPEndPoint(ipAddress, port), logger);
        return new(messenger, logger, initialBufferSize);
    }

    public async ValueTask<int> IncrementAsync(int value, CancellationToken cancellationToken)
    {
        base.AssertIsOperable();

        const int valueArgumentSize = PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.Increment), constantArgumentSizeSum);

        BufferWriter writer = request.GetWriter();

        writer.Write(value);

        Response responseMessage = await base.SendRequestAsync(request, cancellationToken);
        BufferReader responseReader = responseMessage.GetReader();

        int result = responseReader.ReadInt();

        return result;
    }

    public async ValueTask StoreAsync(int value, CancellationToken cancellationToken)
    {
        base.AssertIsOperable();

        const int valueArgumentSize = PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.Store), constantArgumentSizeSum);

        BufferWriter writer = request.GetWriter();

        writer.Write(value);

        await base.SendRequestAsync(request, cancellationToken);
    }

    public async ValueTask IncrementStoredAsync(CancellationToken cancellationToken)
    {
        base.AssertIsOperable();

        Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.IncrementStored));

        await base.SendRequestAsync(request, cancellationToken);
    }

    public async ValueTask<int> GetStoredAsync(CancellationToken cancellationToken)
    {
        base.AssertIsOperable();

        Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.GetStored));

        Response response = await base.SendRequestAsync(request, cancellationToken);
        BufferReader responseReader = response.GetReader();

        int result = responseReader.ReadInt();

        return result;
    }

    public async ValueTask FinishAsync(CancellationToken cancellationToken)
    {
        base.AssertIsOperable();

        Request request = base.Buffer.GetRequest(GetId(IncrementerProcedure.Finish));
        await base.SendRequestAsync(request, cancellationToken);
    }

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();
    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);
    protected override int GetId(IncrementerProcedure procedure) => procedure.GetId();
}