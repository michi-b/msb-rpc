using System.CodeDom.Compiler;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator;

public readonly record struct RpcContract
{
    private string Name { get; }
    private ImmutableArray<RpcProcedure> Procedures { get; }

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public RpcContract(INamedTypeSymbol interfaceSymbol)
    {
        Name = interfaceSymbol.Name;
        Procedures = interfaceSymbol.GetMembers()
            .Select(m => m as IMethodSymbol)
            .Where(m => m != null)
            .Select(m => new RpcProcedure(m!))
            .ToImmutableArray();
    }

    public void Generate(SourceProductionContext context)
    {
        var writer = new IndentedTextWriter(Console.Out);
        writer.WriteLine($"Generating for rpc contract {Name} which has {Procedures.Length} procedures:");

        //output procedures
        writer.Indent++;
        foreach (RpcProcedure procedure in Procedures)
        {
            writer.WriteLine($"Procedure {procedure.Name} has {procedure.Parameters.Length} parameters:");

            writer.Indent++;
            foreach (RpcParameter parameter in procedure.Parameters)
            {
                writer.WriteLine($"Parameter {parameter.Name} has type {parameter.Type}");
            }

            writer.Indent--;
        }

        writer.Indent--;
    }
}