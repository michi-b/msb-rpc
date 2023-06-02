using MsbRpc.Configuration.Builders;

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServerConfigurationBuilder : ServerConfigurationBuilder
{
    public DateTimeEchoServerConfigurationBuilder()
    {
        LoggingName = "DateTimeEchoServer";
        ThreadName = "DateTimeEchoServer";
        EndPointConfiguration.LoggingName = "DateTimeEchoServerEndPoint";
        EndPointRegistryConfiguration.LoggingName = "DateTimeEchoServerEndPointRegistry";

        EndPointConfiguration.InitialBufferSize = 1024;
    }
}