#region

using MsbRpc.Configuration.Builders.Interfaces.Generic;

#endregion

namespace MsbRpc.Configuration.Builders.Abstract;

public abstract class ConfigurationBuilder<TConfiguration> : IConfigurationBuilder<TConfiguration> where TConfiguration : struct
{
    public static implicit operator TConfiguration(ConfigurationBuilder<TConfiguration> builder) => builder.Build();
    public abstract TConfiguration Build();
}