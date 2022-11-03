using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
public class RpcSocketTest
{
    // ReSharper disable once MemberCanBePrivate.Global
    // MSTest needs the public setter
    public TestContext TestContext { private get; set; } = null!;

    [TestMethod]
    public async Task ReceiveSingleByteFromDisconnectedSocketReturnsFalse()
    {
        CancellationToken cancellationToken = TestContext.CancellationTokenSource.Token;

        IPAddress localHost = (await Dns.GetHostEntryAsync("localhost", cancellationToken)).AddressList[0];
        const int serverPort = 50000;

        EndPoint serverEndPoint = new IPEndPoint(localHost, serverPort);
        var listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listenSocket.Bind(serverEndPoint);
        listenSocket.Listen(1);
        ValueTask<Socket> accept = listenSocket.AcceptAsync(cancellationToken);

        var senderSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        await senderSocket.ConnectAsync(serverEndPoint, cancellationToken);

        //establish connection
        Socket receiverSocket = await accept;

        //now immediately disconnect the receiver socket
        await receiverSocket.DisconnectAsync(false, cancellationToken);
        Assert.IsFalse(receiverSocket.Connected);

        //now lets send some random value through the sender socket        
        const byte value = 123;
        var sendSegment = new ArraySegment<byte>(new[] { value });
        await senderSocket.SendAsync(sendSegment, SocketFlags.None, cancellationToken);

        //now lets try to receive the value from the receiver socket
        var receiveSegment = new ArraySegment<byte>(new byte[] { 0 });
        int bytesRead = await receiverSocket.ReceiveAsync(receiveSegment, SocketFlags.None, cancellationToken);

        //log receivedValue
        byte receivedValue = receiveSegment[0];
        Console.WriteLine($"Received: {receivedValue}");

        //expect zero bytes read (actually 1)
        Assert.AreEqual(0, bytesRead);
        //expect the receive buffer to still contain the initialization value 0 (actually the sent byte 123)
        Assert.AreNotEqual(value, receivedValue);
    }
}