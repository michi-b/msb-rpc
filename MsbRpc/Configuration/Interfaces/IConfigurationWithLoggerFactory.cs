using Microsoft.Extensions.Logging;

namespace MsbRpc.Configuration.Interfaces;

public interface IConfigurationWithLoggerFactory : IConfiguration
{
    ILoggerFactory? LoggerFactory { get; }
}