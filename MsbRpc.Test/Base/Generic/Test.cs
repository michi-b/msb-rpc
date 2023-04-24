using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpc.Test.Base.Generic;


public class Test<TTest> : Test where TTest : Test<TTest>
{
    [PublicAPI] protected static readonly ILogger<TTest> Logger;

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // MSTest needs the public setter
    public TestContext TestContext { private get; set; } = null!;

    [PublicAPI] protected CancellationToken CancellationToken => TestContext.CancellationTokenSource.Token;

    static Test() => Logger = LoggerFactory.CreateLogger<TTest>();
}