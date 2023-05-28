using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

public class ConfigurationWithLoggerFactory : Configuration, IConfigurationWithLoggerFactory
{
    public ILoggerFactory? LoggerFactory { get; }

    [PublicAPI]
    public ConfigurationWithLoggerFactory(IConfigurationWithLoggerFactoryBuilder builder) => LoggerFactory = builder.LoggerFactory;
}