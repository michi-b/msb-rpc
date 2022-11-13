﻿using System.Diagnostics;

namespace MsbRpc.Serialization;

public class RecycledBuffer
{
    private byte[] _bytes;

    public RecycledBuffer(int count)
    {
        Debug.Assert(count >= 0);
        _bytes = count == 0 ? Memory.Empty : new byte[count];
    }

    public ArraySegment<byte> Get(int count)
    {
        if (count > _bytes.Length)
        {
            _bytes = new byte[count];
        }

        return new ArraySegment<byte>(_bytes, 0, count);
    }
}