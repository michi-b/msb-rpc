using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public readonly ref struct IndentedTextWriterBlock
{
    private readonly IndentedTextWriter _writer;
    private readonly bool _withTrailingNewline;

    public IndentedTextWriterBlock(IndentedTextWriter writer, bool withTrailingNewline)
    {
        _writer = writer;
        _withTrailingNewline = withTrailingNewline;
        _writer.WriteBlockScopeStart();
        _writer.Indent++;
    }

    public void Dispose()
    {
        _writer.Indent--;
        _writer.WriteBlockScopeEnd(_withTrailingNewline);
    }
}