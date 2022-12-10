using System.CodeDom.Compiler;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;

namespace MsbRpc.Generator.Info;

public readonly struct ProcedureInfo : IEquatable<ProcedureInfo>
{
    public string Name { get; }
    public ImmutableArray<ParameterInfo> Parameters { get; }

    public TypeInfo ReturnType { get; }

    public ProcedureInfo(IMethodSymbol method)
    {
        Name = method.Name;
        ImmutableArray<IParameterSymbol> parameters = method.Parameters;
        Parameters = parameters.Select(parameter => new ParameterInfo(parameter)).ToImmutableArray();
        ReturnType = new TypeInfo(method.ReturnType);
    }

    public bool Equals(ProcedureInfo other)
        => Name == other.Name
           && Parameters.SequenceEqual(other.Parameters)
           && ReturnType.Equals(other.ReturnType);

    public override bool Equals(object? obj) => obj is ProcedureInfo other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Name.GetHashCode() * 397) ^ Parameters.GetHashCode() ^ ReturnType.GetHashCode();
        }
    }

    public void GenerateInterface(IndentedTextWriter writer)
    {
        writer.Write($"{ReturnType.FullName} {Name}");

        using (writer.EncloseInParentheses())
        {
            if (Parameters.Length > 0)
            {
                Parameters[0].GenerateInterface(writer);
                for (int i = 1; i < Parameters.Length; i++)
                {
                    writer.WriteCommaDelimiter();
                    Parameters[i].GenerateInterface(writer);
                }
            }
        }

        writer.WriteLineSemicolon();
    }
}