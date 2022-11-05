using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;
using MsbRpc.Serialization.ByteArraySegment;
using MsbRpc.Serialization.Primitives;
using MsbRpcTest.Serialization.Primitives.ByteArray;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

[TestClass]
#pragma warning disable IDE0079 // Remove unnecessary suppression
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class CharSerializationTest : PrimitiveByteArraySegmentSerializationTest<Char>
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
        ArraySegment<byte> buffer = GetBuffer(8);
        buffer.WriteChar(value, offset);
        int result = buffer.ReadChar(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Char value0 = 'c';
        const Char value1 = 'b';
        const Char value2 = 'a';

        ArraySegment<byte> buffer = GetBuffer(3);

        buffer.WriteChar(value0, GetOffset(0));
        buffer.WriteChar(value1, GetOffset(1));
        buffer.WriteChar(value2, GetOffset(2));

        Char result0 = buffer.ReadChar(GetOffset(0));
        Char result1 = buffer.ReadChar(GetOffset(1));
        Char result2 = buffer.ReadChar(GetOffset(2));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
    }

    protected override void WriteSingleElement(Char value)
    {
        SingleElementBuffer.WriteChar(value);
    }

    protected override Char ReadSingleElement() => SingleElementBuffer.ReadChar();
}