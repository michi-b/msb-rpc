using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;
using MsbRpcTest.Serialization.Network.Utility;
using Serilog;
using Serilog.Core;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
    private static readonly ILoggerFactory LoggerFactory;

    static IncrementerTest()
    {
        Logger logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();
        LoggerFactory = new LoggerFactory().AddSerilog(logger);
    }

    [TestMethod]
    public async Task CanServerListen()
    {
        CancellationToken cancellationToken = CancellationToken;
        (Messenger clientMessenger, Messenger serverMessenger) = (await Connection.ConnectAsync(cancellationToken)).AsMessengers;
        var serverImplementation = new Implementation.Incrementer();
        var server = new IncrementerServerEndPoint(serverMessenger, serverImplementation);

        Task<Messenger.ListenReturnCode> serverListenTask = server.ListenAsync(cancellationToken);

        clientMessenger.Dispose();

        await serverListenTask;

        server.Dispose();
    }

    [TestMethod]
    public async Task Increments0To1()
    {
        const int testValue = 0;
        const int expectedResult = 1;

        CancellationToken cancellationToken = CancellationToken;
        int result;
        await using (var setup = await Setup.CreateAsync(cancellationToken))
        {
            result = await setup.Client.IncrementAsync(testValue, cancellationToken);
        }

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task Increments99To100()
    {
        const int testValue = 99;
        const int expectedResult = 100;

        CancellationToken cancellationToken = CancellationToken;
        int result;
        await using (var setup = await Setup.CreateAsync(cancellationToken))
        {
            result = await setup.Client.IncrementAsync(testValue, cancellationToken);
        }

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task IncrementsMinus1To0()
    {
        const int testValue = -1;
        const int expectedResult = 0;

        CancellationToken cancellationToken = CancellationToken;
        int result;
        await using (var setup = await Setup.CreateAsync(cancellationToken))
        {
            result = await setup.Client.IncrementAsync(testValue, cancellationToken);
        }

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task Increments0To10()
    {
        const int testValue = 0;
        const int expectedResult = 10;

        CancellationToken cancellationToken = CancellationToken;

        int lastResult = testValue;
        await using (var setup = await Setup.CreateAsync(cancellationToken))
        {
            for (int i = 0; i < expectedResult - testValue; i++)
            {
                lastResult = await setup.Client.IncrementAsync(lastResult, cancellationToken);
            }
        }

        Assert.AreEqual(expectedResult, lastResult);
    }

    private class Setup : IAsyncDisposable
    {
        public IncrementerClientEndPoint Client { get; }
        private IncrementerServerEndPoint Server { get; }
        private Task<Messenger.ListenReturnCode> ServerListenTask { get; }

        private Setup(IncrementerClientEndPoint client, IncrementerServerEndPoint server, Task<Messenger.ListenReturnCode> serverListenTask)
        {
            Client = client;
            Server = server;
            ServerListenTask = serverListenTask;
        }

        public static async Task<Setup> CreateAsync(CancellationToken cancellationToken)
        {
            (Messenger clientMessenger, Messenger serverMessenger) = (await Connection.ConnectAsync(cancellationToken)).AsMessengers;
            var serverImplementation = new Implementation.Incrementer();
            var server = new IncrementerServerEndPoint(serverMessenger, serverImplementation, LoggerFactory);

            Task<Messenger.ListenReturnCode> serverListenTask = server.ListenAsync(cancellationToken);
            var client = new IncrementerClientEndPoint(clientMessenger);
            return new Setup(client, server, serverListenTask);
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            await ServerListenTask;
            Server.Dispose();
            ServerListenTask.Dispose();
        }
    }
}