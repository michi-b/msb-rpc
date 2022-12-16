using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Extensions;

public static class IndentedTextWriterExtensions
{
    public static IndentedTextWriterBlock EncloseInBlock(this IndentedTextWriter writer, bool withTrailingNewline = true) 
        => new(writer, withTrailingNewline);
    
    public static IndentedTextWriterParenthesesBlock EncloseInParenthesesBlock(this IndentedTextWriter writer, bool withTrailingNewline = true) 
        => new(writer, withTrailingNewline);

    public static string GetResult(this IndentedTextWriter writer) => writer.InnerWriter.ToString();

    public static void WriteBlockScopeStart(this IndentedTextWriter writer)
    {
        writer.WriteLine('{');
    }

    public static void WriteBlockScopeEnd(this IndentedTextWriter writer, bool withTrailingNewline)
    {
        if (withTrailingNewline)
        {
            writer.WriteLine('}');
        }
        else
        {
            writer.Write('}');
        }
    }
    
    public static void WriteParenthesesBlockScopeStart(this IndentedTextWriter writer)
    {
        writer.WriteLine('(');
    }
    
    public static void WriteParenthesesBlockScopeEnd(this IndentedTextWriter writer, bool withTrailingNewline)
    {
        if (withTrailingNewline)
        {
            writer.WriteLine(')');
        }
        else
        {
            writer.Write(')');
        }
    }
}