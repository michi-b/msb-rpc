using System.CodeDom.Compiler;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Extensions;

public static class IndentedTextWriterExtensions
{
    public static string GetResult(this IndentedTextWriter writer) => writer.InnerWriter.ToString();

    public static ParenthesesBlockScope InParenthesesBlock(this IndentedTextWriter writer, Appendix additions = Appendix.NewLine)
        => new(writer, additions);

    public static BlockScope InBlock(this IndentedTextWriter writer, Appendix additions = Appendix.NewLine) => new(writer, additions);
}