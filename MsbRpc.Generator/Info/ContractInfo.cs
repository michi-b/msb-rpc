﻿using System.CodeDom.Compiler;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace MsbRpc.Generator.Info;

public class ContractInfo : IEquatable<ContractInfo>
{
    public string Name { get; }
    public string Namespace { get; }
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

    public bool Equals(ContractInfo other) => Name == other.Name
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