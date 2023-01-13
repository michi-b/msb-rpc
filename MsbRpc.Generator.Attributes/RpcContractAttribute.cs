using System;
using JetBrains.Annotations;

namespace MsbRpc.Generator.Attributes;

[GeneratorTarget]
public class RpcContractAttribute : Attribute
{
    [GeneratorTarget]
    public readonly RpcContractType ContractType;

    public RpcContractAttribute(RpcContractType contractType) => ContractType = contractType;
}