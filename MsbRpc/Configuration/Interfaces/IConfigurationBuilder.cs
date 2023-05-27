namespace MsbRpc.Configuration.Interfaces;

public interface IConfigurationBuilder<out TConfiguration> where TConfiguration : IConfiguration
{
    TConfiguration Build();
}