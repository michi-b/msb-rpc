using System;
using JetBrains.Annotations;
using MsbRpc.Configuration.Builders.Interfaces;

namespace MsbRpc.Configuration.Builders.Extensions;

[PublicAPI]
public static class ConfigurationBuilderExtensions
{
    // configuration action for classes
    public static TConfigurationBuilder Configure<TConfigurationBuilder>
    (
        this TConfigurationBuilder target,
        Action<TConfigurationBuilder> configure
    )
        where TConfigurationBuilder : class, IConfigurationBuilder
    {
        configure(target);
        return target;
    }

    // configuration func for structs
    public static TConfigurationBuilder Configure<TConfigurationBuilder>
    (
        this TConfigurationBuilder target,
        Func<TConfigurationBuilder, TConfigurationBuilder> configure
    )
        where TConfigurationBuilder : struct, IConfigurationBuilder
        => configure(target);
}