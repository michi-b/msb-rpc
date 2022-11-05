using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.RpcSocket;
using MsbRpc.RpcSocket.Exceptions;
using MsbRpc.Serialization;
using MsbRpc.Serialization.ByteArraySegment;
using MsbRpc.Serialization.Primitives;
using Serilog;
using Serilog.Core;

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

        EndPoint serverEndPoint = NetworkUtility.GetLocalEndPoint();
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

        EndPoint serverEndPoint = NetworkUtility.GetLocalEndPoint();
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

        (RpcSocket sender, RpcSocket receiver) = await ConnectAsync(cancellationToken);

        const byte value = 123;
        var sendSegment = new ArraySegment<byte>(new[] { value });
        await sender.SendAsync(sendSegment, cancellationToken);

        var receiveSegment = new ArraySegment<byte>(new byte[] { 0 });
        bool success = await receiver.ReceiveAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);

        byte receivedValue = receiveSegment[0];

        LogReceived(receivedValue.ToString());

        Assert.AreEqual(value, receivedValue);
    }

    [TestMethod]
    public async Task TransfersMultiByteSegment()
    {
        CancellationToken cancellationToken = CancellationToken;

        (RpcSocket sender, RpcSocket receiver) = await ConnectAsync(cancellationToken);

        const byte value0 = 123;
        const byte value1 = 234;
        const byte value2 = 98;

        var sendSegment = new ArraySegment<byte>(new[] { value0, value1, value2 });
        await sender.SendAsync(sendSegment, cancellationToken);

        ArraySegment<byte> receiveSegment = Memory.Create(3);
        bool success = await receiver.ReceiveAsync(receiveSegment, cancellationToken);
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

        (RpcSocket sender, RpcSocket receiver) = await ConnectAsync(cancellationToken);

        const byte value0 = 64;
        const byte value1 = 0;
        const byte value2 = 123;

        var sendSegment = new ArraySegment<byte>(new[] { value0 });
        await sender.SendAsync(sendSegment, cancellationToken);
        sendSegment.Array![0] = value1;
        await sender.SendAsync(sendSegment, cancellationToken);
        sendSegment.Array![0] = value2;
        await sender.SendAsync(sendSegment, cancellationToken);

        ArraySegment<byte> receiveSegment = Memory.Create(3);
        bool success = await receiver.ReceiveAsync(receiveSegment, cancellationToken);
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

        (RpcSocket sender, RpcSocket receiver) = await ConnectAsync(cancellationToken);

        const byte value0 = 3;
        const byte value1 = 1;
        const byte value2 = 255;

        var sendSegment = new ArraySegment<byte>(new[] { value0 });
        await sender.SendAsync(sendSegment, cancellationToken);
        sendSegment.Array![0] = value1;
        await sender.SendAsync(sendSegment, cancellationToken);
        sendSegment.Array![0] = value2;
        await sender.SendAsync(sendSegment, cancellationToken);

        ArraySegment<byte> receiveSegment = Memory.Create(1);

        bool success = await receiver.ReceiveAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);
        byte receivedValue0 = receiveSegment[0];

        success = await receiver.ReceiveAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);
        byte receivedValue1 = receiveSegment[0];

        success = await receiver.ReceiveAsync(receiveSegment, cancellationToken);
        Assert.IsTrue(success);
        byte receivedValue2 = receiveSegment[0];

        LogReceived(Memory.CreateByteString(receivedValue0, receivedValue1, receivedValue2));

        Assert.AreEqual(value0, receivedValue0);
        Assert.AreEqual(value1, receivedValue1);
        Assert.AreEqual(value2, receivedValue2);
    }

    [TestMethod]
    public async Task TransfersMultipleDifferentValues()
    {
        CancellationToken cancellationToken = CancellationToken;
        (RpcSocket sender, RpcSocket receiver) = await ConnectAsync(cancellationToken);

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

        ArraySegment<byte> sendBytes = Memory.Create(arraySize);

        SequentialWriter sendBytesWriter = new(sendBytes);

        sendBytesWriter.Write(intValue);
        sendBytesWriter.Write(decimalValue);
        sendBytesWriter.Write(charValue);
        sendBytesWriter.Write(booleanValue);
        sendBytesWriter.Write(sByteValue);

        await sender.SendAsync(sendBytes, cancellationToken);

        ArraySegment<byte> receiveBytes = Memory.Create(arraySize);

        bool success = await receiver.ReceiveAsync(receiveBytes, cancellationToken);

        Assert.IsTrue(success);

        LogReceived(receiveBytes.CreateContentString());

        SequentialReader receiveBytesReader = new(receiveBytes);

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

    private static async Task<(RpcSocket sender, RpcSocket receiver)> ConnectAsync(CancellationToken cancellationToken)
    {
        var connection = await TransferConnection.CreateAsync(cancellationToken);
        RpcSocket sender = connection.Sender;
        RpcSocket receiver = connection.Receiver;
        return (sender, receiver);
    }

    private static void LogReceived(string byteString)
    {
        Console.WriteLine($"Received: {byteString}");
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        Logger serilog = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        ILoggerFactory loggerFactory = new LoggerFactory().AddSerilog(serilog);
        return loggerFactory;
    }

    private struct TransferConnection
    {
        public RpcSocket Sender { get; }
        public RpcSocket Receiver { get; }

        private TransferConnection(RpcSocket sender, RpcSocket receiver)
        {
            Sender = sender;
            Receiver = receiver;
        }

        public static async Task<TransferConnection> CreateAsync(CancellationToken cancellationToken)
        {
            ILoggerFactory loggerFactory = CreateLoggerFactory();
            Connection connection = await Connection.ConnectAsync(cancellationToken);
            return new TransferConnection
            (
                new RpcSocket(connection.ClientSocket),
                new RpcSocket(connection.ServerSocket)
            );
        }
    }
}