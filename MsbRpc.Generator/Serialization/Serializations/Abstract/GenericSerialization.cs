using System.CodeDom.Compiler;

namespace MsbRpc.Generator.Serialization.Serializations.Abstract;

public abstract class GenericSerialization : ISerialization
{
    protected const string BufferWriterArgumentName = "writer";
    protected const string BufferReaderArgumentName = "reader";

    public bool IsVoid => false;
    public abstract bool IsResolved { get; }
    public abstract bool IsConstantSize { get; }
    public abstract string DeclarationSyntax { get; }
    public virtual bool NeedsSemicolonAfterSerializationStatement => true;
    public abstract void WriteSizeExpression(IndentedTextWriter writer, string targetExpression);
    public abstract void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression);
    public abstract void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression);
}