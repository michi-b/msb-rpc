using System;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization.Arrays;

public static class Array9DSerializer<TElement>
{
    [MayBeUsedByGeneratedCode]
    public static int GetSize(TElement[,,,,,,,,] array, Func<TElement, int> getElementSize)
    {
        int size = PrimitiveSerializer.IntSize * 9;

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
                                for (int o = 0; o < array.GetLength(6); o++)
                                {
                                    for (int p = 0; p < array.GetLength(7); p++)
                                    {
                                        for (int q = 0; q < array.GetLength(8); q++)
                                        {
                                            size += getElementSize(array[i, j, k, l, m, n, o, p, q]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return size;
    }

    [MayBeUsedByGeneratedCode]
    public static void Write(BufferWriter bufferWriter, TElement[,,,,,,,,] array, Action<BufferWriter, TElement> writeElement)
    {
        bufferWriter.Write(array.GetLength(0));
        bufferWriter.Write(array.GetLength(1));
        bufferWriter.Write(array.GetLength(2));
        bufferWriter.Write(array.GetLength(3));
        bufferWriter.Write(array.GetLength(4));
        bufferWriter.Write(array.GetLength(5));
        bufferWriter.Write(array.GetLength(6));
        bufferWriter.Write(array.GetLength(7));
        bufferWriter.Write(array.GetLength(8));

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
                                for (int o = 0; o < array.GetLength(6); o++)
                                {
                                    for (int p = 0; p < array.GetLength(7); p++)
                                    {
                                        for (int q = 0; q < array.GetLength(8); q++)
                                        {
                                            writeElement(bufferWriter, array[i, j, k, l, m, n, o, p, q]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [MayBeUsedByGeneratedCode]
    public static TElement[,,,,,,,,] Read(BufferReader bufferReader, Func<BufferReader, TElement> readElement)
    {
        int length0 = bufferReader.ReadInt();
        int length1 = bufferReader.ReadInt();
        int length2 = bufferReader.ReadInt();
        int length3 = bufferReader.ReadInt();
        int length4 = bufferReader.ReadInt();
        int length5 = bufferReader.ReadInt();
        int length6 = bufferReader.ReadInt();
        int length7 = bufferReader.ReadInt();
        int length8 = bufferReader.ReadInt();

        var array = new TElement[length0, length1, length2, length3, length4, length5, length6, length7, length8];

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
                                for (int o = 0; o < length6; o++)
                                {
                                    for (int p = 0; p < length7; p++)
                                    {
                                        for (int q = 0; q < length8; q++)
                                        {
                                            array[i, j, k, l, m, n, o, p, q] = readElement(bufferReader);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return array;
    }
}