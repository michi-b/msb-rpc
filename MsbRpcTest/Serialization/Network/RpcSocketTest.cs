using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
public class RpcSocketTest : Test
{
    [TestMethod]
    public async Task SendsInt32()
    {
        var endpoint = NetworkUtility.GetLocalEndPoint(19271);
    }
}