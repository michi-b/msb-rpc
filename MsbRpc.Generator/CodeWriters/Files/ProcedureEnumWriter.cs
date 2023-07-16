#region

using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.GenerationTree;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.Names;

#endregion

namespace MsbRpc.Generator.CodeWriters.Files;

internal class ProcedureEnumWriter : CodeFileWriter
{
    protected override string FileName { get; }

    private ProcedureCollectionNode Procedures { get; }

    public ProcedureEnumWriter(ProcedureCollectionNode procedures)
        : base(procedures.Contract)
    {
        Procedures = procedures;
        FileName = $"{Procedures.ProcedureEnumName}{GeneratedFilePostfix}";
    }

    protected override void Write(IndentedTextWriter writer)
    {
        string GetEnumMemberDefinition(int i) => $"{Procedures[i].Name} = {Procedures[i].ProcedureEnumIntValue}";

        writer.WriteLine($"{Contract.AccessibilityKeyword} enum {Procedures.ProcedureEnumName}");
        using (writer.GetBlock(Appendix.None))
        {
            if (Procedures.Length > 0)
            {
                for (int i = 0; i < Procedures.LastIndex; i++)
                {
                    writer.WriteLine($"{GetEnumMemberDefinition(i)},");
                }

                writer.WriteLine(GetEnumMemberDefinition(Procedures.LastIndex));
            }
        }
    }
}