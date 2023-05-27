using Microsoft.Extensions.Logging;
using MsbRpc.Configuration.Interfaces;

namespace MsbRpc.Configuration;

public class EndPointConfiguration : ConfigurationWithLoggerFactory, IEndPointConfiguration
{
    public int InitialBufferSize { get; }

    public EndPointConfiguration(int initialBufferSize, ILoggerFactory? loggerFactory) : base(loggerFactory) => InitialBufferSize = initialBufferSize;
}