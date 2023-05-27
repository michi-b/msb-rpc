using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

public class ConfigurationWithLoggerFactory : Configuration, IConfigurationWithLoggerFactory
{
    public ILoggerFactory? LoggerFactory { get; }

    public ConfigurationWithLoggerFactory(ILoggerFactory? loggerFactory) => LoggerFactory = loggerFactory;
}