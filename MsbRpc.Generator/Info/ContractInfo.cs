using System;
using System.Collections.Immutable;
using MsbRpc.Generator.Attributes;
using MsbRpc.Generator.Enums;

namespace MsbRpc.Generator.Info;

public readonly struct ContractInfo : IEquatable<ContractInfo>
{
    public readonly string InterfaceName;
    public readonly string Namespace;
    public readonly ImmutableArray<ProcedureInfo> Procedures;
    public readonly RpcContractDirection ContractDirection;
    public readonly int InitialBufferSize;
    public readonly ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> CustomSerializations;
    public readonly ContractAccessibility Accessibility;

    public ContractInfo
    (
        string interfaceName,
        string namespaceName,
        ImmutableArray<ProcedureInfo> procedures,
        RpcContractDirection contractDirection,
        ContractAccessibility contractAccessibility,
        int initialBufferSize
    )
    {
        InterfaceName = interfaceName;
        Namespace = namespaceName;
        Procedures = procedures;
        ContractDirection = contractDirection;
        CustomSerializations = ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo>.Empty;
        Accessibility = contractAccessibility;
        InitialBufferSize = initialBufferSize;
    }

    public ContractInfo WithCustomSerializations(ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> customSerializations) => new(this, customSerializations);

    /// <summary>
    ///     Copy contract info with replaced custom serializations
    /// </summary>
    private ContractInfo(ContractInfo other, ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> customSerializations)
    {
        InterfaceName = other.InterfaceName;
        Namespace = other.Namespace;
        Procedures = other.Procedures;
        ContractDirection = other.ContractDirection;
        CustomSerializations = customSerializations;
        Accessibility = other.Accessibility;
        InitialBufferSize = other.InitialBufferSize;
    }

    public bool Equals(ContractInfo other)
        => InterfaceName == other.InterfaceName
           && Namespace == other.Namespace
           && Procedures.Equals(other.Procedures)
           && ContractDirection == other.ContractDirection
           && CustomSerializations.Equals(other.CustomSerializations)
           && Accessibility == other.Accessibility
           && InitialBufferSize == other.InitialBufferSize;

    public override bool Equals(object? obj) => obj is ContractInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = InterfaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            hashCode = (hashCode * 397) ^ Procedures.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)ContractDirection;
            hashCode = (hashCode * 397) ^ CustomSerializations.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Accessibility;
            hashCode = (hashCode * 397) ^ InitialBufferSize;
            return hashCode;
        }
    }
}