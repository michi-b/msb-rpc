using System;

namespace MsbRpc.Generator.Enums;

public static class EndPointTypeExtensions
{
    public static string GetName(this EndPointType target)
    {
        return target switch
        {
            EndPointType.Client => "Client",
            EndPointType.Server => "Server",
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}