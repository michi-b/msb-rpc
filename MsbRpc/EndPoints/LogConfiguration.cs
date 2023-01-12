using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints;

[PublicAPI]
public struct LogConfiguration
{
    public bool Enabled;
    public LogLevel Level;

    /// <summary>
    /// Creates a disabled <see cref="LogConfiguration"/>.
    /// </summary>
    public LogConfiguration()
    {
        Enabled = false;
        Level = LogLevel.Information;
    }

    /// <summary>
    /// Creates ane enabled <see cref="LogConfiguration" />.
    /// </summary>
    /// <param name="logLevel"></param>
    public LogConfiguration(LogLevel logLevel)
    {
        Enabled = true;
        Level = logLevel;
    }
    
    public LogConfiguration(bool enabled, LogLevel level)
    {
        Enabled = enabled;
        Level = level;
    }

    public bool IsEnabled(ILogger? logger) => Enabled && logger.IsEnabled(Level);
}