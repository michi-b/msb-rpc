using System.CodeDom.Compiler;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization.Serializations;

public sealed class UnresolvedSerialization : ISerialization
{
    private readonly string _declarationSyntax;

    public UnresolvedSerialization(TypeReferenceInfo typeReferenceInfo) => _declarationSyntax = typeReferenceInfo.GetDeclarationSyntax();

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write("0");
    }

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression) { }

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.Write("default!");
    }

    public bool GetIsVoid() => false;

    public bool GetIsResolved() => false;
    public string GetDeclarationSyntax() => _declarationSyntax;
}