using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace MsbRpc.Configuration.Builders.Extensions;

[PublicAPI]
public static class ConfigurationBuilderExtensions
{
    public static TConfigurationBuilder WithLoggerFactory<TConfigurationBuilder>(this TConfigurationBuilder target, ILoggerFactory? loggerFactory)
        where TConfigurationBuilder : ConfigurationWithLoggerFactoryBuilder
    {
        target.LoggerFactory = loggerFactory;
        return target;
    }
}