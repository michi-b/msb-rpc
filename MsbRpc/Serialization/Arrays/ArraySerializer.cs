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
    public static void Write(BufferWriter bufferWriter, TElement[] array, Action<BufferWriter, TElement> writeElement)
    {
        bufferWriter.Write(array.Length);
        foreach (TElement element in array)
        {
            writeElement(bufferWriter, element);
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TElement[] Read(ref BufferReader reader, Func<BufferReader, TElement> readElement)
    {
        int length = reader.ReadInt();
        var array = new TElement[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = readElement(reader);
        }

        return array;
    }

    #endregion
}