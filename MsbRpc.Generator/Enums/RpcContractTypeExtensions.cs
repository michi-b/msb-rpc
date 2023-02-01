using System;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Generator.Enums;

internal static class RpcContractTypeExtensions
{
    public static EndPointDirection GetDirection(this RpcContractType contractType, ConnectionEndType connectionEndType)
    {
        return connectionEndType switch
        {
            ConnectionEndType.Client => contractType switch
            {
                RpcContractType.ClientToServer => EndPointDirection.Outbound,
                RpcContractType.ServerToClient => EndPointDirection.Inbound,
                _ => throw new ArgumentOutOfRangeException(nameof(contractType), contractType, null)
            },
            ConnectionEndType.Server => contractType switch
            {
                RpcContractType.ClientToServer => EndPointDirection.Inbound,
                RpcContractType.ServerToClient => EndPointDirection.Outbound,
                _ => throw new ArgumentOutOfRangeException(nameof(contractType), contractType, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(connectionEndType), connectionEndType, null)
        };
    }
}