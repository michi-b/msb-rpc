using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints;

[PublicAPI]
public struct LogConfiguration
{
    public EventId Id;
    public bool Enabled;
    public LogLevel Level;

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
    /// <param name="logLevel"></param>
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

    public bool IsEnabled(ILogger? logger) => Enabled && logger.IsEnabled(Level);
}