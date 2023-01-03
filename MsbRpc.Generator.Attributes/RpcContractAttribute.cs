using System;
using JetBrains.Annotations;

namespace MsbRpc.Generator.Attributes;

[MeansImplicitUse(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.WithMembers)]
public class RpcContractAttribute : Attribute
{
    [PublicAPI] public readonly RpcContractType ContractType;

    public RpcContractAttribute(RpcContractType contractType) => ContractType = contractType;
}