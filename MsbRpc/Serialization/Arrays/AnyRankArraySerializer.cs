using System;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Arrays;

/// <summary>
///     serializes arrays of any rank, and deserializes them with a given predefined rank
/// </summary>
/// <remarks>
///     only use this class if there is no predefined multi-dimensional array serializer for the given rank
///     because it needs dynamic allocations and is much slower than the predefined serializers
/// </remarks>
public static class AnyRankArraySerializer<TElement>
{
    [MayBeUsedByGeneratedCode]
    public static int GetSize(Array array, Func<TElement, int> getElementSize)
    {
        int rank = array.Rank;
        int size = rank * PrimitiveSerializer.IntSize;

        int[] accumulatedCounts = GetAccumulatedCounts(array);
        int[] currentIndex = new int[rank];

        int elementCount = GetElementCount(array);

        for (int linearIndex = 0; linearIndex < elementCount; linearIndex++)
        {
            DeLinearizeIndex(linearIndex, accumulatedCounts, currentIndex);
            size += getElementSize((TElement)array.GetValue(currentIndex));
        }

        return size;
    }

    [MayBeUsedByGeneratedCode]
    public static void Write(BufferWriter bufferWriter, Array array, Action<BufferWriter, TElement> writeElement)
    {
        int rank = array.Rank;

        for (int i = 0; i < rank; i++)
        {
            bufferWriter.Write(array.GetLength(rank));
        }

        int[] accumulatedCounts = GetAccumulatedCounts(array);
        int[] currentIndex = new int[rank];

        int elementCount = GetElementCount(array);

        for (int linearIndex = 0; linearIndex < elementCount; linearIndex++)
        {
            DeLinearizeIndex(linearIndex, accumulatedCounts, currentIndex);
            writeElement(bufferWriter, (TElement)array.GetValue(currentIndex));
        }
    }

    [MayBeUsedByGeneratedCode]
    public static Array Read(BufferReader bufferReader, int rank, Func<BufferReader, TElement> readElement)
    {
        int[] lengths = new int[rank];

        for (int i = 0; i < rank; i++)
        {
            lengths[i] = bufferReader.ReadInt();
        }

        var array = Array.CreateInstance(typeof(TElement), lengths);

        int[] accumulatedCounts = GetAccumulatedCounts(array);
        int[] currentIndex = new int[rank];

        int elementCount = GetElementCount(array);

        for (int linearIndex = 0; linearIndex < elementCount; linearIndex++)
        {
            DeLinearizeIndex(linearIndex, accumulatedCounts, currentIndex);
            array.SetValue(readElement(bufferReader), currentIndex);
        }

        return array;
    }

    private static void DeLinearizeIndex(int linearIndex, int[] accumulatedCounts, int[] result)
    {
        for (int dimension = 0; dimension < accumulatedCounts.Length; dimension++)
        {
            int currentDimensionIndex = linearIndex / accumulatedCounts[dimension];
            result[dimension] = currentDimensionIndex;
            linearIndex -= currentDimensionIndex * accumulatedCounts[dimension];
        }
    }

    private static int[] GetAccumulatedCounts(Array array)
    {
        int rank = array.Rank;

        int[] counts = new int[rank];

        //last rank element count is just the length of the last dimension      
        counts[rank - 1] = array.GetLength(rank - 1);

        for (int dimension = rank - 2; dimension >= 0; dimension--)
        {
            counts[dimension] = array.GetLength(dimension) * counts[dimension + 1];
        }

        return counts;
    }

    private static int GetElementCount(Array array)
    {
        int count = 0;
        for (int i = 0; i < array.Rank; i++)
        {
            count *= array.GetLength(i);
        }

        return count;
    }
}