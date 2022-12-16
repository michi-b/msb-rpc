using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public readonly ref struct TextWriterParenthesesEnclosure
{
    private readonly IndentedTextWriter _writer;
    private readonly bool _withTrailingNewline;

    public TextWriterParenthesesEnclosure(IndentedTextWriter writer, bool withTrailingNewline)
    {
        _writer = writer;
        _withTrailingNewline = withTrailingNewline;
        _writer.Write('(');
    }

    public void Dispose()
    {
        if (_withTrailingNewline)
        {
            _writer.WriteLine(')');
        }
        else
        {
            _writer.Write(')');
        }
    }
}