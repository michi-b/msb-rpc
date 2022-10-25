using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;

namespace MsbRpcTest.Serialization.Primitives.ByteArrayExtensions;

[TestClass]
public class BooleanSerializationTest : PrimitiveByteArraySegmentSerializationTest<bool>
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

    protected override void WriteSingleElement(bool value) => SingleElementBuffer.WriteBoolean(value);

    protected override bool ReadSingleElement() => SingleElementBuffer.ReadBoolean();
}