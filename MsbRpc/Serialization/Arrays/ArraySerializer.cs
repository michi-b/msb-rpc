using System;
using System.Linq;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Arrays;

public static class ArraySerializer<TElement>
{
    #region 1D

    [MayBeUsedByGeneratedCode]
    public static int GetSize(TElement[] array, Func<TElement, int> getElementSize) => PrimitiveSerializer.IntSize + array.Sum(getElementSize);

    [MayBeUsedByGeneratedCode]
    public static void Write(ref BufferWriter bufferWriter, TElement[] array, BufferWriter.WriteDelegate<TElement> writeElement)
    {
        bufferWriter.Write(array.Length);
        foreach (TElement element in array)
        {
            bufferWriter.WriteCustom(element, writeElement);
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TElement[] Read(ref BufferReader reader, BufferReader.ReadDelegate<TElement> readElement)
    {
        int length = reader.ReadInt();
        var array = new TElement[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = reader.Read(readElement);
        }

        return array;
    }

    #endregion
}