using System.CodeDom.Compiler;
using MsbRpc.Generator.Serialization;

public abstract class GenericSerialization : ISerialization
{
    public abstract void WriteSizeExpression(IndentedTextWriter writer, string targetExpression);
    public abstract void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression);
    public abstract void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression);
    
    public string? GetKeyword() => null;

    public bool GetIsVoid() => false;

    public bool GetIsResolved() => true;
    public abstract string GetDeclarationSyntax();
}
