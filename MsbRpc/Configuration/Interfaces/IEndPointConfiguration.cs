namespace MsbRpc.Configuration.Interfaces;

public interface IEndPointConfiguration : IConfigurationWithLoggerFactory
{
    int InitialBufferSize { get; }
}