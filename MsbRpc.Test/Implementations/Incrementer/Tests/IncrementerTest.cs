using System.Net;
using Incrementer;
using Incrementer.Generated;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Configuration;
using MsbRpc.Exceptions;
using MsbRpc.Exceptions.Generic;
using MsbRpc.Test.Base.Generic;
using MsbRpc.Test.Implementations.Incrementer.ToGenerate;
using MsbRpc.Test.Utility;

namespace MsbRpc.Test.Implementations.Incrementer.Tests;

[TestClass]
public class IncrementerTest : ServerTest<IncrementerTest,
    IncrementerServer,
    IncrementerServerEndPoint,
    IncrementerClientEndPoint,
    IncrementerProcedure,
    IIncrementer>
{
    [TestMethod]
    public void Listens()
    {
        TestListens();
    }

    [TestMethod]
    public async Task ConnectsAndDisconnects()
    {
        await TestConnectsAndDisconnects();
    }

    [TestMethod]
    public async Task ConnectsAndDisconnectsTwoClients()
    {
        await TestConnectsAndDisconnectsTwoClients();
    }

    [TestMethod]
    public async Task CanDisposeServerPrematurely()
    {
        await TestCanDisposeServerPrematurely();
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

    [TestMethod]
    [ExpectedException(typeof(RpcRemoteException<IncrementerProcedure>))]
    public async Task IncrementInvalidStringThrowsWithNoExceptionDetails()
    {
        const string value = "a";
        using IncrementerServer server = StartServer();
        IncrementerClientEndPoint client = await ConnectClient(server);
        await client.IncrementStringAsync(value);
        try
        {
            await client.IncrementStringAsync(value);
        }
        catch (RpcRemoteException<IncrementerProcedure> exception)
        {
            RpcExceptionTransmission transmission = exception.RemoteExceptionTransmission;
            Assert.IsFalse(transmission.HasExceptionTypeName);
            Assert.IsFalse(transmission.HasExceptionMessage);
            Assert.IsFalse(transmission.HasRemoteContinuation);
            Assert.IsFalse(transmission.HasSourceExecutionStage);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(RpcRemoteException<IncrementerProcedure>))]
    public async Task IncrementInvalidStringThrowsWithTransmittedExceptionTypeName()
    {
        const string value = "a";
        using IncrementerServer server = StartServer(RpcExceptionTransmissionOptions.ExceptionTypeName);
        IncrementerClientEndPoint client = await ConnectClient(server);
        try
        {
            await client.IncrementStringAsync(value);
        }
        catch (RpcRemoteException<IncrementerProcedure> exception)
        {
            RpcExceptionTransmission transmission = exception.RemoteExceptionTransmission;
            Assert.IsTrue(transmission.HasExceptionTypeName);
            LogTransmittedExceptionTypeName(transmission);
            Assert.IsFalse(transmission.HasExceptionMessage);
            Assert.IsFalse(transmission.HasSourceExecutionStage);
            Assert.IsFalse(transmission.HasRemoteContinuation);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(RpcRemoteException<IncrementerProcedure>))]
    public async Task IncrementInvalidStringThrowsWithTransmittedExceptionMessage()
    {
        const string value = "a";
        using IncrementerServer server = StartServer(RpcExceptionTransmissionOptions.ExceptionMessage);
        IncrementerClientEndPoint client = await ConnectClient(server);
        try
        {
            await client.IncrementStringAsync(value);
        }
        catch (RpcRemoteException<IncrementerProcedure> exception)
        {
            RpcExceptionTransmission transmission = exception.RemoteExceptionTransmission;
            Assert.IsTrue(transmission.HasExceptionMessage);
            LogTransmittedExceptionMessage(transmission);
            Assert.IsFalse(transmission.HasExceptionTypeName);
            Assert.IsFalse(transmission.HasSourceExecutionStage);
            Assert.IsFalse(transmission.HasRemoteContinuation);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(RpcRemoteException<IncrementerProcedure>))]
    public async Task IncrementInvalidStringThrowsWithTransmittedSourceExecutionStage()
    {
        const string value = "a";
        using IncrementerServer server = StartServer(RpcExceptionTransmissionOptions.SourceExecutionStage);
        IncrementerClientEndPoint client = await ConnectClient(server);
        try
        {
            await client.IncrementStringAsync(value);
        }
        catch (RpcRemoteException<IncrementerProcedure> exception)
        {
            RpcExceptionTransmission transmission = exception.RemoteExceptionTransmission;
            Assert.IsTrue(transmission.HasSourceExecutionStage);
            LogTransmittedSourceExecutionStage(transmission);
            Assert.IsFalse(transmission.HasExceptionTypeName);
            Assert.IsFalse(transmission.HasExceptionMessage);
            Assert.IsFalse(transmission.HasRemoteContinuation);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(RpcRemoteException<IncrementerProcedure>))]
    public async Task IncrementInvalidStringThrowsWithTransmittedRemoteContinuation()
    {
        const string value = "a";
        using IncrementerServer server = StartServer(RpcExceptionTransmissionOptions.RemoteContinuation);
        IncrementerClientEndPoint client = await ConnectClient(server);
        try
        {
            await client.IncrementStringAsync(value);
        }
        catch (RpcRemoteException<IncrementerProcedure> exception)
        {
            RpcExceptionTransmission transmission = exception.RemoteExceptionTransmission;
            Assert.IsTrue(transmission.HasRemoteContinuation);
            LogTransmittedRemoteContinuation(transmission);
            Assert.IsFalse(transmission.HasExceptionTypeName);
            Assert.IsFalse(transmission.HasExceptionMessage);
            Assert.IsFalse(transmission.HasSourceExecutionStage);
            throw;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(RpcRemoteException<IncrementerProcedure>))]
    public async Task IncrementInvalidStringThrowsWithAllTransmittedInformation()
    {
        const string value = "a";
        using IncrementerServer server = StartServer(RpcExceptionTransmissionOptions.All);
        IncrementerClientEndPoint client = await ConnectClient(server);
        try
        {
            await client.IncrementStringAsync(value);
        }
        catch (RpcRemoteException<IncrementerProcedure> exception)
        {
            RpcExceptionTransmission transmission = exception.RemoteExceptionTransmission;

            Assert.IsTrue(transmission.HasExceptionTypeName);
            LogTransmittedExceptionTypeName(transmission);

            Assert.IsTrue(transmission.HasExceptionMessage);
            LogTransmittedExceptionMessage(transmission);

            Assert.IsTrue(transmission.HasSourceExecutionStage);
            LogTransmittedSourceExecutionStage(transmission);

            Assert.IsTrue(transmission.HasRemoteContinuation);
            LogTransmittedRemoteContinuation(transmission);

            throw;
        }
    }

    protected override IncrementerServer CreateServer(RpcExceptionTransmissionOptions exceptionTransmissionOptions = RpcExceptionTransmissionOptions.None)
        => new(TestUtility.LoggerFactory, exceptionTransmissionOptions);

    protected override async ValueTask<IncrementerClientEndPoint> ConnectClient(IPEndPoint endPoint, OutboundEndPointConfiguration configuration)
        => await IncrementerClientEndPoint.ConnectAsync(endPoint, configuration);
}