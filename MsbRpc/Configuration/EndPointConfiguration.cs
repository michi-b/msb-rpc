using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

[PublicAPI]
public class EndPointConfiguration : ConfigurationWithLoggerFactory, IEndPointConfiguration
{
    public int InitialBufferSize { get; }

    [PublicAPI]
    public EndPointConfiguration(IEndPointConfigurationBuilder builder) : base(builder) => InitialBufferSize = builder.InitialBufferSize;
}