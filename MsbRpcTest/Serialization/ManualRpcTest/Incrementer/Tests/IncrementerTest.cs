using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;
using MsbRpcTest.Serialization.Network.Utility;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
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
        (Messenger clientMessenger, Messenger serverMessenger) = (await Connection.ConnectAsync(cancellationToken)).AsMessengers;
        var serverImplementation = new Implementation.Incrementer();
        var server = new IncrementerServerEndPoint(serverMessenger, serverImplementation);
        
        Task<Messenger.ListenReturnCode> serverListenTask = server.ListenAsync(cancellationToken);
        
        var client = new IncrementerClientEndPoint(clientMessenger);
        int result = await client.IncrementAsync(testValue, cancellationToken);
        
        clientMessenger.Dispose();
        
        await serverListenTask;
        
        server.Dispose();

        Assert.AreEqual(expectedResult, result);
    }
    
    [TestMethod]
    public async Task Increments99To100()
    {
        const int testValue = 99;
        const int expectedResult = 100;
        
        CancellationToken cancellationToken = CancellationToken;
        (Messenger clientMessenger, Messenger serverMessenger) = (await Connection.ConnectAsync(cancellationToken)).AsMessengers;
        var serverImplementation = new Implementation.Incrementer();
        var server = new IncrementerServerEndPoint(serverMessenger, serverImplementation);
        
        Task<Messenger.ListenReturnCode> serverListenTask = server.ListenAsync(cancellationToken);
        
        var client = new IncrementerClientEndPoint(clientMessenger);
        int result = await client.IncrementAsync(testValue, cancellationToken);
        
        clientMessenger.Dispose();
        await serverListenTask;
        server.Dispose();

        Assert.AreEqual(expectedResult, result);
    }
    
    [TestMethod]
    public async Task IncrementsMinus1To0()
    {
        const int testValue = 99;
        const int expectedResult = 100;
        
        CancellationToken cancellationToken = CancellationToken;
        (Messenger clientMessenger, Messenger serverMessenger) = (await Connection.ConnectAsync(cancellationToken)).AsMessengers;
        var serverImplementation = new Implementation.Incrementer();
        var server = new IncrementerServerEndPoint(serverMessenger, serverImplementation);
        
        Task<Messenger.ListenReturnCode> serverListenTask = server.ListenAsync(cancellationToken);
        
        var client = new IncrementerClientEndPoint(clientMessenger);
        int result = await client.IncrementAsync(testValue, cancellationToken);
        
        clientMessenger.Dispose();
        await serverListenTask;
        server.Dispose();

        Assert.AreEqual(expectedResult, result);
    }
    
    [TestMethod]
    public async Task Increments0To10()
    {
        const int testValue = 0;
        const int expectedResult = 10;
        
        CancellationToken cancellationToken = CancellationToken;
        (Messenger clientMessenger, Messenger serverMessenger) = (await Connection.ConnectAsync(cancellationToken)).AsMessengers;
        var serverImplementation = new Implementation.Incrementer();
        var server = new IncrementerServerEndPoint(serverMessenger, serverImplementation);
        
        Task<Messenger.ListenReturnCode> serverListenTask = server.ListenAsync(cancellationToken);
        
        var client = new IncrementerClientEndPoint(clientMessenger);
        
        int lastResult = testValue;
        for (int i = 0; i < expectedResult-testValue; i++)
        {
            lastResult = await client.IncrementAsync(lastResult, cancellationToken);
        }
        
        clientMessenger.Dispose();
        await serverListenTask;
        server.Dispose();

        Assert.AreEqual(expectedResult, lastResult);
    }
}