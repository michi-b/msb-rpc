#region

using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;

#endregion

namespace MsbRpc.Configuration;

[PublicAPI]
public readonly struct LogConfiguration
{
    public readonly EventId Id;
    public readonly LogLevel Level;

    public LogConfiguration(ILogConfigurationBuilder builder)
    {
        Id = builder.Id;
        Level = builder.Level;
    }
}