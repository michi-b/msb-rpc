using System;
using System.Diagnostics;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization;

public static class StringSerializer
{
    public static int GetSize(this string target) => target.Length * PrimitiveSerializer.CharSize;

    public static void Serialize(this ArraySegment<byte> buffer, string value)
    {
        Debug.Assert(buffer.Count == value.Length * PrimitiveSerializer.CharSize);

        for (int i = 0; i < value.Length; i++)
        {
            buffer.WriteChar(value[i], i * PrimitiveSerializer.CharSize);
        }
    }

    public static string DeserializeString(this ArraySegment<byte> buffer)
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