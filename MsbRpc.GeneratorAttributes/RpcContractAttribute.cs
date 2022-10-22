using System;
using JetBrains.Annotations;

namespace MsbRpc.GeneratorAttributes;

public class RpcContractAttribute : Attribute
{
    [PublicAPI] public readonly string? ClientName;
    [PublicAPI] public readonly RpcDirection Direction;
    [PublicAPI] public readonly string? ServerName;

    public RpcContractAttribute
    (
        RpcDirection direction = RpcDirection.ClientToServer,
        string? serverName = null,
        string? clientName = null
    )
    {
        Direction = direction;
        ServerName = serverName;
        ClientName = clientName;
    }
}