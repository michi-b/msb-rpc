using System;

namespace MsbRpc.Generator.Enums;

public static class ConnectionEndTypeExtensions
{
    public static string GetName(this ConnectionEndType target)
    {
        return target switch
        {
            ConnectionEndType.Client => "Client",
            ConnectionEndType.Server => "Server",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}