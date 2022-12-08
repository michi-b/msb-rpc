using System.CodeDom.Compiler;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public class ContractInfo
{
    private string Name { get; }
    private string Namespace { get; }
    private ImmutableArray<ProcedureInfo> Procedures { get; }

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

    public void Generate(SourceProductionContext context)
    {
        var writer = new IndentedTextWriter(Console.Out);
        writer.WriteLine($"Generating for rpc contract {Name} in namespace {Namespace} which has {Procedures.Length} procedures:");

        //output procedures
        writer.Indent++;
        foreach (ProcedureInfo procedure in Procedures)
        {
            writer.WriteLine($"Procedure {procedure.Name} has {procedure.Parameters.Length} parameters:");

            writer.Indent++;
            foreach (ParameterInfo parameter in procedure.Parameters)
            {
                writer.WriteLine($"Parameter {parameter.Name} has type {parameter.Type}");
            }

            writer.Indent--;
        }

        writer.Indent--;
    }

    public class SourceSymbolComparer : IEqualityComparer<ContractInfo>
    {
        public static SourceSymbolComparer Instance { get; } = new();

        public bool Equals(ContractInfo x, ContractInfo y)
            => x.Name == y.Name
               && x.Namespace == y.Namespace;

        public int GetHashCode(ContractInfo obj)
        {
            unchecked
            {
                int hashCode = obj.Name.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.Namespace.GetHashCode();
                return hashCode;
            }
        }
    }
}