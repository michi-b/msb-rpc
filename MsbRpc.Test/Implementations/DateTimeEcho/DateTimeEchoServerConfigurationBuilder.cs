using MsbRpc.Configuration.Builders.Generic;
using MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

namespace MsbRpc.Test.Implementations.DateTimeEcho;

public class DateTimeEchoServerConfigurationBuilder : ServerConfigurationBuilder<DateTimeEchoServerConfiguration, IDateTimeEcho>
{
    public DateTimeEchoServerConfigurationBuilder(IFactory<IDateTimeEcho> implementationFactory) : base(implementationFactory) { }

    public DateTimeEchoServerConfigurationBuilder(DateTimeEchoImplementationByDelegateFactory.FactoryDelegate createDateTimeEcho)
        : this((DateTimeEchoImplementationByDelegateFactory)createDateTimeEcho) { }

    public override DateTimeEchoServerConfiguration Build() => new(this);
}