using System.CodeDom.Compiler;
using MsbRpc.Generator.HelperTree;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.Writers;

internal class ProcedureEnumWriter : Writer
{
    protected override string FileName { get; }
    private ProcedureCollection Procedures { get; }
    
    public ProcedureEnumWriter(ContractNode contract, ProcedureCollection procedures)
        : base(contract)
    {
        Procedures = procedures!;
        FileName = $"{GeneratedNamespace}.{Procedures.Names.EnumType}{GeneratedFileEnding}";
    }

    protected override async ValueTask Write(IndentedTextWriter writer)
    {
        string GetEnumMemberDefinition(int i) => $"{Procedures[i].Names.Name} = {Procedures[i].EnumValueString}";

        await writer.WriteLineAsync($"public enum {Procedures.Names.EnumType}");
        
        await writer.WriteLineAsync('{');
        writer.Indent++;
        for (int i = 0; i < Procedures.LastIndex; i++)
        {
            await writer.WriteLineAsync($"{GetEnumMemberDefinition(i)},");
        }
        await writer.WriteLineAsync(GetEnumMemberDefinition(Procedures.LastIndex));

        writer.Indent--;
        await writer.WriteAsync('}');
    }
}