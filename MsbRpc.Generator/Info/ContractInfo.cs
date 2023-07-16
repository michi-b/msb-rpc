#region

using System;
using System.Collections.Immutable;
using MsbRpc.Generator.Attributes;
using MsbRpc.Generator.Enums;

#endregion

namespace MsbRpc.Generator.Info;

internal readonly struct ContractInfo : IEquatable<ContractInfo>
{
    public readonly string InterfaceName;
    public readonly string Namespace;
    public readonly ImmutableArray<ProcedureInfo> Procedures;
    public readonly RpcDirection Direction;
    public readonly int InitialBufferSize;
    public readonly ServerGenerationInfo? ServerGeneration;
    public readonly ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo> CustomSerializations;
    public readonly ContractAccessibility Accessibility;

    public ContractInfo
    (
        string interfaceName,
        string namespaceName,
        ImmutableArray<ProcedureInfo> procedures,
        RpcDirection direction,
        ContractAccessibility contractAccessibility,
        int initialBufferSize,
        ServerGenerationInfo? serverGeneration
    )
    {
        InterfaceName = interfaceName;
        Namespace = namespaceName;
        Procedures = procedures;
        Direction = direction;
        CustomSerializations = ImmutableDictionary<TypeReferenceInfo, CustomSerializationInfo>.Empty;
        Accessibility = contractAccessibility;
        InitialBufferSize = initialBufferSize;
        ServerGeneration = serverGeneration;
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
        Direction = other.Direction;
        CustomSerializations = customSerializations;
        Accessibility = other.Accessibility;
        InitialBufferSize = other.InitialBufferSize;
        ServerGeneration = other.ServerGeneration;
    }

    public bool Equals(ContractInfo other)
        => InterfaceName == other.InterfaceName
           && Namespace == other.Namespace
           && Procedures.Equals(other.Procedures)
           && Direction == other.Direction
           && CustomSerializations.Equals(other.CustomSerializations)
           && Accessibility == other.Accessibility
           && InitialBufferSize == other.InitialBufferSize
           && Equals(ServerGeneration, other.ServerGeneration);

    public override bool Equals(object? obj) => obj is ContractInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = InterfaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            hashCode = (hashCode * 397) ^ Procedures.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Direction;
            hashCode = (hashCode * 397) ^ CustomSerializations.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Accessibility;
            hashCode = (hashCode * 397) ^ InitialBufferSize;
            hashCode = (hashCode * 397) ^ ServerGeneration.GetHashCode();
            return hashCode;
        }
    }
}