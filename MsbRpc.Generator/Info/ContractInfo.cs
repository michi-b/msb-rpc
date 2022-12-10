﻿using System.CodeDom.Compiler;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;

namespace MsbRpc.Generator.Info;

public partial class ContractInfo : IEquatable<ContractInfo>
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

    public string GenerateServerInterface(ContractNames names)
    {
        var writer = new IndentedTextWriter(new StringWriter());

        writer.WriteFileHeader(names.GeneratedNamespace);
        
        writer.WriteLine("public interface {0}", names.ServerInterfaceName);

        using (writer.EncloseInBlock())
        {
            foreach (ProcedureInfo procedure in Procedures)
            {
                procedure.GenerateInterface(writer);
            }
        }

        string result = writer.InnerWriter.ToString();
        return result;
    }

    public ContractNames CreateNames() => new(Name, Namespace);
}