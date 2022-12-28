using System;

namespace MsbRpc.Serialization.Exceptions;

public class ByteArraySegmentHasNoArrayException : NullReferenceException
{
    public ByteArraySegmentHasNoArrayException(string paramName)
        : base($"The byte array segment {paramName} has no array.") { }
}