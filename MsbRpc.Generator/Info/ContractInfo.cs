using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.GenerationHelpers;

namespace MsbRpc.Generator.Info;

public readonly struct ContractInfo : IEquatable<ContractInfo>
{
    public string InterfaceName { get; }
    public string Namespace { get; }

    private readonly EndPointInfo _client;
    private readonly EndPointInfo _server;

    public EndPointInfo this[EndPointId endPoint]
        => endPoint switch
        {
            EndPointId.Client => _client,
            EndPointId.Server => _server,
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

        _server = new EndPointInfo(serverProcedures);

        //todo: fill in correct client procedures
        _client = new EndPointInfo(ImmutableArray<ProcedureInfo>.Empty);
    }

    public bool Equals
        (ContractInfo other)
        => _client.Equals(other._client) && _server.Equals(other._server) && InterfaceName == other.InterfaceName && Namespace == other.Namespace;

    public override bool Equals(object? obj) => obj is ContractInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = _client.GetHashCode();
            hashCode = (hashCode * 397) ^ _server.GetHashCode();
            hashCode = (hashCode * 397) ^ InterfaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ Namespace.GetHashCode();
            return hashCode;
        }
    }
}