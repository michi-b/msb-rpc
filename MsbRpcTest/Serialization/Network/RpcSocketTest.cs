using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Sockets;
using MsbRpc.Sockets.Exceptions;
using MsbRpcTest.Serialization.Network.Utility;

namespace MsbRpcTest.Serialization.Network;

[TestClass]
public class RpcSocketTest : Test
{
    [TestMethod]
    public async Task DisconnectedSocketThrowsException()
    {
        CancellationToken cancellationToken = CancellationToken;
        IPAddress localhost = (await Dns.GetHostAddressesAsync("localhost", cancellationToken))[0];
        var socket = new Socket(localhost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        Assert.ThrowsException<InvalidRpcSocketConstructorSocketException>(() => new RpcSocket(socket));
    }

    [TestMethod]
    public async Task UnspecifiedSocketProtocolTypeThrowsException()
    {
        const ProtocolType protocolType = ProtocolType.Unspecified;

        CancellationToken cancellationToken = CancellationToken;

        IPAddress localHost = (await Dns.GetHostEntryAsync("localhost", cancellationToken)).AddressList[0];
        
        EndPoint serverEndPoint = new IPEndPoint(localHost, 0);
        
        var listenSocket = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, protocolType);
        listenSocket.Bind(serverEndPoint);
        listenSocket.Listen(1);
        ValueTask<Socket> accept = listenSocket.AcceptAsync(cancellationToken);

        var senderSocket = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, protocolType);
        await senderSocket.ConnectAsync(serverEndPoint, cancellationToken);

        //establish connection
        Socket receiverSocket = await accept;

