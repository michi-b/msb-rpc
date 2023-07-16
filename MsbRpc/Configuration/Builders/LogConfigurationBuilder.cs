#region

using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Abstract;
using MsbRpc.Configuration.Builders.Interfaces;

#endregion

namespace MsbRpc.Configuration.Builders;

public class LogConfigurationBuilder : ConfigurationBuilder<LogConfiguration>, ILogConfigurationBuilder
{
    public EventId Id { get; set; }
    public LogLevel Level { get; set; }

    /// <summary>
    ///     Creates ane enabled <see cref="LogConfiguration" />.
    /// </summary>
    public LogConfigurationBuilder(EventId id, LogLevel logLevel = LogLevel.Information)
    {
        Id = id;
        Level = logLevel;
    }

    public override LogConfiguration Build() => new(this);
}