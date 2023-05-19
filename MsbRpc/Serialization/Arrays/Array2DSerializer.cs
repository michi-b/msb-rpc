using System;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using static MsbRpc.Serialization.Buffers.BufferReader;
using static MsbRpc.Serialization.Buffers.BufferWriter;

namespace MsbRpc.Serialization.Arrays;

public static class Array2DSerializer<TElement>
{
    [MayBeUsedByGeneratedCode]
    public static int GetSize(TElement[,] array, Func<TElement, int> getElementSize)
    {
        int size = PrimitiveSerializer.IntSize * 2;
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                size += getElementSize(array[i, j]);
            }
        }

        return size;
    }

    [MayBeUsedByGeneratedCode]
    public static void Write(ref BufferWriter bufferWriter, TElement[,] array, WriteDelegate<TElement> writeElement)
    {
        bufferWriter.Write(array.GetLength(0));
        bufferWriter.Write(array.GetLength(1));

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                TElement element = array[i, j];
                bufferWriter.WriteCustom(element, writeElement);
            }
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TElement[,] Read(ref BufferReader reader, ReadDelegate<TElement> readElement)
    {
        int length0 = reader.ReadInt();
        int length1 = reader.ReadInt();

        var array = new TElement[length0, length1];

        for (int i = 0; i < length0; i++)
        {
            for (int j = 0; j < length1; j++)
            {
                TElement element = reader.ReadCustom(readElement);
                array[i, j] = element;
            }
        }

        return array;
    }
}