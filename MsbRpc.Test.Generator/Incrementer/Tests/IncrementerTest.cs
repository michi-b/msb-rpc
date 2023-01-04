using System.Net;
using System.Net.Sockets;
using System.Threading;
using Incrementer.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpc.Sockets;

namespace MsbRpc.Test.Generator.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
    [TestMethod]
    public void CanServerListen()
    {
        IncrementerServer server = IncrementerServer.Start(() => new Incrementer(), LoggerFactory);
        Thread.Sleep(100);
        server.Dispose();
    }

    [TestMethod]
    public void CanConnectAndDisconnect()
    {
        IncrementerServer server = IncrementerServer.Start(()=> new Incrementer(), LoggerFactory);
        
        IPAddress localHost = Dns.GetHostEntry("localhost").AddressList[0];
        int port = server.Port;
        
        Socket clientSocket = new(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(localHost, port);
        var clientMessenger = new Messenger(new RpcSocket(clientSocket));

        IncrementerClientEndPoint client = new(clientMessenger, LoggerFactory);
        Thread.Sleep(100);
        Assert.AreEqual(1, server.CreateConnectionDump().Length);
        client.Dispose();

        Thread.Sleep(100);
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