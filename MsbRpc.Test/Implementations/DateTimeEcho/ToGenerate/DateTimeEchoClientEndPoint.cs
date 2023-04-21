namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoClientEndPoint
    : MsbRpc.EndPoints.OutboundEndPoint<DateTimeEchoProcedure>
{
    public DateTimeEchoClientEndPoint
    (
        MsbRpc.Messaging.Messenger messenger,
        MsbRpc.Configuration.OutboundEndPointConfiguration configuration
    ) : base
    (
        messenger,
        configuration
    ) { }
    
    public static async System.Threading.Tasks.ValueTask<DateTimeEchoClientEndPoint> ConnectAsync
    (
        System.Net.IPEndPoint endPoint,
        MsbRpc.Configuration.OutboundEndPointConfiguration configuration
    )
    {
        MsbRpc.Messaging.Messenger messenger = await MsbRpc.Messaging.MessengerFactory.ConnectAsync(endPoint);
        return new DateTimeEchoClientEndPoint(messenger, configuration);
    }
    
    public async System.Threading.Tasks.ValueTask<System.DateTime> GetDateTimeAsync
    (
        System.DateTime myDateTime
    )
    {
        base.AssertIsOperable();
        
        MsbRpc.Serialization.Buffers.Request request = base.Buffer.GetRequest(DateTimeEchoClientEndPoint.GetId(DateTimeEchoProcedure.GetDateTime));
        
        await base.SendRequestAsync(request);
        
        return default!;
    }
    
    protected override DateTimeEchoProcedure GetProcedure(int procedureId) => DateTimeEchoProcedureExtensions.FromId(procedureId);
    
    protected override string GetName(DateTimeEchoProcedure procedure) => DateTimeEchoProcedureExtensions.GetName(procedure);
    
    private static int GetId(DateTimeEchoProcedure procedure) => DateTimeEchoProcedureExtensions.GetId(procedure);
}