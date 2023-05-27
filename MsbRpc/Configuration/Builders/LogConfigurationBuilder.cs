using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace MsbRpc.Configuration.Builders;

[PublicAPI]
public class LogConfigurationBuilder : ConfigurationBuilder<LogConfiguration>
{
    public EventId Id { get; set; }
    public bool Enabled { get; set; }
    public LogLevel Level { get; set; }

    /// <summary>
    ///     Creates a disabled <see cref="LogConfiguration" />.
    /// </summary>
    public LogConfigurationBuilder(EventId id)
    {
        Id = id;
        Enabled = false;
        Level = LogLevel.Information;
    }

    /// <summary>
    ///     Creates ane enabled <see cref="LogConfiguration" />.
    /// </summary>
    public LogConfigurationBuilder(EventId id, LogLevel logLevel)
    {
        Id = id;
        Enabled = true;
        Level = logLevel;
    }

    public LogConfigurationBuilder(EventId id, bool enabled, LogLevel level)
    {
        Id = id;
        Enabled = enabled;
        Level = level;
    }

    public override LogConfiguration Build() => new(Id, Enabled, Level);
}