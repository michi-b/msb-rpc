using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints;

[PublicAPI]
public struct LogConfiguration
{
    public bool Enabled;
    public LogLevel Level;

    public LogConfiguration(int eventId, bool enabled, LogLevel level)
    {
        Enabled = enabled;
        Level = level;
    }

    public bool IsEnabled(ILogger? logger) => Enabled && logger.IsEnabled(Level);
}