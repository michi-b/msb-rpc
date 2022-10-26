﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives.ByteArray;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class BooleanSerializationTest : PrimitiveByteArraySerializationTest<Boolean>
{
    [TestMethod]
    public void PreservesTrue()
    {
        TestPreserves(true);
    }

    [TestMethod]
    public void PreservesFalse()
    {
        TestPreserves(false);
    }

    [TestMethod]
    public void PreservesTrueWithOffset()
    {
        const Boolean value = true;
        const int offset = 3;
        byte[] buffer = GetBuffer(7);
        buffer.WriteBoolean(value, offset);
        Boolean result = buffer.ReadBoolean(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesFalseWithOffset()
    {
        const Boolean value = false;
        const int offset = 2;
        byte[] buffer = GetBuffer(4);
        buffer.WriteBoolean(value, offset);
        Boolean result = buffer.ReadBoolean(offset);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void PreservesSeries()
    {
        const Boolean value0 = true;
        const Boolean value1 = false;
        const Boolean value2 = false;
        const Boolean value3 = true;
        const Boolean value4 = false;

        byte[] buffer = GetBuffer(5);

        buffer.WriteBoolean(value0);
        buffer.WriteBoolean(value1, GetOffset(1));
        buffer.WriteBoolean(value2, GetOffset(2));
        buffer.WriteBoolean(value3, GetOffset(3));
        buffer.WriteBoolean(value4, GetOffset(4));

        Boolean result0 = buffer.ReadBoolean(GetOffset(0));
        Boolean result1 = buffer.ReadBoolean(GetOffset(1));
        Boolean result2 = buffer.ReadBoolean(GetOffset(2));
        Boolean result3 = buffer.ReadBoolean(GetOffset(3));
        Boolean result4 = buffer.ReadBoolean(GetOffset(4));

        Assert.AreEqual(value0, result0);
        Assert.AreEqual(value1, result1);
        Assert.AreEqual(value2, result2);
        Assert.AreEqual(value3, result3);
        Assert.AreEqual(value4, result4);
    }

    protected override void WriteSingleElement(Boolean value)
    {
        SingleElementBuffer.WriteBoolean(value);
    }

    protected override Boolean ReadSingleElement() => SingleElementBuffer.ReadBoolean();
}