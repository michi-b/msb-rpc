using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Configuration.Builders;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using Listener = MsbRpc.Servers.Listener.MessengerListener;

namespace MsbRpc.Test.Network.MessengerListener;

[TestClass]
public class MessengerListenerTest : Base.Test
{
    private readonly ConnectionListenerConfigurationBuilder _configuration = new();

    [TestMethod]
    public void CanListen()
    {
        Listener listener = Listener.Start(_configuration, new NullMessengerReceiver());
        listener.Dispose();
    }

    [TestMethod]
    public async Task CanConnectOnce()
    {
        const int maxRetries = 100;
        var buffer = new RpcBuffer();
        var messengerReceiver = new MockMessengerReceiver();
        Listener listener = Listener.Start(_configuration, messengerReceiver);
        Messenger clientMessenger = await Messenger.ConnectAsync(listener.EndPoint, buffer);
        int retryCount = 0;
        while (!messengerReceiver.Messengers.TryDequeue(out Messenger connection))
        {
            Assert.AreNotEqual(retryCount++, maxRetries);
            Thread.Sleep(100);
        }
        listener.Dispose();
    }
}