using MsbRpc.Configuration.Generic;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoServerConfiguration : ServerConfiguration<IDateTimeEcho>
{
    public DateTimeEchoServerConfiguration(DateTimeEchoServerConfigurationBuilder builder) : base(builder) { }
}