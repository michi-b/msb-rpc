﻿#region

using System.Net;
using Microsoft.Extensions.Logging;
using MsbRpc.Contracts;
using MsbRpc.EndPoints;
using MsbRpc.Exceptions;
using MsbRpc.Servers;
using MsbRpc.Test.Utility;

#endregion

namespace MsbRpc.Test.Base.Generic;

public abstract class ServerTest<TTest, TServer, TServerEndPoint, TClientEndPoint, TProcedure, TContract> : Test<TTest>
    where TTest : ServerTest<TTest, TServer, TServerEndPoint, TClientEndPoint, TProcedure, TContract>
    where TServer : InboundEndPointServer
    where TServerEndPoint : InboundEndPoint<TProcedure, TContract>
    where TClientEndPoint : OutboundEndPoint<TProcedure>
    where TProcedure : Enum
    where TContract : IRpcContract
{
    protected async ValueTask<TClientEndPoint> ConnectAsync(Server server) => await ConnectAsync(server.ConnectionListener!.EndPoint);

    protected abstract ValueTask<TClientEndPoint> ConnectAsync(IPEndPoint serverEndPoint);

    protected TServer StartServer(RpcExceptionTransmissionOptions exceptionTransmissionOptions = RpcExceptionTransmissionOptions.None)
    {
        TServer server = CreateServer();
        return server;
    }

    protected abstract TServer CreateServer();

    protected void TestListens()
    {
        for (int timeout = 0; timeout < 10; timeout++)
        {
            TServer server = StartServer();
            Thread.Sleep(timeout);
            server.Dispose();
        }
    }

    protected async Task TestConnectsAndDisconnects()
    {
        using TServer server = StartServer();
        TClientEndPoint client = await ConnectAsync(server);
        await ServerTestUtility.AssertBecomesEqual(1, () => server.EndPoints.Length);
        client.Dispose();
        await ServerTestUtility.AssertBecomesEqual(0, () => server.EndPoints.Length);
    }

    protected async Task TestConnectsAndDisconnectsTwoClients()
    {
        using TServer server = StartServer();

        TClientEndPoint client1 = await ConnectAsync(server);
        await ServerTestUtility.AssertBecomesEqual(1, () => server.EndPoints.Length);

        TClientEndPoint client2 = await ConnectAsync(server);
        await ServerTestUtility.AssertBecomesEqual(2, () => server.EndPoints.Length);

        client1.Dispose();
        await ServerTestUtility.AssertBecomesEqual(1, () => server.EndPoints.Length);

        client2.Dispose();
        await ServerTestUtility.AssertBecomesEqual(0, () => server.EndPoints.Length);
    }

    protected async Task TestCanDisposeServerPrematurely()
    {
        TClientEndPoint client;
        using (TServer server = StartServer())
        {
            client = await ConnectAsync(server);
        }

        client.Dispose();
    }

    protected static void LogTransmittedExceptionTypeName(RpcExceptionTransmission transmission)
    {
        Logger.LogInformation("exception type name is '{ExceptionTypeName}'", transmission.ExceptionTypeName);
    }

    protected static void LogTransmittedExceptionMessage(RpcExceptionTransmission transmission)
    {
        Logger.LogInformation("exception message is '{ExceptionMessage}'", transmission.ExceptionMessage);
    }

    protected static void LogTransmittedSourceExecutionStage(RpcExceptionTransmission transmission)
    {
        Logger.LogInformation("source execution stage is '{SourceExecutionStage}'", transmission.SourceExecutionStage);
    }

    protected static void LogTransmittedRemoteContinuation(RpcExceptionTransmission transmission)
    {
        Logger.LogInformation("remote continuation is '{RemoteContinuation}'", transmission.RemoteContinuation);
    }
}