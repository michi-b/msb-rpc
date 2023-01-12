using Microsoft.Extensions.Logging;
using MsbRpc.EndPoints;

namespace MsbRpc.Extensions;

public static class LoggerExtensions
{
    public static bool GetIsEnabled(this ILogger logger, LogConfiguration configuration) => configuration.IsEnabled(logger)
}