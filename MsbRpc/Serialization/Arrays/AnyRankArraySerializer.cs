using System;
using System.Diagnostics;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using static MsbRpc.Serialization.Buffers.BufferReader;
using static MsbRpc.Serialization.Buffers.BufferWriter;

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
    public static int GetSize(Array array, int constantElementSize) => PrimitiveSerializer.IntSize * array.Rank + GetElementCount(array) * constantElementSize;

    [MayBeUsedByGeneratedCode]
    public static void Write(ref BufferWriter writer, Array array, WriteDelegate<TElement> writeElement)
    {
        int rankCount = array.Rank;

        for (int rank = 0; rank < rankCount; rank++)
        {
            writer.Write(array.GetLength(rank));
        }

        int[] accumulatedCounts = GetAccumulatedCounts(array);
        int[] currentIndex = new int[rankCount];

        int elementCount = GetElementCount(array);

        for (int linearIndex = 0; linearIndex < elementCount; linearIndex++)
        {
            DeLinearizeIndex(linearIndex, accumulatedCounts, currentIndex);
            var element = (TElement)array.GetValue(currentIndex);
            writer.WriteCustom(element, writeElement);
        }
    }

    [MayBeUsedByGeneratedCode]
    public static Array Read(ref BufferReader reader, int rank, ReadDelegate<TElement> readElement)
    {
        int[] lengths = new int[rank];

        for (int i = 0; i < rank; i++)
        {
            lengths[i] = reader.ReadInt();
        }

        var array = Array.CreateInstance(typeof(TElement), lengths);

        int[] accumulatedCounts = GetAccumulatedCounts(array);
        int[] currentIndex = new int[rank];

        int elementCount = GetElementCount(array);

        for (int linearIndex = 0; linearIndex < elementCount; linearIndex++)
        {
            DeLinearizeIndex(linearIndex, accumulatedCounts, currentIndex);
            TElement element = reader.ReadCustom(readElement);
            array.SetValue(element, currentIndex);
        }

        return array;
    }

    private static void DeLinearizeIndex(int linearIndex, int[] accumulatedCounts, int[] result)
    {
        int maxRank = accumulatedCounts.Length - 1;
        for (int currentRank = 0; currentRank < maxRank; currentRank++)
        {
            int nextRankAccumulatedCount = accumulatedCounts[currentRank + 1];
            int currentDimensionIndex = linearIndex / nextRankAccumulatedCount;
            result[currentRank] = currentDimensionIndex;
            linearIndex -= currentDimensionIndex * nextRankAccumulatedCount;
        }

        Debug.Assert(linearIndex < accumulatedCounts[maxRank]);
        result[maxRank] = linearIndex;
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
        int count = 1;
        for (int i = 0; i < array.Rank; i++)
        {
            count *= array.GetLength(i);
        }

        return count;
    }
}