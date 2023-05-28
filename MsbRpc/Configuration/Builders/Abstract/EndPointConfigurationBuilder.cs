using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration.Builders.Abstract;

[PublicAPI]
public abstract class EndPointConfigurationBuilder<TConfiguration> : ConfigurationWithLoggerFactoryBuilder<TConfiguration>, IEndPointConfigurationBuilder where TConfiguration : IConfiguration
{
    public const int DefaultInitialBufferSize = 1024;
    
    public static implicit operator TConfiguration(EndPointConfigurationBuilder<TConfiguration> builder) => builder.Build();

    [PublicAPI] public int InitialBufferSize { get; set; } = DefaultInitialBufferSize;
}