﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.Primitives;

public abstract class PrimitiveSerializationTest<T>
{
    protected byte[] SingleElementBuffer { get; private set; } = null!;

    protected PrimitiveSerializer Serializer { get; } = new();

    protected abstract int ElementSize { get; }

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

    protected abstract void WriteSingleElement(T value);

    protected abstract T ReadSingleElement();

    protected void TestPreserves(T value)
    {
        WriteSingleElement(value);
        T result = ReadSingleElement();
        Assert.AreEqual(value, result);
    }

    protected int GetOffset(int elementIndex) => elementIndex * ElementSize;

    protected byte[] GetBuffer(int elementCount) => new byte[GetOffset(elementCount)];
}