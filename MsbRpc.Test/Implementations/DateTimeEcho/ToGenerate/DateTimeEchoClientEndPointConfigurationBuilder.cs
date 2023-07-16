#region

using MsbRpc.Configuration.Builders;

#endregion

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoClientEndPointConfigurationBuilder : OutboundEndPointConfigurationBuilder
{
    public DateTimeEchoClientEndPointConfigurationBuilder()
    {
        LoggingName = "DateTimeEchoClientEndPoint";
        InitialBufferSize = 1024;
    }
}