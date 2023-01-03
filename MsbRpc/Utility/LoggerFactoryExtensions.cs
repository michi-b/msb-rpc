using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MsbRpc.Utility;

public static class LoggerFactoryExtensions
{
    public static ILogger<TOwner> TryCreateLogger<TOwner>(this ILoggerFactory? loggerFactory)
        => loggerFactory != null
            ? loggerFactory.CreateLogger<TOwner>()
            : new NullLogger<TOwner>();
}