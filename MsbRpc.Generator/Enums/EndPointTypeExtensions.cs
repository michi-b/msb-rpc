using System;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Enums;

internal static class EndPointTypeExtensions
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

    public static EndPointDirection GetDirection(this EndPointType target)
    {
        return target switch
        {
            EndPointType.InboundClient => EndPointDirection.Inbound,
            EndPointType.InboundServer => EndPointDirection.Inbound,
            EndPointType.OutboundClient => EndPointDirection.Outbound,
            EndPointType.OutboundServer => EndPointDirection.Outbound,
            EndPointType.Server => EndPointDirection.Inbound,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}