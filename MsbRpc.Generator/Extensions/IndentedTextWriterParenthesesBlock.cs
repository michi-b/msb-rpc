using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public readonly ref struct IndentedTextWriterParenthesesBlock
{
    private readonly IndentedTextWriter _writer;
    private readonly bool _withTrailingNewline;

    public IndentedTextWriterParenthesesBlock(IndentedTextWriter writer, bool withTrailingNewline)
    {
        _writer = writer;
        _withTrailingNewline = withTrailingNewline;
        _writer.WriteParenthesesBlockScopeStart();
        _writer.Indent++;
    }

    public void Dispose()
    {
        _writer.Indent--;
        _writer.WriteParenthesesBlockScopeEnd(_withTrailingNewline);
    }
}