using System;
using System.Diagnostics;
using MsbRpc.Configuration.Builders;

namespace MsbRpc.Serialization.Buffers;

public class RpcBuffer
{
    private byte[] _bytes;

    public RpcBuffer(int count = EndPointConfigurationBuilder.DefaultInitialBufferSize)
    {
        Debug.Assert(count >= 0);
        _bytes = count == 0 ? ByteArrayUtility.Empty : new byte[count];
    }

    public Request GetRequest(int id, int count = 0)
    {
        Fit(count + Request.Offset);
        return new Request(_bytes, count, id);
    }

    public Response GetFaultedResponse(bool ranToCompletion)
        => new
        (
            _bytes,
            0,
            ranToCompletion
                ? ResponseFlags.RanToCompletion | ResponseFlags.Faulted
                : ResponseFlags.Faulted
        );

    public Response GetResponse(bool ranToCompletion, int count = 0)
    {
        Fit(count + Response.Offset);
        return new Response(_bytes, count, ranToCompletion ? ResponseFlags.RanToCompletion : ResponseFlags.None);
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