using System;
using JetBrains.Annotations;

namespace MsbRpc.Generator.Attributes
{
    public class RpcContractAttribute : Attribute
    {
        [PublicAPI] public readonly RpcDirection InitialDirection;

        public RpcContractAttribute(RpcDirection initialDirection = RpcDirection.ClientToServer) => InitialDirection = initialDirection;
    }
}

