using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface ILogConfigurationBuilder : IConfigurationBuilder<LogConfiguration>
{
    EventId Id { get; set; }
    LogLevel Level { get; set; }
}