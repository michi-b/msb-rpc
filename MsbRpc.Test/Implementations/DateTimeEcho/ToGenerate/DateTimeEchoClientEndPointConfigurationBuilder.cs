using MsbRpc.Configuration.Builders;

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoClientEndPointConfigurationBuilder : OutboundEndPointConfigurationBuilder
{
    public DateTimeEchoClientEndPointConfigurationBuilder()
    {
        LoggingName = "DateTimeEchoClientEndPoint";
        InitialBufferSize = 1024;
    }
}