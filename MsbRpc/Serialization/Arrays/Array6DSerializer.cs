using System;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Arrays;

public static class Array6DSerializer<TElement>
{
    [MayBeUsedByGeneratedCode]
    public static int GetSize(TElement[,,,,,] array, Func<TElement, int> getElementSize)
    {
        int size = PrimitiveSerializer.IntSize * 6;

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                for (int k = 0; k < array.GetLength(2); k++)
                {
                    for (int l = 0; l < array.GetLength(3); l++)
                    {
                        for (int m = 0; m < array.GetLength(4); m++)
                        {
                            for (int n = 0; n < array.GetLength(5); n++)
                            {
                                size += getElementSize(array[i, j, k, l, m, n]);
                            }
                        }
                    }
                }
            }
        }

        return size;
    }

    [MayBeUsedByGeneratedCode]
    public static void Write(BufferWriter bufferWriter, TElement[,,,,,] array, Action<BufferWriter, TElement> writeElement)
    {
        bufferWriter.Write(array.GetLength(0));
        bufferWriter.Write(array.GetLength(1));
        bufferWriter.Write(array.GetLength(2));
        bufferWriter.Write(array.GetLength(3));
        bufferWriter.Write(array.GetLength(4));
        bufferWriter.Write(array.GetLength(5));

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                for (int k = 0; k < array.GetLength(2); k++)
                {
                    for (int l = 0; l < array.GetLength(3); l++)
                    {
                        for (int m = 0; m < array.GetLength(4); m++)
                        {
                            for (int n = 0; n < array.GetLength(5); n++)
                            {
                                writeElement(bufferWriter, array[i, j, k, l, m, n]);
                            }
                        }
                    }
                }
            }
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TElement[,,,,,] Read(BufferReader bufferReader, Func<BufferReader, TElement> readElement)
    {
        int length0 = bufferReader.ReadInt();
        int length1 = bufferReader.ReadInt();
        int length2 = bufferReader.ReadInt();
        int length3 = bufferReader.ReadInt();
        int length4 = bufferReader.ReadInt();
        int length5 = bufferReader.ReadInt();

        var array = new TElement[length0, length1, length2, length3, length4, length5];

        for (int i = 0; i < length0; i++)
        {
            for (int j = 0; j < length1; j++)
            {
                for (int k = 0; k < length2; k++)
                {
                    for (int l = 0; l < length3; l++)
                    {
                        for (int m = 0; m < length4; m++)
                        {
                            for (int n = 0; n < length5; n++)
                            {
                                array[i, j, k, l, m, n] = readElement(bufferReader);
                            }
                        }
                    }
                }
            }
        }

        return array;
    }
}