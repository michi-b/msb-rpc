namespace MsbRpc.Configuration.Interfaces;

public interface IConfigurationBuilder<out TConfiguration> where TConfiguration : IConfigurationWithLoggerFactory
{
    TConfiguration Build();
}