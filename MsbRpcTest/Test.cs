using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;

namespace MsbRpcTest;

public class Test
{
    [PublicAPI] protected static readonly ILoggerFactory? LoggerFactory;

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // MSTest needs the public setter
    public TestContext TestContext { private get; set; } = null!;

    [PublicAPI] protected CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

    static Test()
    {
        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger()!;
        Debug.Assert(logger != null);
        LoggerFactory = new LoggerFactory().AddSerilog(logger);
    }
}