using System.CodeDom.Compiler;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging.Messenger;
using MsbRpc.Serialization;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
public class SocketWrapperTest : Test
{
    [TestMethod]
    public async Task ClosingConnectionStopsListening()
    {
        var server = new SingleConnectionServer(CancellationToken);
        using (Messenger client = await server.Connect())
        {
            //do nothing, just close connection
        }
        SingleConnectionMessageReceiver.ListenResult serverResult = await server.Listen;

        Assert.AreEqual(ListenReturnCode.ConnectionClosed, serverResult.ReturnCode);
        Assert.AreEqual(0, serverResult.Messages.Count);
    }

    [TestMethod]
    public async Task SingleByteMessagesIsDelivered()
    {
       
        const byte value = 123;
        
        var server = new SingleConnectionServer(CancellationToken);
        using (Messenger client = await server.Connect())
        {
            //do nothing, just close connection
        }
        SingleConnectionMessageReceiver.ListenResult serverResult = await server.Listen;

        Log(serverResult);
        Assert.AreEqual(ListenReturnCode.ConnectionClosed, serverResult.ReturnCode);

        List<byte[]> receivedMessages = serverResult.Messages;
        Assert.IsTrue(receivedMessages.Count == 1);  
        Assert.AreEqual(value, receivedMessages[0][0]);
    }

    [TestMethod]
    public async Task EmptyMessageIsDelivered()
    {
        const byte value = 123;
        
        var server = new SingleConnectionServer(CancellationToken);
        using (Messenger client = await server.Connect())
        {
            byte[] message = Memory.Empty;
            await client.SendMessageAsync(message);
        }
        SingleConnectionMessageReceiver.ListenResult serverResult = await server.Listen;
        
        Log(serverResult);
        Assert.AreEqual(ListenReturnCode.ConnectionClosed, serverResult.ReturnCode);

        List<byte[]> receivedMessages = serverResult.Messages;
        Assert.IsTrue(receivedMessages.Count == 1);
        Assert.IsTrue(receivedMessages[0].Length == 0);
    }

    [TestMethod]
    public async Task IntMessageIsDelivered()
    {
        EndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<SingleConnectionMessageReceiver.ListenResult> listen = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var client = new SingleConnectionMessageReceiver(await NetworkUtility.CreateConnectedSocket(ep));

        const int value = -912347287;
        
        byte[] message = BitConverter.GetBytes(value);

        await clientRpcSocket.SendAsync(message);
        clientRpcSocket.Close();

        SingleConnectionMessageReceiver.ListenResult serverResult = await serverTask;
        Log(serverResult);
        Assert.AreEqual(ListenReturnCode.ConnectionClosed, serverResult.ReturnCode);

        List<byte[]> receivedMessages = serverResult.Messages;
        Assert.IsTrue(receivedMessages.Count == 1);
        Assert.AreEqual(value, BitConverter.ToInt32(receivedMessages[0], 0));
    }

    [TestMethod]
    public async Task TooFewBytesReturnsConnectionClosedUnexpectedly()
    {
        EndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<SingleConnectionMessageReceiver.ListenResult> listen = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var client = new SingleConnectionMessageReceiver(await NetworkUtility.CreateConnectedSocket(ep));

        const int value = 96192;
        byte[] message = BitConverter.GetBytes(value);

        await clientRpcSocket.SendAsync(new ArraySegment<byte>(message, 0, 3), 4);
        clientRpcSocket.Close();

        SingleConnectionMessageReceiver.ListenResult serverResult = await serverTask;
        Log(serverResult);
        Assert.AreEqual(ListenReturnCode.ConnectionClosedUnexpectedly, serverResult.ReturnCode);
        Assert.AreEqual(0, serverResult.Messages.Count);
    }

    [TestMethod]
    public async Task TooManyBytesReturnsConnectionClosedUnexpectedly()
    {
        EndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<SingleConnectionMessageReceiver.ListenResult> listen = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var client = new SingleConnectionMessageReceiver(await NetworkUtility.CreateConnectedSocket(ep));

        const int value = 96192;
        byte[] valueBytes = BitConverter.GetBytes(value);
        int valueLength = valueBytes.Length;
        byte[] message = new byte[valueLength + 1];
        Buffer.BlockCopy(valueBytes, 0, message, 0, valueLength);

        await clientRpcSocket.SendAsync(message, valueLength);
        clientRpcSocket.Close();

        SingleConnectionMessageReceiver.ListenResult serverResult = await serverTask;
        Log(serverResult);
        Assert.AreEqual(ListenReturnCode.ConnectionClosedUnexpectedly, serverResult.ReturnCode);
        Assert.AreEqual(1, serverResult.Messages.Count);
    }

    private static void Log(SingleConnectionMessageReceiver.ListenResult serverResult)
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