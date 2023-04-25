using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Configuration;
using MsbRpc.Exceptions;
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

    protected override DateTimeEchoServer CreateServer(RpcExceptionTransmissionOptions _ = RpcExceptionTransmissionOptions.None) => new(TestUtility.LoggerFactory);

    protected override async ValueTask<DateTimeEchoClientEndPoint> ConnectClient(IPEndPoint endPoint, OutboundEndPointConfiguration configuration)
        => await DateTimeEchoClientEndPoint.ConnectAsync(endPoint, configuration);
}