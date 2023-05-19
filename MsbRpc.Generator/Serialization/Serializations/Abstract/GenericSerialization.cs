using System.CodeDom.Compiler;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Serialization.Serializations.Abstract;

public abstract class GenericSerialization : ISerialization
{
    protected const string BufferWriterArgumentName = IndependentNames.Variables.RequestWriter;
    protected const string BufferReaderArgumentName = IndependentNames.Variables.ResponseReader;

    public bool IsVoid => false;

    public virtual bool IsResolved => true;
    public abstract bool InnerValueIsConstantSize { get; }

    public abstract string DeclarationSyntax { get; }

    public virtual bool NeedsSemicolonAfterSerializationStatement => true;

    public abstract void WriteSizeExpression(IndentedTextWriter writer, string targetExpression);
    public abstract void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression);
    public abstract void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression);
}