#region

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Configuration.Builders;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Servers.Listener;
using Listener = MsbRpc.Servers.Listener.MessengerListener;

#endregion

namespace MsbRpc.Test.Network.MessengerListener;

[TestClass]
public class MessengerListenerTest : Base.Test
{
    private readonly MessengerListenerConfigurationBuilder _configuration;

    private readonly ILogger<MessengerListenerTest> _logger;

    private readonly ILoggerFactory _loggerFactory;

    public MessengerListenerTest()
    {
        _loggerFactory = CreateLoggerFactory();
        _logger = _loggerFactory.CreateLogger<MessengerListenerTest>();
        _configuration = new MessengerListenerConfigurationBuilder { LoggerFactory = _loggerFactory };
    }

    [TestMethod]
    public void Listens()
    {
        Listener listener = Listener.Start(_configuration, new NullConnectionReceiver());
        listener.Dispose();
    }

    [TestMethod]
    public async Task ConnectsOnce()
    {
        var buffer = new RpcBuffer();
        var messengerReceiver = new MockConnectionReceiver();
        Listener listener = Listener.Start(_configuration, messengerReceiver);
        await Messenger.ConnectAsync(listener.EndPoint, buffer, _loggerFactory);
        await WaitForConditionAsync(() => messengerReceiver.Messengers.Count == 1, _logger, CancellationToken);
        Assert.AreEqual(1, messengerReceiver.Messengers.Count);
        listener.Dispose();
    }

    [TestMethod]
    public async Task ConnectsIdentified()
    {
        var buffer = new RpcBuffer();
        var messengerReceiver = new MockConnectionReceiver();
        Listener listener = Listener.Start(_configuration, messengerReceiver);
        IdentifiedConnectionTask connectionTask = listener.Schedule();
        _logger.Log(LogLevel.Information, "Scheduled connection task with id {ConnectionId}", connectionTask.Id);
        await Messenger.ConnectAsync(listener.EndPoint, connectionTask.Id, buffer, _loggerFactory);
        Messenger messenger = connectionTask.Await();
        Assert.IsNotNull(messenger);
        Assert.IsTrue(messenger.IsConnected);
        Assert.AreEqual(0, messengerReceiver.Messengers.Count);
        listener.Dispose();
    }
}