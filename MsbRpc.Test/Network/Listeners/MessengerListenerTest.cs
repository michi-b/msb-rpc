#region

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Configuration.Builders;
using MsbRpc.Messaging;
using MsbRpc.Servers.Listeners;
using MsbRpc.Test.Base.Generic;

#endregion

namespace MsbRpc.Test.Network.Listeners;

[TestClass]
public class MessengerListenerTest : Test<MessengerListenerTest>
{
    private readonly MessengerListenerConfigurator _configuration;

    public MessengerListenerTest() => _configuration = new MessengerListenerConfigurator { LoggerFactory = LoggerFactory };

    [TestMethod]
    public void Listens()
    {
        using MessengerListener listener = new(_configuration);
        listener.StartListening(new NullConnectionReceiver());
    }

    [TestMethod]
    public async Task ConnectsOnce()
    {
        var messengerReceiver = new MockConnectionReceiver();
        using var listener = new MessengerListener(_configuration);
        Task listenTask = listener.ListenAsync(messengerReceiver, CancellationToken);
        await Messenger.ConnectAsync(listener.EndPoint, Logger);
        await WaitForConditionAsync(() => messengerReceiver.Messengers.Count == 1, Logger, CancellationToken);
        Assert.AreEqual(1, messengerReceiver.Messengers.Count);
        listener.Dispose();
        await listenTask;
    }

    // [TestMethod]
    // public async Task ConnectsIdentified()
    // {
    //     var buffer = new RpcBuffer();
    //     var messengerReceiver = new MockConnectionReceiver();
    //     Listener listener = Listener.Start(_configuration, messengerReceiver);
    //     IdentifiedConnectionTask connectionTask = listener.Schedule();
    //     _logger.Log(LogLevel.Information, "Scheduled connection task with id {ConnectionId}", connectionTask.Id);
    //     await Messenger.ConnectAsync(listener.EndPoint, connectionTask.Id, buffer, _loggerFactory);
    //     Messenger messenger = connectionTask.Await();
    //     Assert.IsNotNull(messenger);
    //     Assert.IsTrue(messenger.IsConnected);
    //     Assert.AreEqual(0, messengerReceiver.Messengers.Count);
    //     listener.Dispose();
    // }
}