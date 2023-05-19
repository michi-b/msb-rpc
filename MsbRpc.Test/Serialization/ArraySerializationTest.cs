using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Arrays;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Test.Serialization;

[TestClass]
public class ArraySerializationTest : Base.Test
{
    [TestMethod]
    public void PreservesIntArray()
    {
        int[] value = { 5, 4, 2, 3, 1 };
        int length = value.Length;
        int[] expected = new int[length];
        Array.Copy(value, expected, length);

        int size = ArraySerializer<int>.GetSize(value, _ => PrimitiveSerializer.IntSize);

        ArraySegment<byte> buffer = BufferUtility.Create(size);

        var writer = new BufferWriter(buffer);

        ArraySerializer<int>.Write(ref writer, value, (ref BufferWriter w, int v) => w.Write(v));

        var reader = new BufferReader(buffer);

        int[] result = ArraySerializer<int>.Read(ref reader, (ref BufferReader r) => r.ReadInt());

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Preserves3DimensionalStringArray()
    {
        string[,,] value =
        {
            { { "Hello" }, { "World!" } },
            {
                { "my name is" },
                // ReSharper disable once StringLiteralTypo
                { "Michi" }
            },
            { { string.Empty }, { "yes" } }
        };

        int size = Array3DSerializer<string>.GetSize(value, StringSerializer.GetSize);

        ArraySegment<byte> buffer = BufferUtility.Create(size);

        var writer = new BufferWriter(buffer);

        Array3DSerializer<string>.Write(ref writer, value, (ref BufferWriter w, string s) => w.Write(s));

        var reader = new BufferReader(buffer);

        string[,,] result = Array3DSerializer<string>.Read(ref reader, (ref BufferReader r) => r.ReadString());

        CollectionAssert.AreEqual(value, result);
    }
}