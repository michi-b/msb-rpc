using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Serialization;

public interface ISerialization
{
    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression);

    /// <summary>
    ///     write statement to serialize the value, excluding the trailing semicolon and newline
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="bufferWriterExpression"></param>
    /// <param name="valueExpression"></param>
    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression);

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression);

    public bool GetIsVoid();

    public bool GetIsResolved();

    /// <summary>declaration syntax for a variable to store the target type, eg. "int?", "string", "MyStruct" etc.</summary>
    public string GetDeclarationSyntax();
}