using System;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Generator.Enums;

internal static class RpcContractTypeExtensions
{
    public static EndPointDirection GetDirection(this RpcContractDirection contractDirection, ConnectionEndType connectionEndType)
    {
        return connectionEndType switch
        {
            ConnectionEndType.Client => contractDirection switch
            {
                RpcContractDirection.ClientToServer => EndPointDirection.Outbound,
                RpcContractDirection.ServerToClient => EndPointDirection.Inbound,
                _ => throw new ArgumentOutOfRangeException(nameof(contractDirection), contractDirection, null)
            },
            ConnectionEndType.Server => contractDirection switch
            {
                RpcContractDirection.ClientToServer => EndPointDirection.Inbound,
                RpcContractDirection.ServerToClient => EndPointDirection.Outbound,
                _ => throw new ArgumentOutOfRangeException(nameof(contractDirection), contractDirection, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(connectionEndType), connectionEndType, null)
        };
    }
}