using System.CodeDom.Compiler;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
public class SocketWrapperTest : Test
{
    [TestMethod]
    public async Task ClosingConnectionStopsListening()
    {
        IPEndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<TestSocketWrapper.ListenResult> serverTask = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var clientRpcSocket = new TestSocketWrapper(NetworkUtility.LocalHost.AddressFamily);
        await clientRpcSocket.ConnectAsync(ep);

        //byte[] message = Array.Empty<byte>();
        //await clientRpcSocket.SendMessageAsync(message);
        clientRpcSocket.Close();

        TestSocketWrapper.ListenResult serverResult = await serverTask;
        Assert.IsTrue(serverResult.ReturnCode == ListenReturnCode.ConnectionClosed);
    }

    [TestMethod]
    public async Task SingleByteMessagesGetsReceived()
    {
        IPEndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<TestSocketWrapper.ListenResult> serverTask = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var clientRpcSocket = new TestSocketWrapper(NetworkUtility.LocalHost.AddressFamily);
        await clientRpcSocket.ConnectAsync(ep);

        const byte value = 123;
        byte[] message = { value };
        await clientRpcSocket.SendAsync(message);
        clientRpcSocket.Close();

        TestSocketWrapper.ListenResult serverResult = await serverTask;

        Log(serverResult);

        Assert.IsTrue(serverResult.ReturnCode == ListenReturnCode.ConnectionClosed);

        List<byte[]> receivedMessages = serverResult.Messages;

        Assert.IsTrue(receivedMessages.Count > 0);

        Assert.AreEqual(value, receivedMessages[0][0]);
    }

    [TestMethod]
    public async Task EmptyMessageGetsReceived()
    {
        IPEndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<TestSocketWrapper.ListenResult> serverTask = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var clientRpcSocket = new TestSocketWrapper(NetworkUtility.LocalHost.AddressFamily);
        await clientRpcSocket.ConnectAsync(ep);

        await clientRpcSocket.SendAsync(Array.Empty<byte>());
        clientRpcSocket.Close();

        TestSocketWrapper.ListenResult serverResult = await serverTask;
        Log(serverResult);
        Assert.IsTrue(serverResult.ReturnCode == ListenReturnCode.ConnectionClosed);
        List<byte[]> receivedMessages = serverResult.Messages;
        Assert.IsTrue(receivedMessages.Count > 0);
        Assert.IsTrue(receivedMessages[0].Length == 0);
    }

    private static void Log(TestSocketWrapper.ListenResult serverResult)
    {
        List<byte[]> receivedMessages = serverResult.Messages;

        using var writer = new IndentedTextWriter(Console.Out);

        writer.WriteLine($"server result has return type: {serverResult.ReturnCode}");

        writer.Indent++;
        writer.WriteLine($"received {receivedMessages.Count} messages:");

        writer.Indent++;
        for (int i = 0; i < receivedMessages.Count; i++)
        {
            byte[] message = receivedMessages[i];
            writer.WriteLine($"message {i} is {message.Length} bytes:");

            writer.Indent++;
            writer.WriteLine($"{{{string.Join(',', message)}}}");
            writer.Indent--;
        }

        writer.Indent--;

        writer.Indent--;
    }
}