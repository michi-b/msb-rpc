using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

[PublicAPI]
public readonly struct LogConfiguration : IConfiguration
{
    public readonly EventId Id;
    public readonly LogLevel Level;

    public LogConfiguration(ILogConfigurationBuilder builder)
    {
        Id = builder.Id;
        Level = builder.Level;
    }
}