        Assert.ThrowsException<InvalidRpcSocketConstructorSocketException>(() => new RpcSocket(senderSocket));
        Assert.ThrowsException<InvalidRpcSocketConstructorSocketException>(() => new RpcSocket(receiverSocket));
    }

    [TestMethod]
    public async Task CorrectSocketConfigurationAllowsRpcSocketCreation()
    {
        const SocketType socketType = SocketType.Stream;

        CancellationToken cancellationToken = CancellationToken;

        IPAddress localHost = (await Dns.GetHostEntryAsync("localhost", cancellationToken)).AddressList[0];
        
        EndPoint serverEndPoint = new IPEndPoint(localHost, 0);
        
        var listenSocket = new Socket(serverEndPoint.AddressFamily, socketType, ProtocolType.Tcp);
        listenSocket.Bind(serverEndPoint);
        listenSocket.Listen(1);
        ValueTask<Socket> accept = listenSocket.AcceptAsync(cancellationToken);

        var senderSocket = new Socket(serverEndPoint.AddressFamily, socketType, ProtocolType.Tcp);
        await senderSocket.ConnectAsync(serverEndPoint, cancellationToken);

        //establish connection
        Socket receiverSocket = await accept;

        // ReSharper disable twice ObjectCreationAsStatement
        // this only tests that the constructor does not throw an exception
        new RpcSocket(senderSocket);
        new RpcSocket(receiverSocket);
    }

    [TestMethod]
    public async Task TransfersSingleByte()
    {
        CancellationToken cancellationToken = CancellationToken;

        using LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);
        RpcSocket sender = connection.Client;
        RpcSocket receiver = connection.Server;

        const byte value = 123;
        var sendSegment = new ArraySegment<byte>(new[] { value });
        await sender.SendAsync(sendSegment, cancellationToken);

        var receiveSegment = new ArraySegment<byte>(new byte[] { 0 });
        bool success = await receiver.ReceiveAllAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);

        byte receivedValue = receiveSegment[0];

        LogReceived(receivedValue.ToString());

        Assert.AreEqual(value, receivedValue);
    }

    [TestMethod]
    public async Task TransfersMultiByteSegment()
    {
        CancellationToken cancellationToken = CancellationToken;

        using LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);
        RpcSocket sender = connection.Client;
        RpcSocket receiver = connection.Server;
        
        const byte value0 = 123;
        const byte value1 = 234;
        const byte value2 = 98;

        var sendSegment = new ArraySegment<byte>(new[] { value0, value1, value2 });
        await sender.SendAsync(sendSegment, cancellationToken);

        ArraySegment<byte> receiveSegment = BufferUtility.Create(3);
        bool success = await receiver.ReceiveAllAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);

        byte receivedValue0 = receiveSegment[0];
        byte receivedValue1 = receiveSegment[1];
        byte receivedValue2 = receiveSegment[2];

        LogReceived(receiveSegment.CreateContentString());

        Assert.AreEqual(value0, receivedValue0);
        Assert.AreEqual(value1, receivedValue1);
        Assert.AreEqual(value2, receivedValue2);
    }

    [TestMethod]
    public async Task TransfersMultipleSingleBytes()
    {
        CancellationToken cancellationToken = CancellationToken;

        using LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);
        RpcSocket sender = connection.Client;
        RpcSocket receiver = connection.Server;

        const byte value0 = 64;
        const byte value1 = 0;
        const byte value2 = 123;

        var sendSegment = new ArraySegment<byte>(new[] { value0 });
        await sender.SendAsync(sendSegment, cancellationToken);
        sendSegment.Array![0] = value1;
        await sender.SendAsync(sendSegment, cancellationToken);
        sendSegment.Array![0] = value2;
        await sender.SendAsync(sendSegment, cancellationToken);

        ArraySegment<byte> receiveSegment = BufferUtility.Create(3);
        bool success = await receiver.ReceiveAllAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);

        byte receivedValue0 = receiveSegment[0];
        byte receivedValue1 = receiveSegment[1];
        byte receivedValue2 = receiveSegment[2];

        LogReceived(receiveSegment.CreateContentString());

        Assert.AreEqual(value0, receivedValue0);
        Assert.AreEqual(value1, receivedValue1);
        Assert.AreEqual(value2, receivedValue2);
    }

    [TestMethod]
    public async Task TransfersMultipleSingleBytesReceivingSeparately()
    {
        CancellationToken cancellationToken = CancellationToken;

        using LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);
        RpcSocket sender = connection.Client;
        RpcSocket receiver = connection.Server;

        const byte value0 = 3;
        const byte value1 = 1;
        const byte value2 = 255;

        var sendSegment = new ArraySegment<byte>(new[] { value0 });
        await sender.SendAsync(sendSegment, cancellationToken);
        sendSegment.Array![0] = value1;
        await sender.SendAsync(sendSegment, cancellationToken);
        sendSegment.Array![0] = value2;
        await sender.SendAsync(sendSegment, cancellationToken);

        ArraySegment<byte> receiveSegment = BufferUtility.Create(1);

        bool success = await receiver.ReceiveAllAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);
        byte receivedValue0 = receiveSegment[0];

        success = await receiver.ReceiveAllAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);
        byte receivedValue1 = receiveSegment[0];

        success = await receiver.ReceiveAllAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);
        byte receivedValue2 = receiveSegment[0];

        LogReceived(ByteArrayUtility.ToString(receivedValue0, receivedValue1, receivedValue2));

        Assert.AreEqual(value0, receivedValue0);
        Assert.AreEqual(value1, receivedValue1);
        Assert.AreEqual(value2, receivedValue2);
    }

    [TestMethod]
    public async Task TransfersMultipleDifferentValues()
    {
        CancellationToken cancellationToken = CancellationToken;
        
        using LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);
        RpcSocket sender = connection.Client;
        RpcSocket receiver = connection.Server;

        const int intValue = 1235234134;
        const decimal decimalValue = 3124312323.123489087m;
        const char charValue = 'a';
        const bool booleanValue = true;
        const sbyte sByteValue = -122;

        const int arraySize = PrimitiveSerializer.Int32Size
                              + PrimitiveSerializer.DecimalSize
                              + PrimitiveSerializer.CharSize
                              + PrimitiveSerializer.BooleanSize
                              + PrimitiveSerializer.SByteSize;

        ArraySegment<byte> sendBytes = BufferUtility.Create(arraySize);

        BufferWriter sendBytesWriter = new(sendBytes);

        sendBytesWriter.Write(intValue);
        sendBytesWriter.Write(decimalValue);
        sendBytesWriter.Write(charValue);
        sendBytesWriter.Write(booleanValue);
        sendBytesWriter.Write(sByteValue);

        await sender.SendAsync(sendBytes, cancellationToken);

        ArraySegment<byte> receiveBytes = BufferUtility.Create(arraySize);

        bool success = await receiver.ReceiveAllAsync(receiveBytes, cancellationToken);

        Assert.IsTrue(success);

        LogReceived(receiveBytes.CreateContentString());

        BufferReader receiveBytesReader = new(receiveBytes);

        int receivedIntValue = receiveBytesReader.ReadInt32();
        decimal receivedDecimalValue = receiveBytesReader.ReadDecimal();
        char receivedCharValue = receiveBytesReader.ReadChar();
        bool receivedBooleanValue = receiveBytesReader.ReadBoolean();
        sbyte receivedSByteValue = receiveBytesReader.ReadSByte();

        Assert.AreEqual(intValue, receivedIntValue);
        Assert.AreEqual(decimalValue, receivedDecimalValue);
        Assert.AreEqual(charValue, receivedCharValue);
        Assert.AreEqual(booleanValue, receivedBooleanValue);
        Assert.AreEqual(sByteValue, receivedSByteValue);
    }

    private static void LogReceived(string byteString)
    {
        Console.WriteLine($"Received: {byteString}");
    }
}