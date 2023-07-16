#region

using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

#endregion

namespace MsbRpc.Test.Base;

public class Test
{
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable once MemberCanBeProtected.Global
    // MSTest needs the public setter
    public TestContext TestContext { protected get; set; } = null!;

    [PublicAPI] protected CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

    protected static ILoggerFactory CreateLoggerFactory()
    {
        Serilog.ILogger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger()!;
        return new LoggerFactory().AddSerilog(logger);
    }

    protected ValueTask WaitForConditionAsync
        (Func<bool> condition, CancellationToken cancellationToken)
        => WaitForConditionAsync(condition, NullLogger.Instance, cancellationToken);

    protected ValueTask WaitForConditionAsync
        (Func<bool> condition, ILogger logger, CancellationToken cancellationToken)
        => WaitForConditionAsync(condition, 10, logger, cancellationToken);

    protected ValueTask WaitForConditionAsync
        (Func<bool> condition, int maxRetries, CancellationToken cancellationToken)
        => WaitForConditionAsync(condition, maxRetries, NullLogger.Instance, cancellationToken);

    private async ValueTask WaitForConditionAsync(Func<bool> condition, int maxRetries, ILogger logger, CancellationToken cancellationToken)
    {
        int retryIndex = 0;
        while (retryIndex < maxRetries && !cancellationToken.IsCancellationRequested)
        {
            retryIndex++;
            if (condition())
            {
                break;
            }

            await Task.Delay(1 << retryIndex, cancellationToken);
            retryIndex++;
        }

        logger.Log(LogLevel.Information, "finished after {RetryCount} retries", retryIndex + 1);
    }
}