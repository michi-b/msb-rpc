using JetBrains.Annotations;

namespace MsbRpc.Configuration.Builders.Extensions;

[PublicAPI]
public static class ConnectionListenerConfigurationBuilderExtensions
{
    /// <summary>
    ///     sets both the thread name and the logging name
    /// </summary>
    public static ConnectionListenerConfigurationBuilder WithName(this ConnectionListenerConfigurationBuilder configurationBuilder, string name)
        => configurationBuilder.WithLoggingName(name).WithThreadName(name);

    public static ConnectionListenerConfigurationBuilder WithLoggingName(this ConnectionListenerConfigurationBuilder configurationBuilder, string loggingName)
    {
        configurationBuilder.LoggingName = loggingName;
        return configurationBuilder;
    }

    public static ConnectionListenerConfigurationBuilder WithThreadName(this ConnectionListenerConfigurationBuilder configurationBuilder, string threadName)
    {
        configurationBuilder.ThreadName = threadName;
        return configurationBuilder;
    }
}