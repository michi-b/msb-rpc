using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRps.Serialization;

namespace MsbRpsTest.Serialization.Primitives;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyleForMemberAccess")]
public class Int32SerializationTest : SerializationTest<Int32>
{
    protected override int BufferSize => sizeof(Int32);

    [TestMethod]
    public void PreservesZero()
    {
        TestPreserves(0);
    }

    [TestMethod]
    public void PreservesOne()
    {
        TestPreserves(1);
    }

    [TestMethod]
    public void PreservesMinusOne()
    {
        TestPreserves(-1);
    }

    [TestMethod]
    public void PreservesMinusZero()
    {
        TestPreserves(-0);
    }

    [TestMethod]
    public void PreservesMinimum()
    {
        TestPreserves(Int32.MinValue);
    }

    [TestMethod]
    public void PreservesMaximum()
    {
        TestPreserves(Int32.MaxValue);
    }

    protected override void Write(Int32 value, byte[] buffer)
    {
        buffer.Write(value);
    }

    protected override int Read(byte[] buffer) => buffer.ReadInt32();
}