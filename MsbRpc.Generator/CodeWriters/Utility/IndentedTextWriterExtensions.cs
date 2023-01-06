using System.CodeDom.Compiler;

namespace MsbRpc.Generator.CodeWriters.Utility;

public static class IndentedTextWriterExtensions
{
    public static string GetResult(this IndentedTextWriter writer) => writer.InnerWriter.ToString();

    public static ParenthesesBlockScope GetParenthesesBlock(this IndentedTextWriter writer, Appendix additions = Appendix.NewLine)
        => new(writer, additions);

    public static BlockScope GetBlock(this IndentedTextWriter writer, Appendix additions = Appendix.NewLine) => new(writer, additions);
}