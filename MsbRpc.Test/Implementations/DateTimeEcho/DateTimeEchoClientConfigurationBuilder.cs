using MsbRpc.Configuration.Builders;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoClientConfigurationBuilder : OutboundEndPointConfigurationBuilder
{
    public DateTimeEchoClientConfigurationBuilder()
    {
        LoggingName = "DateTimeEchoClientEndPoint";
        InitialBufferSize = 1024;
    }
}