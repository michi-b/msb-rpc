using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace MsbRpc.Test.Base;

public class Test
{
    [PublicAPI] protected static readonly ILoggerFactory LoggerFactory;

    static Test()
    {
        Logger logger = new LoggerConfiguration()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .MinimumLevel.Verbose()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{ThreadId}:{ThreadName}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger()!;
        Debug.Assert(logger != null);
        LoggerFactory = new LoggerFactory().AddSerilog(logger);
    }
}