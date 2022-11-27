using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Messaging;
using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;
using MsbRpcTest.Serialization.Network.Utility;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Tests;

[TestClass]
public class IncrementerTest : Test
{
    [TestMethod]
    public async Task CanServerListen()
    {
        CancellationToken cancellationToken = CancellationToken;
        (Messenger clientMessenger, Messenger serverMessenger) = (await Connection.ConnectAsync(cancellationToken)).AsMessengers;
        var serverImplementation = new Implementation.Incrementer();
        var server = new IncrementerServer(serverMessenger, serverImplementation);
        
        Task<Messenger.ListenReturnCode> serverListenTask = server.ListenAsync(cancellationToken);
        
        clientMessenger.Dispose();
        
        await serverListenTask;
        
        server.Dispose();
    }
}