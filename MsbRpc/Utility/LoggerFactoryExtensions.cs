using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MsbRpc.Utility;

public static class LoggerFactoryExtensions
{
    public static ILogger<TOwner> CreateLoggerOptional<TOwner>(this ILoggerFactory? loggerFactory)
        => loggerFactory != null
            ? loggerFactory.CreateLogger<TOwner>()
            : new NullLogger<TOwner>();
}