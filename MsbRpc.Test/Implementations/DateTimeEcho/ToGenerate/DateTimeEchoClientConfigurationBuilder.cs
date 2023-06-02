using MsbRpc.Configuration.Builders;

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoClientConfigurationBuilder : OutboundEndPointConfigurationBuilder
{
    public DateTimeEchoClientConfigurationBuilder()
    {
        LoggingName = "DateTimeEchoClientEndPoint";
        InitialBufferSize = 1024;
    }
}