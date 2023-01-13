using System;

namespace MsbRpc.Exceptions;

public class EndPointRanToCompletionException : ObjectDisposedException
{
    public EndPointRanToCompletionException(string objectName)
        : base(objectName, "Endpoint has already run to completion and is therefore inoperable.") { }
}