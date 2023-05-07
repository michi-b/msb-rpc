using System;
using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Utility;

public readonly ref struct BlockScope
{
    private readonly IndentedTextWriter _writer;
    private readonly Appendix _appendix;

    public BlockScope(IndentedTextWriter writer, Appendix appendix = Appendix.NewLine)
    {
        _writer = writer;
        _appendix = appendix;

        Enter();
    }

    public void Dispose()
    {
        Exit();
    }

    private void Enter()
    {
        _writer.WriteLine("{");
        _writer.Indent++;
    }

    private void Exit()
    {
        _writer.Indent--;

        switch (_appendix)
        {
            case Appendix.None:
                _writer.Write("}");
                break;
            case Appendix.Semicolon:
                _writer.Write("};");
                break;
            case Appendix.NewLine:
                _writer.WriteLine("}");
                break;
            case Appendix.SemicolonAndNewline:
                _writer.WriteLine("};");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_appendix), _appendix, null);
        }
    }
}