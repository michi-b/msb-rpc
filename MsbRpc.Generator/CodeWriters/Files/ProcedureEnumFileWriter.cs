using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ProcedureEnumFileWriter : CodeFileWriter
{
    protected override string FileName { get; }

    private ProcedureCollection Procedures { get; }

    public ProcedureEnumFileWriter(ContractNode contract, ProcedureCollection procedures)
        : base(contract)
    {
        Procedures = procedures;
        FileName = $"{Procedures.Names.EnumType}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        string GetEnumMemberDefinition(int i) => $"{Procedures[i].Names.Name} = {Procedures[i].EnumValueString}";

        writer.WriteLine($"public enum {Procedures.Names.EnumType}");
        using (writer.InBlock(Appendix.None))
        {
            for (int i = 0; i < Procedures.LastIndex; i++)
            {
                writer.WriteLine($"{GetEnumMemberDefinition(i)},");
            }

            writer.WriteLine(GetEnumMemberDefinition(Procedures.LastIndex));
        }
    }
}