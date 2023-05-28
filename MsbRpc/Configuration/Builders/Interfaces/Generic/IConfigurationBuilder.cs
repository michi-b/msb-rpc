using JetBrains.Annotations;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration.Builders.Interfaces.Generic;

[PublicAPI]
public interface IConfigurationBuilder<out TConfiguration> : IConfigurationBuilder where TConfiguration : IConfiguration
{
    TConfiguration Build();
}