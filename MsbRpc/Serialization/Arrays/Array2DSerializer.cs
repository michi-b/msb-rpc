using System;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

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
    public static void Write(BufferWriter bufferWriter, TElement[,] array, Action<BufferWriter, TElement> writeElement)
    {
        bufferWriter.Write(array.GetLength(0));
        bufferWriter.Write(array.GetLength(1));

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                writeElement(bufferWriter, array[i, j]);
            }
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TElement[,] Read(ref BufferReader reader, Func<BufferReader, TElement> readElement)
    {
        int length0 = reader.ReadInt();
        int length1 = reader.ReadInt();

        var array = new TElement[length0, length1];

        for (int i = 0; i < length0; i++)
        {
            for (int j = 0; j < length1; j++)
            {
                array[i, j] = readElement(reader);
            }
        }

        return array;
    }
}