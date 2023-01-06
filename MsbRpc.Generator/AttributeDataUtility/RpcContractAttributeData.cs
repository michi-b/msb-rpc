using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.AttributeData;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Generator.AttributeDataUtility;

public struct RpcContractAttributeData : IEquatable<RpcContractAttributeData>
{
    public RpcContractType ContractType;

    public static bool TryParse(Microsoft.CodeAnalysis.AttributeData data, ref RpcContractAttributeData target)
    {
        RpcContractType? contractType = null;

        foreach (KeyValuePair<string, TypedConstant> argument in data.GetArguments())
        {
            switch (argument.Key)
            {
                case "contractType":
                    contractType = (RpcContractType)argument.Value.Value!;
                    break;
            }
        }

        if (contractType != null)
        {
            target.ContractType = contractType.Value;
            return true;
        }

        return false;
    }

    public bool Equals(RpcContractAttributeData other) => ContractType == other.ContractType;

    public override bool Equals(object? obj) => obj is RpcContractAttributeData other && Equals(other);

    public override int GetHashCode() => ContractType.GetHashCode();
}