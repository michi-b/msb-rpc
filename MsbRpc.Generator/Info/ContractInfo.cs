using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.GenerationHelpers;

namespace MsbRpc.Generator.Info;

public struct ContractInfo : IEquatable<ContractInfo>
{
    public string InterfaceName { get; }
    public string Namespace { get; }

    public EndPointInfo Client;
    public EndPointInfo Server;

    public EndPointInfo this[EndPointId endPoint]
        => endPoint switch
        {
            EndPointId.Client => Client,
            EndPointId.Server => Server,
            _ => throw new ArgumentOutOfRangeException(nameof(endPoint), endPoint, null)
        };

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public ContractInfo(INamedTypeSymbol interfaceSymbol)
    {
        InterfaceName = interfaceSymbol.Name;
        Namespace = interfaceSymbol.ContainingNamespace.ToDisplayString();

        ImmutableArray<ProcedureInfo> serverProcedures = interfaceSymbol.GetMembers()
            .Select(m => m as IMethodSymbol)
            .Where(m => m != null)
            .Select(m => new ProcedureInfo(m!))
            .ToImmutableArray();

        Server = new EndPointInfo(serverProcedures);

        //todo: fill in correct client procedures
        Client = new EndPointInfo(ImmutableArray<ProcedureInfo>.Empty);
    }

    public bool Equals
        (ContractInfo other)
        => Client.Equals(other.Client) && Server.Equals(other.Server) && InterfaceName == other.InterfaceName && Namespace == other.Namespace;

    public override bool Equals(object? obj) => obj is ContractInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Client.GetHashCode();
            hashCode = (hashCode * 397) ^ Server.GetHashCode();
            hashCode = (hashCode * 397) ^ InterfaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            return hashCode;
        }
    }
}