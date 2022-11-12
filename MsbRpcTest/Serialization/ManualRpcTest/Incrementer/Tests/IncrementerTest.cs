using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Implementation.Generated;
using MsbRpcTest.Serialization.Network;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
    [TestMethod]
    public async Task IncrementOnce()
    {
        Connection connection = await Connection.ConnectAsync(CancellationToken);
        var server = new IncrementerServerEndPoint(connection.ServerSocket, new Implementation.Incrementer());
    }
}