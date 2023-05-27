using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

[PublicAPI]
public readonly struct LogConfiguration : IConfiguration
{
    public readonly EventId Id;
    public readonly LogLevel Level;

    /// <summary>
    ///     Creates a disabled <see cref="LogConfiguration" />.
    /// </summary>
    public LogConfiguration(EventId id)
    {
        Id = id;
        Level = LogLevel.Information;
    }

    /// <summary>
    ///     Creates ane enabled <see cref="LogConfiguration" />.
    /// </summary>
    public LogConfiguration(EventId id, LogLevel logLevel)
    {
        Id = id;
        Level = logLevel;
    }

    public LogConfiguration(EventId id, bool enabled, LogLevel level)
    {
        Id = id;
        Level = level;
    }
}