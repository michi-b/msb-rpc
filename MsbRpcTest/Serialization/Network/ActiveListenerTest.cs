using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpcTest.Serialization.Network.Listeners;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
public class ActiveListenerTest : ListenerTest
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

    protected override SingleConnectionListener.GetReceiveMessagesTask CreateGetReceiveMessagesTask() => NetworkUtility.ReceiveMessagesActiveAsync;
}