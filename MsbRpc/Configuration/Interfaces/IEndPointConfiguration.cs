using JetBrains.Annotations;

namespace MsbRpc.Configuration.Interfaces;

[PublicAPI]
public interface IEndPointConfiguration : IConfigurationWithLoggerFactory
{
    int InitialBufferSize { get; }
}