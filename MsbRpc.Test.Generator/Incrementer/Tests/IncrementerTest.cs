using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Incrementer.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpc.Test.Generator.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
    private static void WaitForThreads()
    {
        Thread.Sleep(100);
    }
    
    [TestMethod]
    public void CanServerListen()
    {
        IncrementerServer server = IncrementerServer.Start(() => new Incrementer(), LoggerFactory);
        WaitForThreads();
        server.Dispose();
    }

    [TestMethod]
    public async Task CanConnectAndDisconnect()
    {
        IncrementerServer server = IncrementerServer.Start(()=> new Incrementer(), LoggerFactory);
        IncrementerClientEndPoint client = await IncrementerClientEndPoint.ConnectAsync(LocalHost, server.Port, LoggerFactory);
        
        WaitForThreads();
        Assert.AreEqual(1, server.CreateConnectionDump().Length);
        
        client.Dispose();
        
        WaitForThreads();
        Assert.AreEqual(0, server.CreateConnectionDump().Length);
        
        server.Dispose();
    }
    
    [TestMethod]
    public async Task CanConnectTwoClientsAndDisconnect()
    {
        IncrementerServer server = IncrementerServer.Start(()=> new Incrementer(), LoggerFactory);
        
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
        
        server.Dispose();
    }
    
    //
    // [TestMethod]
    // public async Task Increments0To1()
    // {
    //     const int testValue = 0;
    //     const int expectedResult = 1;
    //
    //     CancellationToken cancellationToken = CancellationToken;
    //     int result;
    //     await using (var setup = await Setup.CreateAsync(cancellationToken))
    //     {
    //         IncrementerClientEndPoint client = setup.Client;
    //         result = await client.IncrementAsync(testValue, cancellationToken);
    //     }
    //
    //     Assert.AreEqual(expectedResult, result);
    // }
    //
    // [TestMethod]
    // public async Task Increments99To100()
    // {
    //     const int testValue = 99;
    //     const int expectedResult = 100;
    //
    //     CancellationToken cancellationToken = CancellationToken;
    //     int result;
    //     await using (var setup = await Setup.CreateAsync(cancellationToken))
    //     {
    //         result = await setup.Client.IncrementAsync(testValue, cancellationToken);
    //     }
    //
    //     Assert.AreEqual(expectedResult, result);
    // }
    //
    // [TestMethod]
    // public async Task IncrementsMinus1To0()
    // {
    //     const int testValue = -1;
    //     const int expectedResult = 0;
    //
    //     CancellationToken cancellationToken = CancellationToken;
    //     int result;
    //     await using (var setup = await Setup.CreateAsync(cancellationToken))
    //     {
    //         result = await setup.Client.IncrementAsync(testValue, cancellationToken);
    //     }
    //
    //     Assert.AreEqual(expectedResult, result);
    // }
    //
    // [TestMethod]
    // public async Task Increments0To10()
    // {
    //     const int testValue = 0;
    //     const int expectedResult = 10;
    //
    //     CancellationToken cancellationToken = CancellationToken;
    //
    //     int lastResult = testValue;
    //     await using (var setup = await Setup.CreateAsync(cancellationToken))
    //     {
    //         for (int i = 0; i < expectedResult - testValue; i++)
    //         {
    //             lastResult = await setup.Client.IncrementAsync(lastResult, cancellationToken);
    //         }
    //     }
    //
    //     Assert.AreEqual(expectedResult, lastResult);
    // }
}