#region

using MsbRpc.Configuration.Builders;

#endregion

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServerEndPointConfigurationBuilder : InboundEndPointConfigurationBuilder
{
    public DateTimeEchoServerEndPointConfigurationBuilder()
    {
        LoggingName = "DateTimeEchoServerEndPoint";
        InitialBufferSize = 1024;
    }
}