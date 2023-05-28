using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Configuration;
using MsbRpc.Test.Base.Generic;
using MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;
using MsbRpc.Test.Utility;

namespace MsbRpc.Test.Implementations.DateTimeEcho.Tests;

[TestClass]
public class DateTimeEchoTest : ServerTest<DateTimeEchoTest, DateTimeEchoServer, DateTimeEchoServerEndPoint, DateTimeEchoClientEndPoint, DateTimeEchoProcedure,
    IDateTimeEcho>
{
    [TestMethod]
    public void Listens()
    {
        TestListens();
    }

    [TestMethod]
    public async Task ConnectsAndDisconnects()
    {
        await TestConnectsAndDisconnects();
    }

    [TestMethod]
    public async Task ConnectsAndDisconnectsTwoClients()
    {
        await TestConnectsAndDisconnectsTwoClients();
    }

    [TestMethod]
    public async Task CanDisposeServerPrematurely()
    {
        await TestCanDisposeServerPrematurely();
    }

    [TestMethod]
    public async Task ServerRespondsWithDifferentDateTime()
    {
        using DateTimeEchoServer server = StartServer();
        using DateTimeEchoClientEndPoint client = await ConnectClient(server);
        DateTime clientDateTime = DateTime.Now;
        DateTime serverDateTime = await client.GetDateTimeAsync(clientDateTime);
        Assert.AreNotEqual(default, serverDateTime);
        TestContext.Write
        (
            "Client DateTime is {0} ({1}), server DateTime is {2} ({3})",
            clientDateTime,
            clientDateTime.Ticks,
            serverDateTime,
            serverDateTime.Ticks
        );
        Assert.AreNotEqual(clientDateTime, serverDateTime);
    }

    [TestMethod]
    public async Task ServerHasNotInitiallyRunToCompletion()
    {
        using DateTimeEchoServer server = StartServer();
        using DateTimeEchoClientEndPoint client = await ConnectClient(server);
        Assert.IsFalse(client.RanToCompletion);
    }

    [TestMethod]
    public async Task ServerRunsToCompletionUponSingleRpcInvocation()
    {
        using DateTimeEchoServer server = StartServer();
        using DateTimeEchoClientEndPoint client = await ConnectClient(server);
        await client.GetDateTimeAsync(DateTime.Now);
        Assert.IsTrue(client.RanToCompletion);
    }

    protected override DateTimeEchoServer CreateServer() => new(TestUtility.LoggerFactory);

    protected override async ValueTask<DateTimeEchoClientEndPoint> ConnectClient(IPEndPoint endPoint, OutboundEndPointConfiguration configuration)
        => await DateTimeEchoClientEndPoint.ConnectAsync(endPoint, configuration);
}