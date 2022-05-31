using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRps.Serialization;

namespace MsbRpsTest.Serialization.Primitives;

[TestClass]
[SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
public class BooleanSerializationTest : SerializationTest<Boolean>
{
    protected override int BufferSize { get; } = sizeof(Boolean);

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

    protected override void Write(Boolean value, byte[] buffer)
    {
        buffer.Write(value);
    }

    protected override Boolean Read(byte[] buffer) => buffer.ReadBoolean();
}