using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration.Builders;

public abstract class ConfigurationWithLoggerFactoryBuilder<TConfiguration> : ConfigurationBuilder<TConfiguration>, IConfigurationWithLoggerFactoryBuilder
    where TConfiguration : IConfiguration
{
    [PublicAPI]
    // ReSharper disable once RedundantDefaultMemberInitializer
    // logger factory may be null if no logging is required
    public ILoggerFactory? LoggerFactory { get; set; } = null;
}