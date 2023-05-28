using System;
using MsbRpc.Configuration.Builders.Interfaces.Generic;
using MsbRpc.Configuration.Interfaces.Generic;
using MsbRpc.Contracts;

namespace MsbRpc.Configuration.Generic;

public class ServerConfiguration<TContract> : ServerConfiguration, IServerConfiguration<TContract>
    where TContract : IRpcContract
{
    public IFactory<TContract> ImplementationFactory { get; }

    protected ServerConfiguration(IServerConfigurationBuilder<TContract> builder) : base(builder)
    {
        IFactory<TContract>? implementationFactory = builder.ImplementationFactory;
        ImplementationFactory = implementationFactory ?? throw new ArgumentNullException(nameof(builder.ImplementationFactory), "implementation factory may not be null");
    }
}