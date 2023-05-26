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

        TestContext.WriteLine("Size is {0}", size);

        ArraySegment<byte> buffer = BufferUtility.Create(size);

        var writer = new BufferWriter(buffer);

        Array3DSerializer<string>.Write(ref writer, value, (ref BufferWriter w, string s) => w.Write(s));

        var reader = new BufferReader(buffer);

        string[,,] result = Array3DSerializer<string>.Read(ref reader, (ref BufferReader r) => r.ReadString());

        CollectionAssert.AreEqual(value, result);
    }

    [TestMethod]
    public void Preserves11DimensionalSingleCharArray()
    {
        const int rank = 11;

        char[,,,,,,,,,,] value =
        {
            { { { { { { { { { { 'H' } } } } } } } } } },
            { { { { { { { { { { 'e' } } } } } } } } } },
            { { { { { { { { { { 'l' } } } } } } } } } },
            { { { { { { { { { { 'l' } } } } } } } } } },
            { { { { { { { { { { 'o' } } } } } } } } } }
        };

        Assert.AreEqual(rank, value.Rank);

        int size = AnyRankArraySerializer<char>.GetSize(value, _ => PrimitiveSerializer.CharSize);

        TestContext.WriteLine("Size is {0}", size);

        ArraySegment<byte> buffer = BufferUtility.Create(size);

        var writer = new BufferWriter(buffer);

        AnyRankArraySerializer<char>.Write(ref writer, value, (ref BufferWriter w, char charValue) => w.Write(charValue));

        var reader = new BufferReader(buffer);

        char[,,,,,,,,,,] result = (char[,,,,,,,,,,])AnyRankArraySerializer<char>.Read(ref reader, rank, (ref BufferReader r) => r.ReadChar());

        CollectionAssert.AreEqual(value, result);
    }

    [TestMethod]
    public void Preserves11DimensionalTwoCharArray()
    {
        const int rank = 11;

        char[,,,,,,,,,,] value =
        {
            { { { { { { { { { { 'H', 'e' } } } } } } } } } },
            { { { { { { { { { { 'l', 'l' } } } } } } } } } },
            { { { { { { { { { { 'o', 'W' } } } } } } } } } },
            { { { { { { { { { { 'o', 'r' } } } } } } } } } },
            { { { { { { { { { { 'l', 'd' } } } } } } } } } }
        };

        Assert.AreEqual(rank, value.Rank);

        int size = AnyRankArraySerializer<char>.GetSize(value, _ => PrimitiveSerializer.CharSize);

        TestContext.WriteLine("Size is {0}", size);

        ArraySegment<byte> buffer = BufferUtility.Create(size);

        var writer = new BufferWriter(buffer);

        AnyRankArraySerializer<char>.Write(ref writer, value, (ref BufferWriter w, char charValue) => w.Write(charValue));

        var reader = new BufferReader(buffer);

        char[,,,,,,,,,,] result = (char[,,,,,,,,,,])AnyRankArraySerializer<char>.Read(ref reader, rank, (ref BufferReader r) => r.ReadChar());

        CollectionAssert.AreEqual(value, result);
    }
}