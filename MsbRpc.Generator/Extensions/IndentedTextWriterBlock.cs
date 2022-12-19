using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public readonly ref struct IndentedTextWriterBlock
{
    private readonly IndentedTextWriter _writer;
    private readonly BlockOptions _options;

    public IndentedTextWriterBlock(IndentedTextWriter writer, BlockOptions options)
    {
        _writer = writer;
        _options = options;
        _writer.WriteBlockScopeStart();
        _writer.Indent++;
    }

    public void Dispose()
    {
        _writer.Indent--;
        _writer.WriteBlockScopeEnd(_options);
    }
}