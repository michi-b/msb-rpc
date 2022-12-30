using System;
using JetBrains.Annotations;

namespace MsbRpc.Generator.Attributes;

[MeansImplicitUse(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.WithMembers)]
public class RpcContractAttribute : Attribute
{
    [PublicAPI] public readonly RpcDirection InitialDirection;

    public RpcContractAttribute(RpcDirection initialDirection = RpcDirection.ClientToServer) => InitialDirection = initialDirection;
}