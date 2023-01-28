using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Exceptions;
using MsbRpc.Test.Generator.Incrementer.ToGenerate;
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
        IncrementerServer server = StartServer();
        WaitForThreads();
        server.Dispose();
    }

    [TestMethod]
    public async Task ConnectsAndDisconnects()
    {
        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);

        WaitForThreads();
        Assert.AreEqual(1, server.CreateConnectionDump().Length);

        client.Dispose();

        WaitForThreads();
        Assert.AreEqual(0, server.CreateConnectionDump().Length);
    }

    [TestMethod]
    public async Task ConnectsAndDisconnectsTwoClients()
    {
        using IncrementerServer server = StartServer();

        IncrementerClientEndPoint client1 = await ConnectClient(server);
        WaitForThreads();
        Assert.AreEqual(1, server.CreateConnectionDump().Length);

        IncrementerClientEndPoint client2 = await ConnectClient(server);
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
        using (IncrementerServer server = StartServer())
        {
            client = await ConnectClient(server);
        }

        client.Dispose();
    }

    [TestMethod]
    public async Task Increments0To1()
    {
        const int testValue = 0;
        const int expectedResult = 1;

        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);

        int result = await client.IncrementAsync(testValue);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task Increments99To100()
    {
        const int testValue = 99;
        const int expectedResult = 100;

        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);

        int result = await client.IncrementAsync(testValue);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task IncrementsMinus1To0()
    {
        const int testValue = -1;
        const int expectedResult = 0;

        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);

        int result = await client.IncrementAsync(testValue);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task Increments0To10()
    {
        const int testValue = 0;
        const int expectedResult = 10;

        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);

        int lastResult = testValue;
        for (int i = 0; i < expectedResult - testValue; i++)
        {
            lastResult = await client.IncrementAsync(lastResult);
        }

        Assert.AreEqual(expectedResult, lastResult);
    }

    [TestMethod]
    public async Task Increments0To10Stored()
    {
        const int testValue = 0;
        const int expectedResult = 10;

        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);

        await client.StoreAsync(testValue);
        for (int i = 0; i < 10; i++)
        {
            await client.IncrementStoredAsync();
        }

        int result = await client.GetStoredAsync();

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task FinishProcedureMakesEndPointsRunToCompletion()
    {
        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);
        Assert.IsFalse(client.RanToCompletion);
        await client.FinishAsync();
        Assert.IsTrue(client.RanToCompletion);
    }

    [TestMethod]
    [ExpectedException(typeof(EndPointRanToCompletionException))]
    public async Task IncrementingAfterFinishingThrowsRanToCompletionException()
    {
        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);
        await client.FinishAsync();
        try
        {
            await client.IncrementAsync(0);
        }
        catch (EndPointRanToCompletionException exception)
        {
            Logger.LogInformation("Exception message: {ExceptionMessage}", exception.Message);
            throw;
        }
    }

    [TestMethod]
    public async Task IncrementsString()
    {
        const string value = "0";
        const string expected = "1";
        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);
        string result = await client.IncrementStringAsync(value);
        Assert.AreEqual(expected, result);
    }

    private static void ConfigureServer(IncrementerServer.Configuration configuration)
    {
        configuration.LoggerFactory = LoggerFactory;
    }

    private static void ConfigureServerEndPoint(IncrementerServerEndPoint.Configuration configuration)
    {
        configuration.LoggerFactory = LoggerFactory;
    }

    private static void ConfigureClientEndPoint(IncrementerClientEndPoint.Configuration configuration)
    {
        configuration.LoggerFactory = LoggerFactory;
    }

    private static ValueTask<IncrementerClientEndPoint> ConnectClient(IncrementerServer server)
    {
        IPEndPoint endPoint = new(LocalHost, server.Port);
        return IncrementerClientEndPoint.ConnectAsync(endPoint, ConfigureClientEndPoint);
    }

    private IncrementerServer StartServer()
    {
        return IncrementerServer.Start(() => new Incrementer(), ConfigureServer, ConfigureServerEndPoint);
    }

    private static void WaitForThreads()
    {
        Thread.Sleep(100);
    }
}