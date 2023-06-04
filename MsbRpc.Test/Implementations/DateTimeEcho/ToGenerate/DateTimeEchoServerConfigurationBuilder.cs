using MsbRpc.Configuration.Builders;

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServerConfigurationBuilder : ServerConfigurationBuilder
{
    public DateTimeEchoServerConfigurationBuilder()
    {
        LoggingName = "DateTimeEchoServer";
        ThreadName = "DateTimeEchoServer";
        EndPointRegistryConfiguration.LoggingName = "DateTimeEchoServerEndPointRegistry";
        EndPointConfiguration = new DateTimeEchoServerEndPointConfigurationBuilder();
    }
}