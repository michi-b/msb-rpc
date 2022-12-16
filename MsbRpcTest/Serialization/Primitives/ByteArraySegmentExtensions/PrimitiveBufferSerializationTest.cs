﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpcTest.Serialization.Primitives.ByteArraySegmentExtensions;

public abstract class PrimitiveBufferSerializationTest<TPrimitive> where TPrimitive : struct
{
    protected ArraySegment<byte> SingleElementBuffer { get; private set; } = null!;

    private static int ElementSize => PrimitivesUtility.GetSize(typeof(TPrimitive).FullName!);

    [TestInitialize]
    public void Setup()
    {
        SingleElementBuffer = new byte[ElementSize];
    }

    [TestCleanup]
    public void Cleanup()
    {
        SingleElementBuffer = null!;
    }

    protected abstract void WriteSingleElement(TPrimitive value);

    protected abstract TPrimitive ReadSingleElement();

    protected void TestPreserves(TPrimitive value)
    {
        WriteSingleElement(value);
        TPrimitive result = ReadSingleElement();
        Assert.AreEqual(value, result);
    }

    protected int GetOffset(int elementIndex) => elementIndex * ElementSize;

    protected ArraySegment<byte> GetByteArraySegment(int elementCount) => new(new byte[GetOffset(elementCount)]);

    protected ArraySegment<byte> GetBuffer(int elementCount) => new byte[GetOffset(elementCount)];
}