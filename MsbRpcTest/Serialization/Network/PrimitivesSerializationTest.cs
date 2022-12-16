﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Sockets;
using MsbRpcTest.Serialization.Network.Utility;
using MsbRpcTest.Serialization.Network.Utility.Listeners;

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
        CancellationToken cancellationToken = CancellationToken;

        using LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);

        using Task<byte[]> listenTask = ByteArrayListener.ListenAsync(connection.Server, cancellationToken);

        byte[] bytes = new byte[NetworkUtility.DefaultBufferSize];

        const Int32 value = 531234;

        bytes.WriteInt(value);

        await connection.Client.SendAsync(new ArraySegment<byte>(bytes, 0, sizeof(Int32)), cancellationToken);
        connection.Client.Dispose();

        byte[] receivedBytes = await listenTask;

        Int32 result = receivedBytes.ReadInt();

        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public async Task PreservesAllPrimitives()
    {
        CancellationToken cancellationToken = CancellationToken;

        using LocalConnection connection = await LocalConnection.ConnectAsync(cancellationToken);

        using Task<byte[]> listenTask = ByteArrayListener.ListenAsync(connection.Server, cancellationToken);

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

        buffer.WriteBool(trueBooleanValue, offset);
        offset += PrimitiveSerializer.BooleanSize;

        buffer.WriteBool(falseBooleanValue, offset);
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

        buffer.WriteShort(int16Value, offset);
        offset += PrimitiveSerializer.Int16Size;

        buffer.WriteInt(int32Value, offset);
        offset += PrimitiveSerializer.Int32Size;

        buffer.WriteLong(int64Value, offset);
        offset += PrimitiveSerializer.Int64Size;

        buffer.WriteSbyte(sByteValue, offset);
        offset += PrimitiveSerializer.SByteSize;

        buffer.WriteFloat(singleValue, offset);
        offset += PrimitiveSerializer.SingleSize;

        buffer.WriteUshort(uint16Value, offset);
        offset += PrimitiveSerializer.UInt16Size;

        buffer.WriteUint(uint32Value, offset);
        offset += PrimitiveSerializer.UInt32Size;

        buffer.WriteUlong(uint64Value, offset);
        offset += PrimitiveSerializer.UInt64Size;

        int byteCount = offset;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.IsTrue(byteCount <= buffer.Length);

        using (RpcSocket clientSocket = connection.Client)
        {
            await clientSocket.SendAsync(new ArraySegment<byte>(buffer, 0, byteCount), cancellationToken);
            clientSocket.Dispose();
        }

        byte[] bytes = await listenTask;

        Assert.IsTrue(bytes.Length == byteCount);

        offset = 0;

        bool trueBooleanResult = bytes.ReadBool(offset);
        offset += PrimitiveSerializer.BooleanSize;

        bool falseBooleanResult = bytes.ReadBool(offset);
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

        Int16 int16Result = bytes.ReadShort(offset);
        offset += PrimitiveSerializer.Int16Size;

        Int32 int32Result = bytes.ReadInt(offset);
        offset += PrimitiveSerializer.Int32Size;

        Int64 int64Result = bytes.ReadLong(offset);
        offset += PrimitiveSerializer.Int64Size;

        SByte sByteResult = bytes.ReadSbyte(offset);
        offset += PrimitiveSerializer.SByteSize;

        Single singleResult = bytes.ReadFloat(offset);
        offset += PrimitiveSerializer.SingleSize;

        UInt16 uint16Result = bytes.ReadUshort(offset);
        offset += PrimitiveSerializer.UInt16Size;

        UInt32 uint32Result = bytes.ReadUint(offset);
        offset += PrimitiveSerializer.UInt32Size;

        UInt64 uint64Result = bytes.ReadUlong(offset);
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