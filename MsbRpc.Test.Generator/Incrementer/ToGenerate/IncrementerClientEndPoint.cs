using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Incrementer.Generated;
using MsbRpc.Configuration;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Test.Generator.Incrementer.ToGenerate;

public class IncrementerClientEndPoint : OutboundEndPoint<IncrementerClientEndPoint, IncrementerProcedure>
{
    private IncrementerClientEndPoint
    (
        Messenger messenger,
        Configuration configuration
    ) : base
    (
        messenger,
        configuration
    ) { }

    public class Configuration : OutboundEndPointConfiguration { }

    public static async ValueTask<IncrementerClientEndPoint> ConnectAsync
    (
        IPEndPoint endPoint,
        Action<Configuration>? configure
    )
    {
        var configuration = new Configuration();
        configure?.Invoke(configuration);
        Messenger messenger = await MessengerFactory.ConnectAsync(endPoint, configuration.LoggerFactory);
        return new IncrementerClientEndPoint(messenger, configuration);
    }

    public async ValueTask<int> IncrementAsync(int value, CancellationToken cancellationToken)
    {
        AssertIsOperable();

        const int valueArgumentSize = PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.Increment), constantArgumentSizeSum);

        BufferWriter writer = request.GetWriter();

        writer.Write(value);

        Response responseMessage = await SendRequestAsync(request);
        BufferReader responseReader = responseMessage.GetReader();

        int result = responseReader.ReadInt();

        return result;
    }

    public async ValueTask StoreAsync(int value, CancellationToken cancellationToken)
    {
        AssertIsOperable();

        const int valueArgumentSize = PrimitiveSerializer.IntSize;
        const int constantArgumentSizeSum = valueArgumentSize;

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.Store), constantArgumentSizeSum);

        BufferWriter writer = request.GetWriter();

        writer.Write(value);

        await SendRequestAsync(request);
    }

    public async ValueTask IncrementStoredAsync(CancellationToken cancellationToken)
    {
        AssertIsOperable();

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.IncrementStored));

        await SendRequestAsync(request);
    }

    public async ValueTask<int> GetStoredAsync(CancellationToken cancellationToken)
    {
        AssertIsOperable();

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.GetStored));

        Response response = await SendRequestAsync(request);
        BufferReader responseReader = response.GetReader();

        int result = responseReader.ReadInt();

        return result;
    }

    public async ValueTask FinishAsync(CancellationToken cancellationToken)
    {
        AssertIsOperable();

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.Finish));
        await SendRequestAsync(request);
    }

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();
    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);
    protected override int GetId(IncrementerProcedure procedure) => procedure.GetId();
}