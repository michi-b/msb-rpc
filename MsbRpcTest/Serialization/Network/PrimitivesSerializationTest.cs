using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Network;

#pragma warning disable IDE0079 // Remove unnecessary suppression
[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class PrimitivesSerializationTest : Test
{
    [TestMethod]
    public async Task PreservesInt32()
    {
        EndPoint ep = NetworkUtility.GetLocalEndPoint();

        using Task<byte[]> listenTask
            = NetworkUtility.ReceiveBytesAsync(ep, CancellationToken);

        byte[] bytes = new byte[NetworkUtility.DefaultBufferSize];

        const Int32 value = 531234;

        bytes.WriteInt32(value);

        Socket clientSocket = NetworkUtility.CreateSocket();
        await clientSocket.ConnectAsync(ep, CancellationToken);

        Assert.IsTrue(clientSocket.Connected);

        await clientSocket.SendAsync(new ArraySegment<byte>(bytes, 0, sizeof(Int32)), SocketFlags.None);
        clientSocket.Close();

        byte[] receivedBytes = await listenTask;

        Int32 result = receivedBytes.ReadInt32();

        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public async Task PreservesAllPrimitives()
    {
        EndPoint ep = NetworkUtility.GetLocalEndPoint();

        using Task<byte[]> listenTask = NetworkUtility.ReceiveBytesAsync(ep, CancellationToken);

        byte[] buffer = new byte[NetworkUtility.DefaultBufferSize];

        const Boolean trueBooleanValue = true;
        const Boolean falseBooleanValue = false;
        const Byte byteValue = 69;
        const Char charYValue = 'y';
        const Char charAeValue = 'ä';
        Decimal decimalValue = new(123, 123, 123, true, 28);
        const Double doubleValue = 123523413214.44512312445312;
        const Int16 int16Value = -23213;
        const Int32 int32Value = 1234122134;
        const Int64 int64Value = -8790678128934128934;
        const SByte sByteValue = -123;
        const Single singleValue = -2412341234.1234f;
        const UInt16 uint16Value = 32143;
        const UInt32 uint32Value = 3245324423;
        const UInt64 uint64Value = UInt64.MaxValue;

        int offset = 0;

        buffer.WriteBoolean(trueBooleanValue, offset);
        offset += PrimitiveSerializer.BooleanSize;

        buffer.WriteBoolean(falseBooleanValue, offset);
        offset += PrimitiveSerializer.BooleanSize;

        buffer[offset++] = byteValue;

        buffer.WriteChar(charYValue, offset);
        offset += PrimitiveSerializer.CharSize;

        buffer.WriteChar(charAeValue, offset);
        offset += PrimitiveSerializer.CharSize;

        buffer.WriteDecimal(decimalValue, offset);
        offset += PrimitiveSerializer.DecimalSize;

        buffer.WriteDouble(doubleValue, offset);
        offset += PrimitiveSerializer.DoubleSize;

        buffer.WriteInt16(int16Value, offset);
        offset += PrimitiveSerializer.Int16Size;

        buffer.WriteInt32(int32Value, offset);
        offset += PrimitiveSerializer.Int32Size;

        buffer.WriteInt64(int64Value, offset);
        offset += PrimitiveSerializer.Int64Size;

        buffer.WriteSByte(sByteValue, offset);
        offset += PrimitiveSerializer.SByteSize;

        buffer.WriteSingle(singleValue, offset);
        offset += PrimitiveSerializer.SingleSize;

        buffer.WriteUInt16(uint16Value, offset);
        offset += PrimitiveSerializer.UInt16Size;

        buffer.WriteUInt32(uint32Value, offset);
        offset += PrimitiveSerializer.UInt32Size;

        buffer.WriteUInt64(uint64Value, offset);
        offset += PrimitiveSerializer.UInt64Size;

        int byteCount = offset;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.IsTrue(byteCount <= buffer.Length);

        Socket clientSocket = NetworkUtility.CreateSocket();
        await clientSocket.ConnectAsync(ep);
        await clientSocket.SendAsync(new ArraySegment<byte>(buffer, 0, byteCount), SocketFlags.None);
        clientSocket.Close();

        byte[] bytes = await listenTask;

        Assert.IsTrue(bytes.Length == byteCount);

        offset = 0;

        bool trueBooleanResult = bytes.ReadBoolean(offset);
        offset += PrimitiveSerializer.BooleanSize;

        bool falseBooleanResult = bytes.ReadBoolean(offset);
        offset += PrimitiveSerializer.BooleanSize;

        byte byteResult = bytes[offset++];

        Char charYResult = bytes.ReadChar(offset);
        offset += PrimitiveSerializer.CharSize;

        Char charAeResult = bytes.ReadChar(offset);
        offset += PrimitiveSerializer.CharSize;

        Decimal decimalResult = bytes.ReadDecimal(offset);
        offset += PrimitiveSerializer.DecimalSize;

        Double doubleResult = bytes.ReadDouble(offset);
        offset += PrimitiveSerializer.DoubleSize;

        Int16 int16Result = bytes.ReadInt16(offset);
        offset += PrimitiveSerializer.Int16Size;

        Int32 int32Result = bytes.ReadInt32(offset);
        offset += PrimitiveSerializer.Int32Size;

        Int64 int64Result = bytes.ReadInt64(offset);
        offset += PrimitiveSerializer.Int64Size;

        SByte sByteResult = bytes.ReadSByte(offset);
        offset += PrimitiveSerializer.SByteSize;

        Single singleResult = bytes.ReadSingle(offset);
        offset += PrimitiveSerializer.SingleSize;

        UInt16 uint16Result = bytes.ReadUInt16(offset);
        offset += PrimitiveSerializer.UInt16Size;

        UInt32 uint32Result = bytes.ReadUInt32(offset);
        offset += PrimitiveSerializer.UInt32Size;

        UInt64 uint64Result = bytes.ReadUInt64(offset);
        offset += PrimitiveSerializer.UInt64Size;

        Assert.AreEqual(byteCount, offset);

        Assert.AreEqual(trueBooleanValue, trueBooleanResult);
        Assert.AreEqual(falseBooleanValue, falseBooleanResult);
        Assert.AreEqual(byteValue, byteResult);
        Assert.AreEqual(charYValue, charYResult);
        Assert.AreEqual(charAeValue, charAeResult);
        Assert.AreEqual(decimalValue, decimalResult);
        Assert.AreEqual(doubleValue, doubleResult);
        Assert.AreEqual(int16Value, int16Result);
        Assert.AreEqual(int32Value, int32Result);
        Assert.AreEqual(int64Value, int64Result);
        Assert.AreEqual(sByteValue, sByteResult);
        Assert.AreEqual(singleValue, singleResult);
        Assert.AreEqual(uint16Value, uint16Result);
        Assert.AreEqual(uint32Value, uint32Result);
        Assert.AreEqual(uint64Value, uint64Result);
    }
}