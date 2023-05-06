using System.CodeDom.Compiler;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization.Serializations.Abstract;

public abstract class CustomSerialization : ISerialization
{
    private readonly string _declarationSyntax;

    protected CustomSerialization(TypeReferenceInfo typeReferenceInfo) => _declarationSyntax = typeReferenceInfo.GetDeclarationSyntax();

    public abstract void WriteSizeExpression(IndentedTextWriter writer, string targetExpression);
    public abstract void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression);
    public abstract void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression);

    public bool GetIsVoid() => false;

    public virtual bool GetIsResolved() => true;

    public string GetDeclarationSyntax() => _declarationSyntax;
}