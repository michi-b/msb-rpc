using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives.ByteArray;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class CharSerializationTest : PrimitiveByteArraySerializationTest<Char>
{
    [TestMethod]
    public void PreservesA()
    {
        TestPreserves('A');
    }

    [TestMethod]
    public void PreservesLowerCaseZ()
    {
        TestPreserves('z');
    }

    [TestMethod]
    public void PreservesMinimum()
    {
        TestPreserves(Char.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(Char.MaxValue);
    }

    [TestMethod]
    public void PreservesWithOffset()
    {
        const Char value = 'o';
        const int offset = 13;
        byte[] buffer = GetBuffer(8);
        Serializer.WriteChar(value, buffer, offset);
        int result = PrimitiveSerializer.ReadChar(buffer, offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Char value0 = 'c';
        const Char value1 = 'b';
        const Char value2 = 'a';

        byte[] buffer = GetBuffer(3);

        Serializer.WriteChar(value0, buffer, GetOffset(0));
        Serializer.WriteChar(value1, buffer, GetOffset(1));
        Serializer.WriteChar(value2, buffer, GetOffset(2));

        Char result0 = PrimitiveSerializer.ReadChar(buffer, GetOffset(0));
        Char result1 = PrimitiveSerializer.ReadChar(buffer, GetOffset(1));
        Char result2 = PrimitiveSerializer.ReadChar(buffer, GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Char value)
    {
        Serializer.WriteChar(value, SingleElementBuffer);
    }

    protected override Char ReadSingleElement() => PrimitiveSerializer.ReadChar(SingleElementBuffer);
}