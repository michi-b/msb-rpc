using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Incrementer.Generated;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.EndPoints;
using Serilog;
using Serilog.Core;

namespace MsbRpc.Test.Generator.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
    private static readonly IPAddress LocalHost = Dns.GetHostEntry("localhost").AddressList[0];

    private static readonly ILogger<IncrementerTest> Logger;

    private static readonly ILoggerFactory LoggerFactory;

    static IncrementerTest()
    {
        Logger logger = new LoggerConfiguration()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{ThreadId}:{ThreadName}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger()!;
        LoggerFactory = new LoggerFactory().AddSerilog(logger);
        Logger = LoggerFactory.CreateLogger<IncrementerTest>();
    }

    [TestMethod]
    public void Listens()
    {
        IncrementerServer server = IncrementerServer.Start(() => new Incrementer(), LoggerFactory);
        WaitForThreads();
        server.Dispose();
    }

    [TestMethod]
    public async Task ConnectsAndDisconnects()
    {
        using IncrementerServer server = IncrementerServer.Start(() => new Incrementer(), LoggerFactory);
        IncrementerClientEndPoint client = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);

        WaitForThreads();
        Assert.AreEqual(1, server.CreateConnectionDump().Length);

        client.Dispose();

        WaitForThreads();
        Assert.AreEqual(0, server.CreateConnectionDump().Length);
    }

    [TestMethod]
    public async Task ConnectsAndDisconnectsTwoClients()
    {
        using IncrementerServer server = IncrementerServer.Start(() => new Incrementer(), LoggerFactory);

        IncrementerClientEndPoint client1 = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);
        WaitForThreads();
        Assert.AreEqual(1, server.CreateConnectionDump().Length);

        IncrementerClientEndPoint client2 = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);
        WaitForThreads();
        Assert.AreEqual(2, server.CreateConnectionDump().Length);

        client1.Dispose();
        WaitForThreads();
        Assert.AreEqual(1, server.CreateConnectionDump().Length);

        client2.Dispose();
        WaitForThreads();
        Assert.AreEqual(0, server.CreateConnectionDump().Length);
    }

    [TestMethod]
    public async Task CanDisposeServerPrematurely()
    {
        IncrementerClientEndPoint client;
        using (IncrementerServer server = IncrementerServer.Start(() => new Incrementer(), LoggerFactory))
        {
            client = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);
        }

        client.Dispose();
    }

    [TestMethod]
    public async Task Increments0To1()
    {
        const int testValue = 0;
        const int expectedResult = 1;

        using IncrementerServer server = IncrementerServer.Start(Incrementer.Create, LoggerFactory);
        IncrementerClientEndPoint client = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);

        int result = await client.IncrementAsync(testValue, CancellationToken);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task Increments99To100()
    {
        const int testValue = 99;
        const int expectedResult = 100;

        using IncrementerServer server = IncrementerServer.Start(Incrementer.Create, LoggerFactory);
        IncrementerClientEndPoint client = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);

        int result = await client.IncrementAsync(testValue, CancellationToken);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task IncrementsMinus1To0()
    {
        const int testValue = -1;
        const int expectedResult = 0;

        using IncrementerServer server = IncrementerServer.Start(Incrementer.Create, LoggerFactory);
        IncrementerClientEndPoint client = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);

        int result = await client.IncrementAsync(testValue, CancellationToken);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task Increments0To10()
    {
        const int testValue = 0;
        const int expectedResult = 10;

        using IncrementerServer server = IncrementerServer.Start(Incrementer.Create, LoggerFactory);
        IncrementerClientEndPoint client = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);

        int lastResult = testValue;
        for (int i = 0; i < expectedResult - testValue; i++)
        {
            lastResult = await client.IncrementAsync(lastResult, CancellationToken);
        }

        Assert.AreEqual(expectedResult, lastResult);
    }

    [TestMethod]
    public async Task Increments0To10Stored()
    {
        const int testValue = 0;
        const int expectedResult = 10;

        using IncrementerServer server = IncrementerServer.Start(Incrementer.Create, LoggerFactory);
        IncrementerClientEndPoint client = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);

        await client.StoreAsync(testValue, CancellationToken);
        for (int i = 0; i < 10; i++)
        {
            await client.IncrementStoredAsync(CancellationToken);
        }

        int result = await client.GetStoredAsync(CancellationToken);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    [ExpectedException(typeof(EndPointRanToCompletionException))]
    public async Task IncrementingAfterFinishingThrowsRanToCompletionException()
    {
        using IncrementerServer server = IncrementerServer.Start(Incrementer.Create, LoggerFactory);
        IncrementerClientEndPoint client = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);
        await client.FinishAsync(CancellationToken);
        try
        {
            await client.IncrementAsync(0, CancellationToken);
        }
        catch (EndPointRanToCompletionException exception)
        {
            Logger.LogInformation("Exception message: {ExceptionMessage}", exception.Message);
            throw;
        }
    }

    private static void WaitForThreads()
    {
        Thread.Sleep(100);
    }
}