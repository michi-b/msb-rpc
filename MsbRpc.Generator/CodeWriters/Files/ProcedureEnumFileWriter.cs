using System.CodeDom.Compiler;
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
        FileName = $"{GeneratedNamespace}.{Procedures.Names.EnumType}{GeneratedFilePostfix}";
    }

    protected override async ValueTask WriteAsync(IndentedTextWriter writer)
    {
        string GetEnumMemberDefinition(int i) => $"{Procedures[i].Names.Name} = {Procedures[i].EnumValueString}";

        await writer.WriteLineAsync($"public enum {Procedures.Names.EnumType}");
        await writer.EnterBlockAsync();
        {
            for (int i = 0; i < Procedures.LastIndex; i++)
            {
                await writer.WriteLineAsync($"{GetEnumMemberDefinition(i)},");
            }

            await writer.WriteLineAsync(GetEnumMemberDefinition(Procedures.LastIndex));
        }
        await writer.ExitBlockAsync(BlockAdditions.None);
    }
}