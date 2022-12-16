using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public readonly struct ContractInfo : IEquatable<ContractInfo>
{
    public string InterfaceName { get; }
    public string Namespace { get; }
    public ImmutableArray<ProcedureInfo> ServerProcedures { get; }
    
    public ImmutableArray<ProcedureInfo> ClientProcedures { get; }

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public ContractInfo(INamedTypeSymbol interfaceSymbol)
    {
        InterfaceName = interfaceSymbol.Name;
        Namespace = interfaceSymbol.ContainingNamespace.ToDisplayString();
        ServerProcedures = interfaceSymbol.GetMembers()
            .Select(m => m as IMethodSymbol)
            .Where(m => m != null)
            .Select(m => new ProcedureInfo(m!))
            .ToImmutableArray();
        
        //todo: fill in correct client procedures
        ClientProcedures = ImmutableArray<ProcedureInfo>.Empty;
    }

    public bool Equals(ContractInfo other)
        => InterfaceName == other.InterfaceName
           && Namespace == other.Namespace
           && ServerProcedures.SequenceEqual(other.ServerProcedures);

    public override bool Equals(object obj) => Equals((ContractInfo)obj);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = InterfaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            hashCode = (hashCode * 397) ^ ServerProcedures.GetHashCode();
            return hashCode;
        }
    }
}