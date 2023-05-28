using MsbRpc.Contracts;

namespace MsbRpc.Configuration.Interfaces.Generic;

public interface IServerConfiguration<out TContract> : IServerConfiguration
    where TContract : IRpcContract
{
    public IFactory<TContract> ImplementationFactory { get; }
}