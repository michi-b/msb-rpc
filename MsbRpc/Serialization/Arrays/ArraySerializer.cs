#region

using System;
using System.Linq;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using static MsbRpc.Serialization.Buffers.BufferReader;
using static MsbRpc.Serialization.Buffers.BufferWriter;

#endregion

namespace MsbRpc.Serialization.Arrays;

public static class ArraySerializer<TElement>
{
    #region 1D

    [MayBeUsedByGeneratedCode]
    public static int GetSize(TElement[] array, Func<TElement, int> getElementSize) => PrimitiveSerializer.IntSize + array.Sum(getElementSize);

    [MayBeUsedByGeneratedCode]
    public static int GetSize(TElement[] array, int constantElementSize) => PrimitiveSerializer.IntSize + array.Length * constantElementSize;

    [MayBeUsedByGeneratedCode]
    public static void Write(ref BufferWriter writer, TElement[] array, WriteDelegate<TElement> writeElement)
    {
        writer.Write(array.Length);
        foreach (TElement element in array)
        {
            writer.WriteCustom(element, writeElement);
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TElement[] Read(ref BufferReader reader, ReadDelegate<TElement> readElement)
    {
        int length = reader.ReadInt();
        var array = new TElement[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = reader.ReadCustom(readElement);
        }

        return array;
    }

    #endregion
}