#region

using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;

#endregion

namespace MsbRpc.Configuration.Builders.Abstract;

public abstract class ConfigurationWithLoggerFactoryBuilder<TConfiguration> : ConfigurationBuilder<TConfiguration>, IConfigurationWithLoggerFactoryBuilder
    where TConfiguration : struct
{
    [PublicAPI]
    // ReSharper disable once RedundantDefaultMemberInitializer
    // logger factory may be null if no logging is required
    public ILoggerFactory? LoggerFactory { get; set; } = null;
}