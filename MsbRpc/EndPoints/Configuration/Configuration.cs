using Microsoft.Extensions.Logging;

namespace MsbRpc.EndPoints.Configuration;

public class Configuration
{
    public ILoggerFactory? LoggerFactory { get; set; } = null;
}