using System;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Generator.Enums;

internal static class RpcContractTypeExtensions
{
    public static EndPointDirection GetDirection(this RpcDirection direction, EndPointType endPointType)
    {
        return endPointType switch
        {
            EndPointType.Client => direction switch
            {
                RpcDirection.ClientToServer => EndPointDirection.Outbound,
                RpcDirection.ServerToClient => EndPointDirection.Inbound,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            },
            EndPointType.Server => direction switch
            {
                RpcDirection.ClientToServer => EndPointDirection.Inbound,
                RpcDirection.ServerToClient => EndPointDirection.Outbound,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(endPointType), endPointType, null)
        };
    }
}