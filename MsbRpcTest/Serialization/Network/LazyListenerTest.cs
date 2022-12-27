using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpcTest.Serialization.Network.Utility.Listeners;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
public class LazyListenerTest : ListenerTest
{
    [TestMethod]
    public async Task ClosingConnectionStopsListening()
    {
        await TestClosingConnectionStopsListening();
    }

    [TestMethod]
    public async Task SingleByteMessagesIsDelivered()
    {
        await TestSingleByteMessagesIsDelivered();
    }

    [TestMethod]
    public async Task EmptyMessageIsDelivered()
    {
        await TestEmptyMessageIsDelivered();
    }

    [TestMethod]
    public async Task IntMessageIsDelivered()
    {
        await TestIntMessageIsDelivered();
    }

    [TestMethod]
    public async Task MultipleIntMessagesAreDelivered()
    {
        await TestMultipleIntMessagesAreDelivered();
    }

    protected override Task<List<ArraySegment<byte>>> Listen(Messenger messenger)
        => MessagesListener.Listen(messenger, TaskCreationOptions.None);
}