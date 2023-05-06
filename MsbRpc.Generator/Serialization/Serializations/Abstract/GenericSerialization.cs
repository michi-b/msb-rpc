using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Serialization.Serializations.Abstract;

public abstract class GenericSerialization : ISerialization
{
    protected GenericSerialization(){}

    public abstract void WriteSizeExpression(IndentedTextWriter writer, string targetExpression);
    public abstract void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression);
    public abstract void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression);

    public bool GetIsVoid() => false;

    public virtual bool GetIsResolved() => true;

    public abstract string GetDeclarationSyntax();
}