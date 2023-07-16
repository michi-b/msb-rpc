#region

using JetBrains.Annotations;

#endregion

namespace MsbRpc.Configuration.Builders.Extensions;

[PublicAPI]
public static class InboundEndPointConfigurationBuilderExtensions
{
    public static TConfigurationBuilder WithLoggingName<TConfigurationBuilder>(this TConfigurationBuilder target, string loggingName)
        where TConfigurationBuilder : InboundEndPointConfigurationBuilder
    {
        target.LoggingName = loggingName;
        return target;
    }
}