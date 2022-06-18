﻿using System.CodeDom.Compiler;
using System.Net;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Primitives;

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

        clientRpcSocket.Close();

        TestSocketWrapper.ListenResult serverResult = await serverTask;

        Assert.AreEqual(ListenForMessagesReturnCode.ConnectionClosed, serverResult.ForMessagesReturnCode);
        Assert.AreEqual(0, serverResult.Messages.Count);
    }

    [TestMethod]
    public async Task SingleByteMessagesIsDelivered()
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
        Assert.AreEqual(ListenForMessagesReturnCode.ConnectionClosed, serverResult.ForMessagesReturnCode);

        List<byte[]> receivedMessages = serverResult.Messages;
        Assert.IsTrue(receivedMessages.Count == 1);
        Assert.AreEqual(value, receivedMessages[0][0]);
    }

    [TestMethod]
    public async Task EmptyMessageIsDelivered()
    {
        IPEndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<TestSocketWrapper.ListenResult> serverTask = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var clientRpcSocket = new TestSocketWrapper(NetworkUtility.LocalHost.AddressFamily);
        await clientRpcSocket.ConnectAsync(ep);

        await clientRpcSocket.SendAsync(Array.Empty<byte>());
        clientRpcSocket.Close();

        TestSocketWrapper.ListenResult serverResult = await serverTask;
        Log(serverResult);
        Assert.AreEqual(ListenForMessagesReturnCode.ConnectionClosed, serverResult.ForMessagesReturnCode);

        List<byte[]> receivedMessages = serverResult.Messages;
        Assert.IsTrue(receivedMessages.Count == 1);
        Assert.IsTrue(receivedMessages[0].Length == 0);
    }

    [TestMethod]
    public async Task IntMessageIsDelivered()
    {
        IPEndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<TestSocketWrapper.ListenResult> serverTask = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var clientRpcSocket = new TestSocketWrapper(NetworkUtility.LocalHost.AddressFamily);
        await clientRpcSocket.ConnectAsync(ep);

        const int value = -912347287;
        byte[] message = BitConverter.GetBytes(value);

        await clientRpcSocket.SendAsync(message);
        clientRpcSocket.Close();

        TestSocketWrapper.ListenResult serverResult = await serverTask;
        Log(serverResult);
        Assert.AreEqual(ListenForMessagesReturnCode.ConnectionClosed, serverResult.ForMessagesReturnCode);

        List<byte[]> receivedMessages = serverResult.Messages;
        Assert.IsTrue(receivedMessages.Count == 1);
        Assert.AreEqual(value, BitConverter.ToInt32(receivedMessages[0], 0));
    }

    [TestMethod]
    public async Task TooFewBytesReturnsConnectionClosedUnexpectedly()
    {
        IPEndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<TestSocketWrapper.ListenResult> serverTask = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var clientRpcSocket = new TestSocketWrapper(NetworkUtility.LocalHost.AddressFamily);
        await clientRpcSocket.ConnectAsync(ep);

        const int value = 96192;
        byte[] message = BitConverter.GetBytes(value);

        await clientRpcSocket.SendAsync(new ArraySegment<byte>(message, 0, 3), 4);
        clientRpcSocket.Close();

        TestSocketWrapper.ListenResult serverResult = await serverTask;
        Log(serverResult);
        Assert.AreEqual(ListenForMessagesReturnCode.ConnectionClosedUnexpectedly, serverResult.ForMessagesReturnCode);
        Assert.AreEqual(0, serverResult.Messages.Count);

    }

    [TestMethod]
    public async Task TooManyBytesReturnsConnectionClosedUnexpectedly()
    {
        IPEndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<TestSocketWrapper.ListenResult> serverTask = NetworkUtility.ReceiveMessagesAsync(ep, CancellationToken);
        var clientRpcSocket = new TestSocketWrapper(NetworkUtility.LocalHost.AddressFamily);
        await clientRpcSocket.ConnectAsync(ep);

        const int value = 96192;
        byte[] valueBytes = BitConverter.GetBytes(value);
        int valueLength = valueBytes.Length;
        byte[] message = new byte[valueLength + 1];
        Buffer.BlockCopy(valueBytes, 0, message, 0, valueLength);

        await clientRpcSocket.SendAsync(message, valueLength);
        clientRpcSocket.Close();

        TestSocketWrapper.ListenResult serverResult = await serverTask;
        Log(serverResult);
        Assert.AreEqual(ListenForMessagesReturnCode.ConnectionClosedUnexpectedly, serverResult.ForMessagesReturnCode);
        Assert.AreEqual(1, serverResult.Messages.Count);

    }

    private static void Log(TestSocketWrapper.ListenResult serverResult)
    {
        List<byte[]> receivedMessages = serverResult.Messages;

        using var writer = new IndentedTextWriter(Console.Out);

        writer.WriteLine($"server result has return type: {serverResult.ForMessagesReturnCode}");

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