#region

using JetBrains.Annotations;

#endregion

namespace MsbRpc.Configuration.Builders.Interfaces.Generic;

[PublicAPI]
public interface IConfigurationBuilder<out TConfiguration> : IConfigurationBuilder where TConfiguration : struct
{
    TConfiguration Build();
}