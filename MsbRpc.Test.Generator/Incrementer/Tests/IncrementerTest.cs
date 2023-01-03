using System;
using System.Threading;
using System.Threading.Tasks;
using Incrementer.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Test.Network.Utility;

namespace MsbRpc.Test.Generator.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
    [TestMethod]
    public void CanServerListen()
    {
        IncrementerServer server = IncrementerServer.Start(() => new Incrementer(), LoggerFactory);
        Thread.Sleep(1000);
        server.Dispose();
    }

    [TestMethod]
    public void CanConnectToServer()
    {
            
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