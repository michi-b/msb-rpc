using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.GenerationTree;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ProcedureEnumFileWriter : CodeFileWriter
{
    protected override string FileName { get; }

    private ProcedureCollectionNode Procedures { get; }

    public ProcedureEnumFileWriter(ProcedureCollectionNode procedures)
        : base(procedures.Contract)
    {
        Procedures = procedures;
        FileName = $"{Procedures.EnumName}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        string GetEnumMemberDefinition(int i) => $"{Procedures[i].Name} = {Procedures[i].IntValueString}";

        writer.WriteLine($"public enum {Procedures.EnumName}");
        using (writer.GetBlock(Appendix.None))
        {
            for (int i = 0; i < Procedures.LastIndex; i++)
            {
                writer.WriteLine($"{GetEnumMemberDefinition(i)},");
            }

            writer.WriteLine(GetEnumMemberDefinition(Procedures.LastIndex));
        }
    }
}