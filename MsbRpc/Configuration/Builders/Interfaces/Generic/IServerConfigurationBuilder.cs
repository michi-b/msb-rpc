using JetBrains.Annotations;
using MsbRpc.Contracts;

namespace MsbRpc.Configuration.Builders.Interfaces.Generic;

[PublicAPI]
public interface IServerConfigurationBuilder<TContract> : IServerConfigurationBuilder
    where TContract : IRpcContract
{
    public IFactory<TContract> ImplementationFactory { get; set; }
}