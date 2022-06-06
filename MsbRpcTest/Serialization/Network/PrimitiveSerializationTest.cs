using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class PrimitiveSerializationTest : Test
{
    private CancellationTokenSource _cancellationTokenSource = null!;
    private PrimitiveSerializer _serializer = null!;
    private Task<byte[]> _serverTask = null!;

    [TestInitialize]
    public void InitializeTest()
    {
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken);
        _serializer = new PrimitiveSerializer();
        _serverTask = NetworkUtility.ReceiveBufferAsync(_cancellationTokenSource.Token);
    }


    [TestCleanup]
    public void CleanupTest()
    {
        _cancellationTokenSource.Cancel();

        _serverTask.Wait(TimeSpan.FromSeconds(1));
        if (!_serverTask.IsCompleted)
        {
            throw new InvalidOperationException();
        }
        _serverTask.Dispose();

        _serverTask = null!;
        _serializer = null!;
        _cancellationTokenSource = null!;
    }

    [TestMethod]
    public async Task PreservesInt32()
    {
        const int value = 531234;
        byte[] buffer = new byte[NetworkUtility.DefaultBufferSize];
        _serializer.WriteInt32(value, buffer);

        var clientSocket = new Socket(NetworkUtility.LocalHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        await clientSocket.ConnectAsync(NetworkUtility.LocalHost);

        await clientSocket.SendAsync(new ArraySegment<byte>(buffer, 0, sizeof(int)), SocketFlags.None);
        clientSocket.Close();

        byte[] serverReceivedBytes = await _serverTask;

        int result = PrimitiveSerializer.ReadInt32(serverReceivedBytes);

        Assert.AreEqual(value, result);
    }
}