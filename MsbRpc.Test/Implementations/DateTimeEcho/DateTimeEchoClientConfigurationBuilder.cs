using MsbRpc.Configuration.Builders;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoClientConfigurationBuilder : OutboundEndPointConfigurationBuilder
{
    public DateTimeEchoClientConfigurationBuilder() => InitialBufferSize = 1024;
}