using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration.Builders.Extensions;

[PublicAPI]
public static class ServerConfigurationBuilderExtensions
{
    /// <summary>
    ///     sets both the thread name and the logging name
    /// </summary>
    public static TConfigurationBuilder WithName<TConfigurationBuilder>(this TConfigurationBuilder configurationBuilder, string name)
        where TConfigurationBuilder : IServerConfigurationBuilder
        => configurationBuilder.WithLoggingName(name).WithThreadName(name);

    public static TConfigurationBuilder WithLoggingName<TConfigurationBuilder>(this TConfigurationBuilder configurationBuilder, string loggingName)
        where TConfigurationBuilder : IServerConfigurationBuilder
    {
        configurationBuilder.LoggingName = loggingName;
        return configurationBuilder;
    }

    public static TConfigurationBuilder WithThreadName<TConfigurationBuilder>(this TConfigurationBuilder configurationBuilder, string threadName)
        where TConfigurationBuilder : IServerConfigurationBuilder
    {
        configurationBuilder.ThreadName = threadName;
        return configurationBuilder;
    }
}