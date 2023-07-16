#region

using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Builders.Interfaces.Generic;

#endregion

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface ILogConfigurationBuilder : IConfigurationBuilder<LogConfiguration>
{
    EventId Id { get; set; }
    LogLevel Level { get; set; }
}