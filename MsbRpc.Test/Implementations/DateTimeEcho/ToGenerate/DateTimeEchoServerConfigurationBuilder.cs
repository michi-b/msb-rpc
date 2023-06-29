using MsbRpc.Configuration.Builders;

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServerConfigurationBuilder : ServerConfigurationBuilder
{
    public DateTimeEchoServerConfigurationBuilder()
    {
        ConnectionListenerConfiguration.LoggingName = "DateTimeEchoServerConnectionListener";
        ConnectionListenerConfiguration.ThreadName = "DateTimeEchoServerConnectionListener";
        EndPointRegistryConfiguration.LoggingName = "DateTimeEchoServerEndPointRegistry";
        EndPointConfiguration = new DateTimeEchoServerEndPointConfigurationBuilder();
    }
}