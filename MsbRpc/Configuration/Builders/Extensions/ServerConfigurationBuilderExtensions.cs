using JetBrains.Annotations;

namespace MsbRpc.Configuration.Builders.Extensions;

[PublicAPI]
public static class ServerConfigurationBuilderExtensions
{
    /// <summary>
    ///     sets both the thread name and the logging name
    /// </summary>
    public static TConfigurationBuilder WithName<TConfigurationBuilder>(this TConfigurationBuilder configurationBuilder, string name)
        where TConfigurationBuilder : ServerConfigurationBuilder
        => configurationBuilder.WithLoggingName(name).WithThreadName(name);

    public static TConfigurationBuilder WithLoggingName<TConfigurationBuilder>(this TConfigurationBuilder configurationBuilder, string loggingName)
        where TConfigurationBuilder : ServerConfigurationBuilder
    {
        configurationBuilder.LoggingName = loggingName;
        return configurationBuilder;
    }

    public static TConfigurationBuilder WithThreadName<TConfigurationBuilder>(this TConfigurationBuilder configurationBuilder, string threadName)
        where TConfigurationBuilder : ServerConfigurationBuilder
    {
        configurationBuilder.ThreadName = threadName;
        return configurationBuilder;
    }
}