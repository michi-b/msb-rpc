using System;

namespace MsbRpc.Serialization.Exceptions;

public class OutOfByteArraySegmentBoundsException : ArgumentOutOfRangeException
{
    public OutOfByteArraySegmentBoundsException(ArraySegment<byte> segment, int offset, int count)
        : base($"Tried to access {count} bytes starting at {offset} in a byte array of length {segment.Count}") { }
}