using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.HelperTree;

namespace MsbRpc.Generator.CodeWriters;

public abstract class CodeWriter
{
    protected readonly string GeneratedNamespace;

    protected abstract string FileName { get; }

    protected CodeWriter(ContractNode contract) : this(contract.Names.GeneratedNamespace) { }

    private CodeWriter(string generatedNamespace) => GeneratedNamespace = generatedNamespace;

    public async Task GenerateAsync(SourceProductionContext context)
    {
        IndentedTextWriter writer = CreateCodeWriter();
        await WriteAsync(writer);
        string code = writer.GetResult();
        context.AddSource(FileName, SourceText.From(code, Encoding.UTF8));
    }

    protected abstract ValueTask WriteAsync(IndentedTextWriter writer);

    private IndentedTextWriter CreateCodeWriter()
    {
        IndentedTextWriter writer = new(new StringWriter());
        writer.WriteFileHeader(GeneratedNamespace);
        return writer;
    }

    protected static string GetArgumentOutOfRangeExceptionSwitchExpressionCase(string variableName)
        => $"_ => throw new {IndependentNames.Types.ArgumentOutOfRangeException}(nameof({variableName}), {variableName}, null)";
}