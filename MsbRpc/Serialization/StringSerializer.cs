using System;
using System.Diagnostics;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization;

public static class StringSerializer
{
    public static int GetSize(string target) => target.Length * PrimitiveSerializer.CharSize;

    public static void Serialize(string value, ArraySegment<byte> buffer)
    {
        Debug.Assert(buffer.Count == value.Length * PrimitiveSerializer.CharSize);

        for (int i = 0; i < value.Length; i++)
        {
            buffer.WriteChar(value[i], i * PrimitiveSerializer.CharSize);
        }
    }

    public static string DeserializeString(ArraySegment<byte> buffer)
    {
        int count = buffer.Count / PrimitiveSerializer.CharSize;
        char[] characters = new char[count];
        for (int i = 0; i < count; i++)
        {
            characters[i] = buffer.ReadChar(i * PrimitiveSerializer.CharSize);
        }

        return new string(characters);
    }
}