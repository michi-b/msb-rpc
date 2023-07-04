using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Configuration.Builders;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using Listener = MsbRpc.Servers.Listener.MessengerListener;

namespace MsbRpc.Test.Network.MessengerListener;

[TestClass]
public class MessengerListenerTest : Base.Test
{
    private readonly MessengerListenerConfigurationBuilder _configuration;

    private ILogger<MessengerListenerTest> _logger;

    public MessengerListenerTest()
    {
        ILoggerFactory loggerFactory = CreateLoggerFactory();
        _logger = loggerFactory.CreateLogger<MessengerListenerTest>();
        _configuration = new MessengerListenerConfigurationBuilder { LoggerFactory = loggerFactory };
    }

    [TestMethod]
    public void Listens()
    {
        Listener listener = Listener.Start(_configuration, new NullMessengerReceiver());
        listener.Dispose();
    }

    // Listener CreateListener()
    // {
    //     
    // }

    [TestMethod]
    public async Task ConnectsOnce()
    {
        var buffer = new RpcBuffer();
        var messengerReceiver = new MockMessengerReceiver();
        Listener listener = Listener.Start(_configuration, messengerReceiver);
        await Messenger.ConnectAsync(listener.EndPoint, buffer);
        await WaitForConditionAsync(() => messengerReceiver.Messengers.Count == 1, _logger, CancellationToken);
        Assert.AreEqual(1, messengerReceiver.Messengers.Count);
        listener.Dispose();
    }

    // [TestMethod]
    // public async Task ConnectsIdentified() { }
}