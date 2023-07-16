#region

using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Interfaces;

#endregion

namespace MsbRpc.Configuration.Builders.Abstract;

[PublicAPI]
public abstract class EndPointConfigurationBuilder<TConfiguration> : ConfigurationWithLoggerFactoryBuilder<TConfiguration>, IEndPointConfigurationBuilder
    where TConfiguration : struct
{
    public const int DefaultInitialBufferSize = 1024;

    [PublicAPI] public int InitialBufferSize { get; set; } = DefaultInitialBufferSize;

    public static implicit operator TConfiguration(EndPointConfigurationBuilder<TConfiguration> builder) => builder.Build();
}