using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration.Builders.Abstract;

public abstract class ConfigurationBuilder<TConfiguration> : IConfigurationBuilder<TConfiguration> where TConfiguration : IConfiguration
{
    public static implicit operator TConfiguration(ConfigurationBuilder<TConfiguration> builder) => builder.Build();
    public abstract TConfiguration Build();
}