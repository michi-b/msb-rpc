using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration.Builders;

public class ConfigurationWithLoggerFactoryBuilder : IConfigurationBuilder<IConfigurationWithLoggerFactory>
{
    [PublicAPI]
    // ReSharper disable once RedundantDefaultMemberInitializer
    // logger factory may be null if no logging is required
    public ILoggerFactory? LoggerFactory { get; set; } = null;

    public IConfigurationWithLoggerFactory Build() => new ConfigurationWithLoggerFactory(LoggerFactory);
}