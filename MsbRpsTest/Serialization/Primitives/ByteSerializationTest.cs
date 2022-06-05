using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRps.Serialization;
using MsbRps.Serialization.Primitives;

namespace MsbRpsTest.Serialization.Primitives;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class ByteSerializationTest : PrimitiveSerializationTest<Byte>
{
    protected override int ElementSize { get; } = sizeof(Boolean);

    [TestMethod]
    public void PreservesMinValue()
    {
        TestPreserves(Byte.MinValue);
    }

    [TestMethod]
    public void PreservesMaxValue()
    {
        TestPreserves(Byte.MaxValue);
    }

    [TestMethod]
    public void Preserves2WithOffset()
    {
        const Byte value = 2;
        const int offset = 4;
        byte[] buffer = GetBuffer(7);
        PrimitiveSerializer.Write(value, buffer, offset);
        Byte result = PrimitiveSerializer.ReadByte(buffer, offset);
        Assert.AreEqual(value, result);
    }
    
    [TestMethod]
    public void PreservesSeries()
    {
        const Byte value0 = 5;
        const Byte value1 = 4;
        const Byte value2 = 1;
        const Byte value3 = 3;
        const Byte value4 = 2;

        byte[] buffer = GetBuffer(5);

        PrimitiveSerializer.Write(value0, buffer, 0);
        PrimitiveSerializer.Write(value1, buffer, GetOffset(1));
        PrimitiveSerializer.Write(value2, buffer, GetOffset(2));
        PrimitiveSerializer.Write(value3, buffer, GetOffset(3));
        PrimitiveSerializer.Write(value4, buffer, GetOffset(4));

        Byte result0 = PrimitiveSerializer.ReadByte(buffer, GetOffset(0));
        Byte result1 = PrimitiveSerializer.ReadByte(buffer, GetOffset(1));
        Byte result2 = PrimitiveSerializer.ReadByte(buffer, GetOffset(2));
        Byte result3 = PrimitiveSerializer.ReadByte(buffer, GetOffset(3));
        Byte result4 = PrimitiveSerializer.ReadByte(buffer, GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(byte value)
    {
        PrimitiveSerializer.Write(value, SingleElementBuffer);
    }

    protected override Byte ReadSingleElement() => PrimitiveSerializer.ReadByte(SingleElementBuffer);
}