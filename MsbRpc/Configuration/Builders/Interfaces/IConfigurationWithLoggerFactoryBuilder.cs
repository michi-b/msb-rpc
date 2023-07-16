#region

using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

#endregion

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IConfigurationWithLoggerFactoryBuilder
{
    public ILoggerFactory? LoggerFactory { get; set; }
}