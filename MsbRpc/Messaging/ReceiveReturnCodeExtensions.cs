using System;

namespace MsbRpc.Messaging;

public static class ReceiveReturnCodeExtensions
{
    public static string GetName(this ReceiveReturnCode target)
    {
        return target switch
        {
            ReceiveReturnCode.Success => nameof(ReceiveReturnCode.Success),
            ReceiveReturnCode.ConnectionClosed => nameof(ReceiveReturnCode.ConnectionClosed),
            ReceiveReturnCode.ConnectionDisposed => nameof(ReceiveReturnCode.ConnectionDisposed),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}