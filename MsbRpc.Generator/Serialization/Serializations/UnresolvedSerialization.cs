#region

using System.CodeDom.Compiler;
using MsbRpc.Generator.Info;

#endregion

namespace MsbRpc.Generator.Serialization.Serializations;

public sealed class UnresolvedSerialization : ISerialization
{
    public bool IsVoid => false;

    public bool IsResolved => false;
    public bool IsConstantSize => true;

    public string DeclarationSyntax { get; }

    public bool NeedsSemicolonAfterSerializationStatement => true;
    public UnresolvedSerialization(TypeReferenceInfo typeReferenceInfo) => DeclarationSyntax = typeReferenceInfo.GetDeclarationSyntax();

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write("0");
    }

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression) { }

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.Write("default!");
    }
}