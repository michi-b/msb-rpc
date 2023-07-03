using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Network;
using MsbRpc.Test.Base.Generic;
using MsbRpc.Test.Network.Utility;

namespace MsbRpc.Test.Network;

[TestClass]
public class ConnectionTest : Test<ConnectionTest>
{
    [TestMethod]
    public async Task ConnectsToEphemeralPort()
    {
        CancellationToken cancellationToken = CancellationToken;

        IPAddress localHost = await NetworkUtility.GetLocalHostAsync();

        using var listenSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        listenSocket.Bind(new IPEndPoint(localHost, 0));
        var listenEndPoint = (IPEndPoint)listenSocket.LocalEndPoint!;
        Console.WriteLine("using port {0}", listenEndPoint.Port);
        listenSocket.Listen(1);

        ValueTask<Socket> listenTask = listenSocket.AcceptAsync(cancellationToken);
        var clientSocket = new Socket(localHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        await clientSocket.ConnectAsync(listenEndPoint, cancellationToken);
        Socket serverSocket = await listenTask;

        Assert.IsTrue(clientSocket.Connected);
        Assert.IsTrue(serverSocket.Connected);
    }

    [TestMethod]
    public async Task ConnectsToEphemeralListener()
    {
        CancellationToken cancellationToken = CancellationToken;

        var ephemeralListener = await EphemeralListener.CreateAsync(cancellationToken);

        using var clientSocket = new Socket(ephemeralListener.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        ValueTask<Socket> listenTask = ephemeralListener.AcceptAsync(cancellationToken);

        await clientSocket.ConnectAsync(ephemeralListener.EndPoint, cancellationToken);

        using Socket serverSocket = await listenTask;

        Assert.IsTrue(clientSocket.Connected);
        Assert.IsTrue(serverSocket.Connected);
    }
}