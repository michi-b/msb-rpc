using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.HelperTree;

namespace MsbRpc.Generator.Writers;

public abstract class Writer
{
    protected readonly string GeneratedNamespace;
    
    protected Writer(ContractNode contract) : this(contract.Names.GeneratedNamespace) {}

    private Writer(string generatedNamespace) => GeneratedNamespace = generatedNamespace;

    public async Task Generate(SourceProductionContext context)
    {
        IndentedTextWriter writer = CreateCodeWriter();
        await Write(writer);
        string code = writer.GetResult();
        context.AddSource(FileName, SourceText.From(code, Encoding.UTF8));
    }
    
    protected abstract string FileName { get; }
    
    protected abstract ValueTask Write(IndentedTextWriter writer);
    
    private IndentedTextWriter CreateCodeWriter()
    {
        IndentedTextWriter writer = new(new StringWriter());

        writer.WriteFileHeader(GeneratedNamespace);

        return writer;
    }
}