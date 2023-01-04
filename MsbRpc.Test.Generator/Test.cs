using System.Diagnostics;
using System.Net;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace MsbRpc.Test.Generator;

public class Test
{
    protected static readonly ILoggerFactory LoggerFactory;
    protected static readonly IPAddress LocalHost = Dns.GetHostEntry("localhost").AddressList[0];
    

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // MSTest needs the public setter
    public TestContext TestContext { private get; set; } = null!;

    [PublicAPI] protected CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

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