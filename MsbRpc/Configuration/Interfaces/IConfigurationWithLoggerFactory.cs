using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace MsbRpc.Configuration.Interfaces;

[PublicAPI]
public interface IConfigurationWithLoggerFactory : IConfiguration
{
    ILoggerFactory? LoggerFactory { get; }
}