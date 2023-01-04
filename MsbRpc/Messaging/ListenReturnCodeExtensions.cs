using System;

namespace MsbRpc.Messaging;

public static class ListenReturnCodeExtensions
{
    public static string GetName(this ListenReturnCode target)
        => target switch
        {
            ListenReturnCode.ConnectionClosed => nameof(ListenReturnCode.ConnectionClosed),
            ListenReturnCode.OperationDiscontinued => nameof(ListenReturnCode.OperationDiscontinued),
            ListenReturnCode.ConnectionDisposed => nameof(ListenReturnCode.ConnectionDisposed),
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
}