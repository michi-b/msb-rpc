using MsbRpc.Contracts;

namespace MsbRpc.Configuration.Builders.Interfaces.Generic;

public interface IServerConfigurationBuilder<TContract> : IServerConfigurationBuilder
    where TContract : IRpcContract
{
    public IFactory<TContract> ImplementationFactory { get; set; }
}