using JetBrains.Annotations;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration.Builders.Abstract;

[PublicAPI]
public abstract class EndPointConfigurationBuilder<TConfiguration> : ConfigurationWithLoggerFactoryBuilder<TConfiguration> where TConfiguration : IConfiguration
{
    public const int DefaultInitialBufferSize = 1024;

    [PublicAPI] public int InitialBufferSize { get; set; } = DefaultInitialBufferSize;
}