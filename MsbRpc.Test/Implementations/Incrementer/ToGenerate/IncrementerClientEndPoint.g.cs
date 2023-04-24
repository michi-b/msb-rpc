#nullable enable

using System.Net;
using Incrementer.Generated;
using MsbRpc.Configuration;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Test.Implementations.Incrementer.ToGenerate;

public class IncrementerClientEndPoint : OutboundEndPoint<IncrementerProcedure>
{
    private IncrementerClientEndPoint
    (
        Messenger messenger,
        OutboundEndPointConfiguration configuration
    ) : base
    (
        messenger,
        configuration
    ) { }

    public static async ValueTask<IncrementerClientEndPoint> ConnectAsync
    (
        IPEndPoint endPoint,
        OutboundEndPointConfiguration outboundEndPointConfiguration
    )
    {
        Messenger messenger = await MessengerFactory.ConnectAsync(endPoint, outboundEndPointConfiguration.LoggerFactory);
        return new IncrementerClientEndPoint(messenger, outboundEndPointConfiguration);
    }

    public async ValueTask<int> IncrementAsync(int value)
    {
        AssertIsOperable();

        const int valueArgumentSize = PrimitiveSerializer.IntSize;
        // ReSharper disable once InlineTemporaryVariable
        const int constantArgumentSizeSum = valueArgumentSize;

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.Increment), constantArgumentSizeSum);

        BufferWriter writer = request.GetWriter();

        writer.Write(value);

        Response responseMessage = await SendRequestAsync(request);
        BufferReader responseReader = responseMessage.GetReader();

        int result = responseReader.ReadInt();

        return result;
    }

    public async ValueTask<string?> IncrementNullableStringAsync(string? value)
    {
        AssertIsOperable();

        int valueArgumentSize = NullableStringSerializer.GetSize(value);
        // ReSharper disable once InlineTemporaryVariable
        int dynamicArgumentSizeSum = valueArgumentSize + PrimitiveSerializer.IntSize;

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.IncrementString), dynamicArgumentSizeSum);

        BufferWriter requestWriter = request.GetWriter();

        requestWriter.WriteNullable(value);

        Response responseMessage = await SendRequestAsync(request);
        BufferReader responseReader = responseMessage.GetReader();

        string? result = responseReader.ReadStringNullable();

        return result;
    }

    public async ValueTask StoreAsync(int value)
    {
        AssertIsOperable();

        const int valueArgumentSize = PrimitiveSerializer.IntSize;
        // ReSharper disable once InlineTemporaryVariable
        const int constantArgumentSizeSum = valueArgumentSize;

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.Store), constantArgumentSizeSum);

        BufferWriter writer = request.GetWriter();

        writer.Write(value);

        await SendRequestAsync(request);
    }

    public async ValueTask IncrementStoredAsync()
    {
        AssertIsOperable();

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.IncrementStored));

        await SendRequestAsync(request);
    }

    public async ValueTask<int> GetStoredAsync()
    {
        AssertIsOperable();

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.GetStored));

        Response response = await SendRequestAsync(request);
        BufferReader responseReader = response.GetReader();

        int result = responseReader.ReadInt();

        return result;
    }

    public async ValueTask FinishAsync()
    {
        AssertIsOperable();

        Request request = Buffer.GetRequest(GetId(IncrementerProcedure.Finish));
        await SendRequestAsync(request);
    }

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();
    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);
    private static int GetId(IncrementerProcedure procedure) => procedure.GetId();
}