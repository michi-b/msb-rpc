#region

using Microsoft.Extensions.Logging;
using MsbRpc.Configuration;

#endregion

namespace MsbRpc.Extensions;

public static class LoggerExtensions
{
    public static bool GetIsEnabled(this ILogger logger, LogConfiguration configuration) => logger.IsEnabled(configuration.Level);
}