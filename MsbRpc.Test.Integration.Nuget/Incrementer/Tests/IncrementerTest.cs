using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Sockets;
using MsbRpc.Test.Integration.Nuget.Incrementer.Generated;

namespace MsbRpc.Test.Integration.Nuget.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
    [TestMethod]
    public async Task CanIncrement10TimesFrom5To15()
    {
        CancellationToken cancellationToken = CancellationToken;

        //create client and server socket
        const SocketType socketType = SocketType.Stream;
        const ProtocolType protocolType = ProtocolType.Tcp;

        IPAddress localHost = (await Dns.GetHostEntryAsync("localhost", cancellationToken)).AddressList[0];
        AddressFamily addressFamily = localHost.AddressFamily;

        var listenSocket = new Socket(addressFamily, socketType, protocolType);
        listenSocket.Bind(new IPEndPoint(localHost, 0));
        Assert.IsNotNull(listenSocket);
        Assert.IsNotNull(listenSocket.LocalEndPoint);

        var listenEndPoint = (IPEndPoint)listenSocket.LocalEndPoint;
        ILogger<IncrementerTest> logger = LoggerFactory.CreateLogger<IncrementerTest>();
        logger.LogInformation("using port {Port}", listenEndPoint.Port);

        listenSocket.Listen(1);
        ValueTask<Socket> acceptClientTask = listenSocket.AcceptAsync(cancellationToken);

        Socket clientSocket = new(addressFamily, socketType, protocolType);
        await clientSocket.ConnectAsync(listenEndPoint, cancellationToken);

        Socket serverSocket = await acceptClientTask;

        //create client and server
        var serverEndPoint = new IncrementerServerEndPoint(new Messenger(new RpcSocket(serverSocket)), LoggerFactory);
        var clientEndPoint = new IncrementerClientEndPoint(new Messenger(new RpcSocket(clientSocket)), LoggerFactory);

        Assert.IsTrue(serverEndPoint.State == State.IdleInbound);
        Assert.IsTrue(clientEndPoint.State == State.IdleOutbound);

        var incrementerServerImplementation = new IncrementerServerImplementation();
        Task<Messenger.ListenReturnCode> serverTask = Task.Factory.StartNew
        (
            () => serverEndPoint.Listen(incrementerServerImplementation),
            cancellationToken,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default
        );

        int value = 5;
        for (int i = 0; i < 10; i++)
        {
            value = await clientEndPoint.IncrementAsync(value, cancellationToken);
        }

        Assert.AreEqual(15, value);

        clientEndPoint.Dispose();
        Assert.IsTrue(clientEndPoint.State == State.Disposed);

        Messenger.ListenReturnCode listenReturnCode = await serverTask;

        Assert.IsTrue(listenReturnCode == Messenger.ListenReturnCode.ConnectionClosed);
        Assert.IsTrue(serverEndPoint.State == State.Disposed);
    }
}