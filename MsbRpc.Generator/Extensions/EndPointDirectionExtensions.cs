using MsbRpc.EndPoints;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.Extensions;

public static class EndPointDirectionExtensions
{
    public static string GetEnumValueCode(this EndPointDirection target)
    {
        return target switch
        {
            EndPointDirection.Inbound => EnumValues.InboundEndPointDirection,
            EndPointDirection.Outbound => EnumValues.OutboundEndPointDirection,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
    }
}