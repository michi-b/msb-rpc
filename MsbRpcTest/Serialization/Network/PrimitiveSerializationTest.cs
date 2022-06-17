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
public class PrimitiveSerializationTest : Test
{
    [TestMethod]
    public async Task PreservesInt32()
    {
        IPEndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<byte[]> serverTask = NetworkUtility.ReceiveBufferAsync(ep, CancellationToken);
        byte[] buffer = new byte[NetworkUtility.DefaultBufferSize];
        var primitiveSerializer = new PrimitiveSerializer();

        const Int32 value = 531234;

        primitiveSerializer.WriteInt32(value, buffer);

        Socket clientSocket = NetworkUtility.CreateSocket();
        await clientSocket.ConnectAsync(ep, CancellationToken);

        await clientSocket.SendAsync(new ArraySegment<byte>(buffer, 0, sizeof(Int32)), SocketFlags.None);
        clientSocket.Close();

        byte[] serverReceivedBytes = await serverTask;

        Int32 result = PrimitiveSerializer.ReadInt32(serverReceivedBytes);

        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public async Task PreservesAllPrimitives()
    {
        IPEndPoint ep = NetworkUtility.GetLocalEndPoint();
        using Task<byte[]> serverTask = NetworkUtility.ReceiveBufferAsync(ep, CancellationToken);
        byte[] buffer = new byte[NetworkUtility.DefaultBufferSize];
        var primitiveSerializer = new PrimitiveSerializer();

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

        PrimitiveSerializer.WriteBoolean(trueBooleanValue, buffer, offset);
        offset += PrimitiveSerializer.BooleanSize;

        PrimitiveSerializer.WriteBoolean(falseBooleanValue, buffer, offset);
        offset += PrimitiveSerializer.BooleanSize;

        buffer[offset++] = byteValue;

        primitiveSerializer.WriteChar(charYValue, buffer, offset);
        offset += PrimitiveSerializer.CharSize;

        primitiveSerializer.WriteChar(charAeValue, buffer, offset);
        offset += PrimitiveSerializer.CharSize;

        primitiveSerializer.WriteDecimal(decimalValue, buffer, offset);
        offset += PrimitiveSerializer.DecimalSize;

        primitiveSerializer.WriteDouble(doubleValue, buffer, offset);
        offset += PrimitiveSerializer.DoubleSize;

        primitiveSerializer.WriteInt16(int16Value, buffer, offset);
        offset += PrimitiveSerializer.Int16Size;

        primitiveSerializer.WriteInt32(int32Value, buffer, offset);
        offset += PrimitiveSerializer.Int32Size;

        primitiveSerializer.WriteInt64(int64Value, buffer, offset);
        offset += PrimitiveSerializer.Int64Size;

        PrimitiveSerializer.WriteSByte(sByteValue, buffer, offset);
        offset += PrimitiveSerializer.SByteSize;

        primitiveSerializer.WriteSingle(singleValue, buffer, offset);
        offset += PrimitiveSerializer.SingleSize;

        primitiveSerializer.WriteUInt16(uint16Value, buffer, offset);
        offset += PrimitiveSerializer.UInt16Size;

        primitiveSerializer.WriteUInt32(uint32Value, buffer, offset);
        offset += PrimitiveSerializer.UInt32Size;

        primitiveSerializer.WriteUInt64(uint64Value, buffer, offset);
        offset += PrimitiveSerializer.UInt64Size;

        int byteCount = offset;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.IsTrue(byteCount <= buffer.Length);

        Socket clientSocket = NetworkUtility.CreateSocket();
        await clientSocket.ConnectAsync(ep);
        await clientSocket.SendAsync(new ArraySegment<byte>(buffer, 0, byteCount), SocketFlags.None);
        clientSocket.Close();

        byte[] result = await serverTask;

        Assert.IsTrue(result.Length >= byteCount);

        offset = 0;

        bool trueBooleanResult = PrimitiveSerializer.ReadBoolean(result, offset);
        offset += PrimitiveSerializer.BooleanSize;

        bool falseBooleanResult = PrimitiveSerializer.ReadBoolean(result, offset);
        offset += PrimitiveSerializer.BooleanSize;

        byte byteResult = result[offset++];

        Char charYResult = PrimitiveSerializer.ReadChar(result, offset);
        offset += PrimitiveSerializer.CharSize;

        Char charAeResult = PrimitiveSerializer.ReadChar(result, offset);
        offset += PrimitiveSerializer.CharSize;

        Decimal decimalResult = primitiveSerializer.ReadDecimal(result, offset);
        offset += PrimitiveSerializer.DecimalSize;

        Double doubleResult = PrimitiveSerializer.ReadDouble(result, offset);
        offset += PrimitiveSerializer.DoubleSize;

        Int16 int16Result = PrimitiveSerializer.ReadInt16(result, offset);
        offset += PrimitiveSerializer.Int16Size;

        Int32 int32Result = PrimitiveSerializer.ReadInt32(result, offset);
        offset += PrimitiveSerializer.Int32Size;

        Int64 int64Result = PrimitiveSerializer.ReadInt64(result, offset);
        offset += PrimitiveSerializer.Int64Size;

        SByte sByteResult = PrimitiveSerializer.ReadSByte(result, offset);
        offset += PrimitiveSerializer.SByteSize;

        Single singleResult = PrimitiveSerializer.ReadSingle(result, offset);
        offset += PrimitiveSerializer.SingleSize;

        UInt16 uint16Result = PrimitiveSerializer.ReadUInt16(result, offset);
        offset += PrimitiveSerializer.UInt16Size;

        UInt32 uint32Result = PrimitiveSerializer.ReadUInt32(result, offset);
        offset += PrimitiveSerializer.UInt32Size;

        UInt64 uint64Result = PrimitiveSerializer.ReadUInt64(result, offset);
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