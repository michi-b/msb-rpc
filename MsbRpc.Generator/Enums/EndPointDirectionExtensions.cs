using System;
using MsbRpc.Generator.CodeWriters.Utility;

namespace MsbRpc.Generator.Enums;

internal static class EndPointDirectionExtensions
{
    public static string GetEnumValueCode(this EndPointDirection target)
    {
        return target switch
        {
            EndPointDirection.Inbound => IndependentNames.EnumValues.InboundEndPointDirection,
            EndPointDirection.Outbound => IndependentNames.EnumValues.OutboundEndPointDirection,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}