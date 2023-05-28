using JetBrains.Annotations;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IConfigurationBuilder<out TConfiguration> where TConfiguration : IConfiguration
{
    TConfiguration Build();
}