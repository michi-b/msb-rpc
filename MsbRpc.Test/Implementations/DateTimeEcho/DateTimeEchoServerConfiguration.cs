using MsbRpc.Configuration;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoServerConfiguration : ServerConfiguration
{
    public DateTimeEchoServerConfiguration(IServerConfigurationBuilder builder) : base(builder) { }
}