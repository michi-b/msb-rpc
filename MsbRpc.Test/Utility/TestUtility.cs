#region

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

#endregion

namespace MsbRpc.Test.Utility;

public static class TestUtility
{
    public static readonly ILoggerFactory LoggerFactory;

    public static readonly ILogger Logger;

    static TestUtility()
    {
        Logger logger = new LoggerConfiguration()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .MinimumLevel.Verbose()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{ThreadId}:{ThreadName}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger()!;
        Debug.Assert(logger != null);
        LoggerFactory = new LoggerFactory().AddSerilog(logger);
        Logger = LoggerFactory.CreateLogger(nameof(TestUtility));
    }
}