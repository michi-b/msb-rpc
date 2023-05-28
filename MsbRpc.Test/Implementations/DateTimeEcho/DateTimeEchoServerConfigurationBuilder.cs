using MsbRpc.Configuration.Builders;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoServerConfigurationBuilder : ServerConfigurationBuilder<DateTimeEchoServerConfiguration>
{
    public DateTimeEchoServerConfigurationBuilder()
    {
        LoggingName = "DateTimeEchoServer";
        ThreadName = "DateTimeEchoServer";
    }

    public override DateTimeEchoServerConfiguration Build() => new(this);
}