#region

using System;

#endregion

namespace MsbRpc.Exceptions;

public class MaximumNumberOfRetriesExceededException : Exception
{
    public MaximumNumberOfRetriesExceededException(int maxRetries)
        : base($"Maximum number of retries ({maxRetries}) exceeded.") { }
}