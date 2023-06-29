using System.Net;
using MsbRpc.Configuration;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Servers.Listener;

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoClientEndPoint
    : OutboundEndPoint<DateTimeEchoProcedure>
{
    private DateTimeEchoClientEndPoint
    (
        Messenger messenger,
        in OutboundEndPointConfiguration configuration
    ) : base
    (
        messenger,
        configuration
    ) { }

    public static async ValueTask<DateTimeEchoClientEndPoint> ConnectAsync
    (
        IPEndPoint endPoint,
        OutboundEndPointConfiguration configuration
    )
    {
        Messenger messenger = await Messenger.ConnectAsync(endPoint);
        
        var result = new DateTimeEchoClientEndPoint(messenger, configuration);
        
        //send initial connection message so the server knows what kind of connection this is
        await messenger.SendInitialConnectionMessage(InitialConnectionMessage.CreateUnIdentified(), result.Buffer);

        return result;
    }

    public async ValueTask<DateTime> GetDateTimeAsync(DateTime clientDateTime)
    {
        AssertIsOperable();

        int myDateTimeArgumentSize = DateTimeSerializer.Size;

        int argumentSizeSum = myDateTimeArgumentSize;

        Request request = Buffer.GetRequest(GetId(DateTimeEchoProcedure.GetDateTime), argumentSizeSum);

        BufferWriter writer = request.GetWriter();

        DateTimeSerializer.Write(writer, clientDateTime);

        Response response = await SendRequestAsync(request);

        BufferReader responseReader = response.GetReader();

        DateTime result = DateTimeSerializer.Read(responseReader);

        return result;
    }

    protected override DateTimeEchoProcedure GetProcedure(int procedureId) => DateTimeEchoProcedureExtensions.FromId(procedureId);

    protected override string GetName(DateTimeEchoProcedure procedure) => procedure.GetName();

    private static int GetId(DateTimeEchoProcedure procedure) => procedure.GetId();
}