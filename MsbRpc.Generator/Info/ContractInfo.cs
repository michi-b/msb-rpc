﻿using System;
using System.Collections.Immutable;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Generator.Info;

internal readonly struct ContractInfo : IEquatable<ContractInfo>
{
    public readonly string InterfaceName;
    public readonly string Namespace;
    public readonly ImmutableArray<ProcedureInfo> Procedures;
    public readonly RpcContractType ContractType;
    public readonly ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> CustomSerializations;

    public ContractInfo
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
        CustomSerializations = ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo>.Empty;
    }

    public ContractInfo WithCustomSerializations(ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> customSerializations) => new(this, customSerializations);

    private ContractInfo(ContractInfo other, ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> customSerializations)
    {
        InterfaceName = other.InterfaceName;
        Namespace = other.Namespace;
        Procedures = other.Procedures;
        ContractType = other.ContractType;
        CustomSerializations = customSerializations;
    }

    public bool Equals(ContractInfo other)
        => InterfaceName == other.InterfaceName
           && Namespace == other.Namespace
           && Procedures.Equals(other.Procedures)
           && ContractType == other.ContractType
           && CustomSerializations.Equals(other.CustomSerializations);

    public override bool Equals(object? obj) => obj is ContractInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = InterfaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            hashCode = (hashCode * 397) ^ Procedures.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)ContractType;
            hashCode = (hashCode * 397) ^ CustomSerializations.GetHashCode();
            return hashCode;
        }
    }
}