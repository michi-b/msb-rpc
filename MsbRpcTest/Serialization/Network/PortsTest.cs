using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
public class PortsTest : Test
{
    [TestMethod]
    public async Task TestPort1234()
    {
        CancellationToken cancellationToken = CancellationToken;

        IPAddress localHost = (await Dns.GetHostEntryAsync("localhost", cancellationToken)).AddressList[0];

        using var listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listenSocket.Bind(new IPEndPoint(localHost, 0));
        var listenEndPoint = (IPEndPoint)listenSocket.LocalEndPoint!;
        Console.WriteLine("using port {0}", listenEndPoint.Port);
        listenSocket.Listen();

        ValueTask<Socket> listenTask = listenSocket.AcceptAsync(cancellationToken);
        var clientSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        await clientSocket.ConnectAsync(listenEndPoint, cancellationToken);
        Socket serverSocket = await listenTask;

        Assert.IsTrue(clientSocket.Connected);
        Assert.IsTrue(serverSocket.Connected);
    }
}