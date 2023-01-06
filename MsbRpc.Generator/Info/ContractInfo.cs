using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Attributes;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Info;

internal readonly struct ContractInfo : IEquatable<ContractInfo>
{
    public readonly string InterfaceName;
    public readonly string Namespace;
    public readonly ImmutableArray<ProcedureInfo> Procedures;
    public readonly RpcContractType ContractType;

    private ContractInfo
    (
        string interfaceName,
        string namespaceName,
        ImmutableArray<ProcedureInfo> procedures,
        RpcContractType contractType
    )
    {
        InterfaceName = interfaceName;
        Namespace = namespaceName;
        Procedures = procedures;
        ContractType = contractType;
    }

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public static ContractInfo? Parse(INamedTypeSymbol contract)
    {
        AttributeData? attributes = (from attributeData in contract.GetAttributes()
            let attributeClass = attributeData.AttributeClass
            where TypeCheck.IsRpcContractAttribute(attributeClass)
            select attributeData).FirstOrDefault();

        if (attributes == null)
        {
            return null;
        }

        RpcContractType? contractType = null;

        foreach (KeyValuePair<string, TypedConstant> argument in attributes.GetArguments())
        {
            // ReSharper disable once ConvertSwitchStatementToSwitchExpression
            switch (argument.Key)
            {
                case "contractType":
                    contractType = (RpcContractType)argument.Value.Value!;
                    break;
            }
        }

        if (contractType == null)
        {
            return null;
        }

        string interfaceName = contract.Name;
        string namespaceName = contract.ContainingNamespace.ToDisplayString();
        ImmutableArray<ProcedureInfo> procedures = contract.GetMembers()
            .Select(m => m as IMethodSymbol)
            .Where(m => m != null)
            .Select(m => new ProcedureInfo(m!))
            .ToImmutableArray();

        if (procedures.Length == 0)
        {
            return null;
        }

        return new ContractInfo(interfaceName, namespaceName, procedures, contractType.Value);
    }

    public bool Equals(ContractInfo other)
        => InterfaceName == other.InterfaceName
           && Namespace == other.Namespace
           && Procedures.Equals(other.Procedures)
           && ContractType == other.ContractType;

    public override bool Equals(object? obj) => obj is ContractInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = InterfaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            hashCode = (hashCode * 397) ^ Procedures.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)ContractType;
            return hashCode;
        }
    }
}