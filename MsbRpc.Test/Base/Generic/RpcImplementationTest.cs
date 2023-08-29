#region

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Configuration.Builders;
using MsbRpc.Contracts;
using MsbRpc.EndPoints;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listeners;

#endregion

namespace MsbRpc.Test.Base.Generic;

public abstract class RpcImplementationTest<TTest, TServerEndPoint, TClientEndPoint, TProcedure, TContract> : Test<TTest>
    where TTest : RpcImplementationTest<TTest, TServerEndPoint, TClientEndPoint, TProcedure, TContract>
    where TServerEndPoint : InboundEndPoint<TProcedure, TContract>
    where TClientEndPoint : OutboundEndPoint<TProcedure>
    where TProcedure : Enum
    where TContract : IRpcContract
{
    private readonly int _port;

    protected virtual string ListenerName => $"{typeof(TTest).Name}{nameof(MessengerListener)}";

    protected RpcImplementationTest(int port) => _port = port;

    protected virtual void ConfigureListener(MessengerListenerConfigurator configurator)
    {
        configurator.Port = _port;
        configurator.LoggerFactory = LoggerFactory;
        configurator.WithName(ListenerName);
    }

    private MessengerListener StartListen()
    {
        MessengerListenerConfigurator configurator = new();
        ConfigureListener(configurator);
        MessengerListener listener = new(new MessengerListenerConfigurator());
        listener.StartListening(new ConnectionReceiver<TServerEndPoint, TProcedure, TContract>(CreateServerEndPoint, LoggerFactory, CancellationToken));
        return listener;
    }

    protected abstract TServerEndPoint CreateServerEndPoint(Messenger messenger);

    protected void TestListens()
    {
        MessengerListener messengerListener;
        using (messengerListener = StartListen())
        {
            for (int timeout = 0; timeout < 10; timeout++)
            {
                Thread.Sleep(timeout);
                Assert.IsTrue(messengerListener.IsListening);
            }
        }

        Assert.IsFalse(messengerListener.IsListening);
    }

    protected async Task TestCanDisposeServerPrematurely()
    {
        //todo: reimplement this test
        // TClientEndPoint client;
        // using (TServer server = StartServer())
        // {
        //     client = await ConnectAsync(server);
        // }
        //
        // client.Dispose();
    }

    protected async Task TestConnectsAndDisconnects()
    {
        //todo: reimplement this test
        // using TServer server = StartServer();
        // TClientEndPoint client = await ConnectAsync(server);
        // await ServerTestUtility.AssertBecomesEqual(1, () => server.EndPoints.Length);
        // client.Dispose();
        // await ServerTestUtility.AssertBecomesEqual(0, () => server.EndPoints.Length);
    }

    protected async Task TestConnectsAndDisconnectsTwoClients()
    {
        //todo: reimplement this test
        // using TServer server = StartServer();
        //
        // TClientEndPoint client1 = await ConnectAsync(server);
        // await ServerTestUtility.AssertBecomesEqual(1, () => server.EndPoints.Length);
        //
        // TClientEndPoint client2 = await ConnectAsync(server);
        // await ServerTestUtility.AssertBecomesEqual(2, () => server.EndPoints.Length);
        //
        // client1.Dispose();
        // await ServerTestUtility.AssertBecomesEqual(1, () => server.EndPoints.Length);
        //
        // client2.Dispose();
        // await ServerTestUtility.AssertBecomesEqual(0, () => server.EndPoints.Length);
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