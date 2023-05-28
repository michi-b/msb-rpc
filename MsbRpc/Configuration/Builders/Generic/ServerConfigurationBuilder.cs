using MsbRpc.Configuration.Builders.Interfaces.Generic;
using MsbRpc.Configuration.Interfaces.Generic;
using MsbRpc.Contracts;

namespace MsbRpc.Configuration.Builders.Generic;

public abstract class ServerConfigurationBuilder<TConfiguration, TContract>
    : ServerConfigurationBuilder<TConfiguration>, IServerConfigurationBuilder<TContract>
    where TConfiguration : IServerConfiguration<TContract>
    where TContract : IRpcContract
{
    public IFactory<TContract>? ImplementationFactory { get; set; } = null;
}