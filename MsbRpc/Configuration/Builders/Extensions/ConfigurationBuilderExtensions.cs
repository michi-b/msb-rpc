using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration.Builders.Extensions;

[PublicAPI]
public static class ConfigurationBuilderWithLoggerFactoryExtensions
{
    public static TConfigurationBuilder WithLoggerFactory<TConfigurationBuilder>(this TConfigurationBuilder target, ILoggerFactory? loggerFactory)
        where TConfigurationBuilder : IConfigurationWithLoggerFactoryBuilder
    {
        target.LoggerFactory = loggerFactory;
        return target;
    }
}