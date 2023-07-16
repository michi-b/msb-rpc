using MsbRpc.Configuration.Builders;

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServerConfigurationBuilder : ServerConfigurationBuilder
{
    public DateTimeEchoServerConfigurationBuilder()
    {
        MessengerListenerConfiguration.LoggingName = "DateTimeEchoServerConnectionListener";
        MessengerListenerConfiguration.ThreadName = "DateTimeEchoServerConnectionListener";
        EndPointRegistryConfiguration.LoggingName = "DateTimeEchoServerEndPointRegistry";
        EndPointConfiguration = new DateTimeEchoServerEndPointConfigurationBuilder();
    }
}
