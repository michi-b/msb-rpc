﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpc.Test.ManualRpcTest.Incrementer.Generated;
using MsbRpc.Test.Serialization.Network.Utility;

namespace MsbRpc.Test.ManualRpcTest.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
    [TestMethod]
    public async Task CanServerListen()
    {
        CancellationToken cancellationToken = CancellationToken;

        using LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);

        var server = new IncrementerServerEndPoint(connection.CreateServerMessenger());

        Task<Messenger.ListenReturnCode> serverListenTask = CreateServerListenTask(server, cancellationToken);

        connection.CreateClientMessenger().Dispose();

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

    private static Task<Messenger.ListenReturnCode> CreateServerListenTask(IncrementerServerEndPoint server, CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew
        (
            () => server.Listen(new Implementation.Incrementer()),
            cancellationToken,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default
        );
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
            LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);

            var server = new IncrementerServerEndPoint(connection.CreateServerMessenger(), LoggerFactory);

            Task<Messenger.ListenReturnCode> listenTask = CreateServerListenTask(server, cancellationToken);

            var client = new IncrementerClientEndPoint(connection.CreateClientMessenger(), LoggerFactory);

            return new Setup(client, server, listenTask);
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            await ServerListenTask;
            Server.Dispose();
        }
    }
}