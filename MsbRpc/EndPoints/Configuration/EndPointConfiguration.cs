using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints;

public class EndPointConfiguration
{
    public int InitialBufferSize = DefaultInitialSize;
    
    public const int DefaultInitialSize = 1024;
    public ILoggerFactory? LoggerFactory { get; set; } = null;
}