using System;
using System.Diagnostics;

namespace MsbRpc.Serialization.Buffers;

public class RpcBuffer
{
    private byte[] _bytes;

    public RpcBuffer(int count = BufferUtility.DefaultInitialSize)
    {
        Debug.Assert(count >= 0);
        _bytes = count == 0 ? ByteArrayUtility.Empty : new byte[count];
    }

    public Request GetRequest(int count, int id)
    {
        Fit(count + Request.Offset);
        return new Request(_bytes, count, id);
    }

    public Message GetMessage(int count)
    {
        Fit(count + Message.Offset);
        return new Message(_bytes, count);
    }

    public ArraySegment<byte> Get(int count)
    {
        Fit(count);
        return new ArraySegment<byte>(_bytes, 0, count);
    }

    private void Fit(int length)
    {
        if (_bytes.Length < length)
        {
            _bytes = new byte[length];
        }
    }
}