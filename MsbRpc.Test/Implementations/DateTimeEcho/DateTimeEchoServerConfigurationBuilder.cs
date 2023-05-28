using MsbRpc.Configuration.Builders.Generic;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoServerConfigurationBuilder : ServerConfigurationBuilder<DateTimeEchoServerConfiguration, IDateTimeEcho>
{
    public override DateTimeEchoServerConfiguration Build() => new(this);
}