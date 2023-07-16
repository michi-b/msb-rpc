#region

using System;
using MsbRpc.Attributes;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;
using static MsbRpc.Serialization.Buffers.BufferReader;
using static MsbRpc.Serialization.Buffers.BufferWriter;

#endregion

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
    public static int GetSize(TElement[,,,,,,,,] array, int constantElementSize)
        => PrimitiveSerializer.IntSize * 9 +
           array.GetLength(0)
           * array.GetLength(1)
           * array.GetLength(2)
           * array.GetLength(3)
           * array.GetLength(4)
           * array.GetLength(5)
           * array.GetLength(6)
           * array.GetLength(7)
           * array.GetLength(8)
           * constantElementSize;

    [MayBeUsedByGeneratedCode]
    public static void Write(ref BufferWriter writer, TElement[,,,,,,,,] array, WriteDelegate<TElement> writeElement)
    {
        writer.Write(array.GetLength(0));
        writer.Write(array.GetLength(1));
        writer.Write(array.GetLength(2));
        writer.Write(array.GetLength(3));
        writer.Write(array.GetLength(4));
        writer.Write(array.GetLength(5));
        writer.Write(array.GetLength(6));
        writer.Write(array.GetLength(7));
        writer.Write(array.GetLength(8));

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
                                            TElement element = array[i, j, k, l, m, n, o, p, q];
                                            writer.WriteCustom(element, writeElement);
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
    public static TElement[,,,,,,,,] Read(BufferReader reader, ReadDelegate<TElement> readElement)
    {
        int length0 = reader.ReadInt();
        int length1 = reader.ReadInt();
        int length2 = reader.ReadInt();
        int length3 = reader.ReadInt();
        int length4 = reader.ReadInt();
        int length5 = reader.ReadInt();
        int length6 = reader.ReadInt();
        int length7 = reader.ReadInt();
        int length8 = reader.ReadInt();

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
                                            TElement element = reader.ReadCustom(readElement);
                                            array[i, j, k, l, m, n, o, p, q] = element;
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