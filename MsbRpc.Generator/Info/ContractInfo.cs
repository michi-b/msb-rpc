using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public partial class ContractInfo : IEquatable<ContractInfo>
{
    public string Name { get; }
    public string Namespace { get; }
    public ImmutableArray<ProcedureInfo> Procedures { get; }

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public ContractInfo(INamedTypeSymbol interfaceSymbol)
    {
        Name = interfaceSymbol.Name;
        Namespace = interfaceSymbol.ContainingNamespace.ToDisplayString();
        Procedures = interfaceSymbol.GetMembers()
            .Select(m => m as IMethodSymbol)
            .Where(m => m != null)
            .Select(m => new ProcedureInfo(m!))
            .ToImmutableArray();
    }

    public bool Equals(ContractInfo other)
        => Name == other.Name
           && Namespace == other.Namespace
           && Procedures.SequenceEqual(other.Procedures);

    public override bool Equals(object obj) => Equals((ContractInfo)obj);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            hashCode = (hashCode * 397) ^ Procedures.GetHashCode();
            return hashCode;
        }
    }
}