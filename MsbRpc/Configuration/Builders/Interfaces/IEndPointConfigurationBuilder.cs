using JetBrains.Annotations;

namespace MsbRpc.Configuration.Builders.Interfaces;

[PublicAPI]
public interface IEndPointConfigurationBuilder : IConfigurationWithLoggerFactoryBuilder
{
    int InitialBufferSize { get; set; }
}