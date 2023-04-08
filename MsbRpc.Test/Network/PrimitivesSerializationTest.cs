using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;
using MsbRpc.Sockets;
using MsbRpc.Test.Network.Utility;
using MsbRpc.Test.Network.Utility.Listeners;

namespace MsbRpc.Test.Network;

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

        using Task<byte[]> listenTask = ByteArrayListener.Listen(connection.Server);

        byte[] bytes = new byte[NetworkUtility.DefaultBufferSize];

        const Int32 value = 531234;

        bytes.WriteInt(value);

        await connection.Client.SendAsync(new ArraySegment<byte>(bytes, 0, sizeof(Int32)));
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

        using Task<byte[]> listenTask = ByteArrayListener.Listen(connection.Server);

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

        //bool?
        bool? nullBooleanValue = null;
        bool? nullableTrueBooleanValue = true;
        //byte?
        byte? nullByteValue = null;
        byte? nullableByteValue = 10;
        //sbyte?
        sbyte? nullSByteValue = null;
        sbyte? nullableSByteValue = -123;
        //char?
        char? nullCharValue = null;
        char? nullableCharValue = 'b';
        //short?
        short? nullInt16Value = null;
        short? nullableInt16Value = -22345;
        //ushort?
        ushort? nullUInt16Value = null;
        ushort? nullableUInt16Value = 65432;
        //int?
        int? nullInt32Value = null;
        int? nullableInt32Value = 123454789;
        //uint?
        uint? nullUInt32Value = null;
        uint? nullableUInt32Value = 1234567890;
        //long?
        long? nullInt64Value = null;
        long? nullableInt64Value = -1234567890123456789;
        //ulong?
        ulong? nullUInt64Value = null;
        ulong? nullableUInt64Value = 12345678901234567890;
        //float?
        float? nullSingleValue = null;
        float? nullableSingleValue = 123.456f;
        //double?
        double? nullDoubleValue = null;
        double? nullableDoubleValue = 111.222;
        //decimal?
        decimal? nullDecimalValue = null;
        decimal? nullableDecimalValue = 654.321m;

        int offset = 0;

        buffer.WriteBool(trueBooleanValue, offset);
        offset += PrimitiveSerializer.BoolSize;

        buffer.WriteBool(falseBooleanValue, offset);
        offset += PrimitiveSerializer.BoolSize;

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
        offset += PrimitiveSerializer.ShortSize;

        buffer.WriteInt(int32Value, offset);
        offset += PrimitiveSerializer.IntSize;

        buffer.WriteLong(int64Value, offset);
        offset += PrimitiveSerializer.LongSize;

        buffer.WriteSbyte(sByteValue, offset);
        offset += PrimitiveSerializer.SbyteSize;

        buffer.WriteFloat(singleValue, offset);
        offset += PrimitiveSerializer.FloatSize;

        buffer.WriteUshort(uint16Value, offset);
        offset += PrimitiveSerializer.UshortSize;

        buffer.WriteUint(uint32Value, offset);
        offset += PrimitiveSerializer.UintSize;

        buffer.WriteUlong(uint64Value, offset);
        offset += PrimitiveSerializer.UlongSize;

        buffer.WriteBoolNullable(nullBooleanValue, offset);
        offset += PrimitiveSerializer.NullableBoolSize;

        buffer.WriteBoolNullable(nullableTrueBooleanValue, offset);
        offset += PrimitiveSerializer.NullableBoolSize;

        buffer.WriteByteNullable(nullByteValue, offset);
        offset += PrimitiveSerializer.NullableByteSize;

        buffer.WriteByteNullable(nullableByteValue, offset);
        offset += PrimitiveSerializer.NullableByteSize;

        buffer.WriteSbyteNullable(nullSByteValue, offset);
        offset += PrimitiveSerializer.NullableSbyteSize;

        buffer.WriteSbyteNullable(nullableSByteValue, offset);
        offset += PrimitiveSerializer.NullableSbyteSize;

        buffer.WriteCharNullable(nullCharValue, offset);
        offset += PrimitiveSerializer.NullableCharSize;

        buffer.WriteCharNullable(nullableCharValue, offset);
        offset += PrimitiveSerializer.NullableCharSize;

        buffer.WriteShortNullable(nullInt16Value, offset);
        offset += PrimitiveSerializer.NullableShortSize;

        buffer.WriteShortNullable(nullableInt16Value, offset);
        offset += PrimitiveSerializer.NullableShortSize;

        buffer.WriteUshortNullable(nullUInt16Value, offset);
        offset += PrimitiveSerializer.NullableUshortSize;

        buffer.WriteUshortNullable(nullableUInt16Value, offset);
        offset += PrimitiveSerializer.NullableUshortSize;

        buffer.WriteIntNullable(nullInt32Value, offset);
        offset += PrimitiveSerializer.NullableIntSize;

        buffer.WriteIntNullable(nullableInt32Value, offset);
        offset += PrimitiveSerializer.NullableIntSize;

        buffer.WriteUintNullable(nullUInt32Value, offset);
        offset += PrimitiveSerializer.NullableUintSize;

        buffer.WriteUintNullable(nullableUInt32Value, offset);
        offset += PrimitiveSerializer.NullableUintSize;

        buffer.WriteLongNullable(nullInt64Value, offset);
        offset += PrimitiveSerializer.NullableLongSize;

        buffer.WriteLongNullable(nullableInt64Value, offset);
        offset += PrimitiveSerializer.NullableLongSize;

        buffer.WriteUlongNullable(nullUInt64Value, offset);
        offset += PrimitiveSerializer.NullableUlongSize;

        buffer.WriteUlongNullable(nullableUInt64Value, offset);
        offset += PrimitiveSerializer.NullableUlongSize;

        buffer.WriteFloatNullable(nullSingleValue, offset);
        offset += PrimitiveSerializer.NullableFloatSize;

        buffer.WriteFloatNullable(nullableSingleValue, offset);
        offset += PrimitiveSerializer.NullableFloatSize;

        buffer.WriteDoubleNullable(nullDoubleValue, offset);
        offset += PrimitiveSerializer.NullableDoubleSize;

        buffer.WriteDoubleNullable(nullableDoubleValue, offset);
        offset += PrimitiveSerializer.NullableDoubleSize;

        buffer.WriteDecimalNullable(nullDecimalValue, offset);
        offset += PrimitiveSerializer.NullableDecimalSize;

        buffer.WriteDecimalNullable(nullableDecimalValue, offset);
        offset += PrimitiveSerializer.NullableDecimalSize;

        int byteCount = offset;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Assert.IsTrue(byteCount <= buffer.Length);

        using (RpcSocket clientSocket = connection.Client)
        {
            await clientSocket.SendAsync(new ArraySegment<byte>(buffer, 0, byteCount));
            clientSocket.Dispose();
        }

        byte[] bytes = await listenTask;

        Assert.IsTrue(bytes.Length == byteCount);

        offset = 0;

        bool trueBooleanResult = bytes.ReadBool(offset);
        offset += PrimitiveSerializer.BoolSize;

        bool falseBooleanResult = bytes.ReadBool(offset);
        offset += PrimitiveSerializer.BoolSize;

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
        offset += PrimitiveSerializer.ShortSize;

        Int32 int32Result = bytes.ReadInt(offset);
        offset += PrimitiveSerializer.IntSize;

        Int64 int64Result = bytes.ReadLong(offset);
        offset += PrimitiveSerializer.LongSize;

        SByte sByteResult = bytes.ReadSbyte(offset);
        offset += PrimitiveSerializer.SbyteSize;

        Single singleResult = bytes.ReadFloat(offset);
        offset += PrimitiveSerializer.FloatSize;

        UInt16 uint16Result = bytes.ReadUshort(offset);
        offset += PrimitiveSerializer.UshortSize;

        UInt32 uint32Result = bytes.ReadUint(offset);
        offset += PrimitiveSerializer.UintSize;

        UInt64 uint64Result = bytes.ReadUlong(offset);
        offset += PrimitiveSerializer.UlongSize;

        bool? nullBooleanResult = bytes.ReadBoolNullable(offset);
        offset += PrimitiveSerializer.NullableBoolSize;

        bool? nullableTrueBooleanResult = bytes.ReadBoolNullable(offset);
        offset += PrimitiveSerializer.NullableBoolSize;

        byte? nullByteResult = bytes.ReadByteNullable(offset);
        offset += PrimitiveSerializer.NullableByteSize;

        byte? nullableByteResult = bytes.ReadByteNullable(offset);
        offset += PrimitiveSerializer.NullableByteSize;

        sbyte? nullSByteResult = bytes.ReadSbyteNullable(offset);
        offset += PrimitiveSerializer.NullableSbyteSize;

        sbyte? nullableSByteResult = bytes.ReadSbyteNullable(offset);
        offset += PrimitiveSerializer.NullableSbyteSize;

        char? nullCharResult = bytes.ReadCharNullable(offset);
        offset += PrimitiveSerializer.NullableCharSize;

        char? nullableCharResult = bytes.ReadCharNullable(offset);
        offset += PrimitiveSerializer.NullableCharSize;

        short? nullInt16Result = bytes.ReadShortNullable(offset);
        offset += PrimitiveSerializer.NullableShortSize;

        short? nullableInt16Result = bytes.ReadShortNullable(offset);
        offset += PrimitiveSerializer.NullableShortSize;

        ushort? nullUInt16Result = bytes.ReadUshortNullable(offset);
        offset += PrimitiveSerializer.NullableUshortSize;

        ushort? nullableUInt16Result = bytes.ReadUshortNullable(offset);
        offset += PrimitiveSerializer.NullableUshortSize;

        int? nullInt32Result = bytes.ReadIntNullable(offset);
        offset += PrimitiveSerializer.NullableIntSize;

        int? nullableInt32Result = bytes.ReadIntNullable(offset);
        offset += PrimitiveSerializer.NullableIntSize;

        uint? nullUInt32Result = bytes.ReadUintNullable(offset);
        offset += PrimitiveSerializer.NullableUintSize;

        uint? nullableUInt32Result = bytes.ReadUintNullable(offset);
        offset += PrimitiveSerializer.NullableUintSize;

        long? nullInt64Result = bytes.ReadLongNullable(offset);
        offset += PrimitiveSerializer.NullableLongSize;

        long? nullableInt64Result = bytes.ReadLongNullable(offset);
        offset += PrimitiveSerializer.NullableLongSize;

        ulong? nullUInt64Result = bytes.ReadUlongNullable(offset);
        offset += PrimitiveSerializer.NullableUlongSize;

        ulong? nullableUInt64Result = bytes.ReadUlongNullable(offset);
        offset += PrimitiveSerializer.NullableUlongSize;

        float? nullSingleResult = bytes.ReadFloatNullable(offset);
        offset += PrimitiveSerializer.NullableFloatSize;

        float? nullableSingleResult = bytes.ReadFloatNullable(offset);
        offset += PrimitiveSerializer.NullableFloatSize;

        double? nullDoubleResult = bytes.ReadDoubleNullable(offset);
        offset += PrimitiveSerializer.NullableDoubleSize;

        double? nullableDoubleResult = bytes.ReadDoubleNullable(offset);
        offset += PrimitiveSerializer.NullableDoubleSize;

        decimal? nullDecimalResult = bytes.ReadDecimalNullable(offset);
        offset += PrimitiveSerializer.NullableDecimalSize;

        decimal? nullableDecimalResult = bytes.ReadDecimalNullable(offset);
        offset += PrimitiveSerializer.NullableDecimalSize;

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
        Assert.AreEqual(nullBooleanValue, nullBooleanResult);
        Assert.AreEqual(nullableTrueBooleanValue, nullableTrueBooleanResult);
        Assert.AreEqual(nullByteValue, nullByteResult);
        Assert.AreEqual(nullableByteValue, nullableByteResult);
        Assert.AreEqual(nullSByteValue, nullSByteResult);
        Assert.AreEqual(nullableSByteValue, nullableSByteResult);
        Assert.AreEqual(nullCharValue, nullCharResult);
        Assert.AreEqual(nullableCharValue, nullableCharResult);
        Assert.AreEqual(nullInt16Value, nullInt16Result);
        Assert.AreEqual(nullableInt16Value, nullableInt16Result);
        Assert.AreEqual(nullUInt16Value, nullUInt16Result);
        Assert.AreEqual(nullableUInt16Value, nullableUInt16Result);
        Assert.AreEqual(nullInt32Value, nullInt32Result);
        Assert.AreEqual(nullableInt32Value, nullableInt32Result);
        Assert.AreEqual(nullUInt32Value, nullUInt32Result);
        Assert.AreEqual(nullableUInt32Value, nullableUInt32Result);
        Assert.AreEqual(nullInt64Value, nullInt64Result);
        Assert.AreEqual(nullableInt64Value, nullableInt64Result);
        Assert.AreEqual(nullUInt64Value, nullUInt64Result);
        Assert.AreEqual(nullableUInt64Value, nullableUInt64Result);
        Assert.AreEqual(nullSingleValue, nullSingleResult);
        Assert.AreEqual(nullableSingleValue, nullableSingleResult);
        Assert.AreEqual(nullDoubleValue, nullDoubleResult);
        Assert.AreEqual(nullableDoubleValue, nullableDoubleResult);
        Assert.AreEqual(nullDecimalValue, nullDecimalResult);
        Assert.AreEqual(nullableDecimalValue, nullableDecimalResult);
    }
}