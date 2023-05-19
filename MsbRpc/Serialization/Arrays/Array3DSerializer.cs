using System;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Arrays;

public static class Array3DSerializer<TElement>
{
    [MayBeUsedByGeneratedCode]
    public static int GetSize(TElement[,,] array, Func<TElement, int> getElementSize)
    {
        int size = PrimitiveSerializer.IntSize * 3;

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                for (int k = 0; k < array.GetLength(2); k++)
                {
                    size += getElementSize(array[i, j, k]);
                }
            }
        }

        return size;
    }

    [MayBeUsedByGeneratedCode]
    public static void Write(BufferWriter bufferWriter, TElement[,,] array, Action<BufferWriter, TElement> writeElement)
    {
        bufferWriter.Write(array.GetLength(0));
        bufferWriter.Write(array.GetLength(1));
        bufferWriter.Write(array.GetLength(2));

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                for (int k = 0; k < array.GetLength(2); k++)
                {
                    writeElement(bufferWriter, array[i, j, k]);
                }
            }
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TElement[,,] Read(ref BufferReader reader, Func<BufferReader, TElement> readElement)
    {
        int length0 = reader.ReadInt();
        int length1 = reader.ReadInt();
        int length2 = reader.ReadInt();

        var array = new TElement[length0, length1, length2];

        for (int i = 0; i < length0; i++)
        {
            for (int j = 0; j < length1; j++)
            {
                for (int k = 0; k < length2; k++)
                {
                    array[i, j, k] = readElement(reader);
                }
            }
        }

        return array;
    }
}