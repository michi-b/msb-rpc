using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

[PublicAPI]
public readonly struct LogConfiguration : IConfiguration
{
    public readonly EventId Id;
    public readonly bool Enabled;
    public readonly LogLevel Level;

    /// <summary>
    ///     Creates a disabled <see cref="LogConfiguration" />.
    /// </summary>
    public LogConfiguration(EventId id)
    {
        Id = id;
        Enabled = false;
        Level = LogLevel.Information;
    }

    /// <summary>
    ///     Creates ane enabled <see cref="LogConfiguration" />.
    /// </summary>
    public LogConfiguration(EventId id, LogLevel logLevel)
    {
        Id = id;
        Enabled = true;
        Level = logLevel;
    }

    public LogConfiguration(EventId id, bool enabled, LogLevel level)
    {
        Id = id;
        Enabled = enabled;
        Level = level;
    }

    public bool IsEnabled(ILogger logger) => Enabled && logger.IsEnabled(Level);
}