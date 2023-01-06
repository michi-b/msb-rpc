using System;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.Enums;

internal static class EndPointTypeIdExtensions
{
    public static string GetPostfix(this EndPointType target)
    {
        return target switch
        {
            EndPointType.InboundClient => ClientPostfix,
            EndPointType.InboundServer => ServerPostfix,
            EndPointType.OutboundClient => ClientPostfix,
            EndPointType.OutboundServer => ServerPostfix,
            EndPointType.Server => ServerPostfix,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}