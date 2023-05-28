using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IConfigurationWithLoggerFactoryBuilder
{
    public ILoggerFactory? LoggerFactory { get; set; }
}