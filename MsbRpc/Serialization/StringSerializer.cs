﻿#region

using System;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

#endregion

namespace MsbRpc.Serialization;

public static class StringSerializer
{
    public static int GetSize(string value)
    {
        int count = value.Length;
        return PrimitiveSerializer.IntSize + count * PrimitiveSerializer.CharSize;
    }

    public static void Write(ref this BufferWriter writer, string value)
    {
        int count = value.Length;
        writer.Write(count);
        ArraySegment<byte> segment = writer.GetWriteSegment(count * PrimitiveSerializer.CharSize);
        for (int i = 0; i < count; i++)
        {
            segment.WriteChar(value[i], i * PrimitiveSerializer.CharSize);
        }
    }

    public static string ReadString(ref this BufferReader reader)
    {
        int count = reader.ReadInt();

        return reader.ReadString(count);
    }

    private static string ReadString(ref this BufferReader reader, int count)
    {
        ArraySegment<byte> segment = reader.ReadSegment(count * PrimitiveSerializer.CharSize);
        char[] characters = new char[count];
        for (int i = 0; i < count; i++)
        {
            characters[i] = segment.ReadChar(i * PrimitiveSerializer.CharSize);
        }

        return new string(characters);
    }
}