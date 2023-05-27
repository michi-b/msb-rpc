using JetBrains.Annotations;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public class EndPointConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder, IConfigurationBuilder<EndPointConfiguration>
{
    public const int DefaultInitialBufferSize = 1024;

    [PublicAPI] public int InitialBufferSize { get; set; } = DefaultInitialBufferSize;

    public new EndPointConfiguration Build() => new(InitialBufferSize, LoggerFactory);
}