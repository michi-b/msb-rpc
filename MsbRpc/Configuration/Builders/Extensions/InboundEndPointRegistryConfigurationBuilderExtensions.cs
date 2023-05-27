namespace MsbRpc.Configuration.Builders.Extensions;

public static class InboundEndPointRegistryConfigurationBuilderExtensions
{
    public static TConfigurationBuilder WithLoggingName<TConfigurationBuilder>(this TConfigurationBuilder target, string loggingName)
        where TConfigurationBuilder : InboundEndPointRegistryConfigurationBuilder
    {
        target.LoggingName = loggingName;
        return target;
    }
}