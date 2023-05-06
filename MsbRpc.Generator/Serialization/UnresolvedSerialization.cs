using System.CodeDom.Compiler;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization;

public class UnresolvedSerialization : NonKeywordSerialization
{
    public UnresolvedSerialization(TypeReferenceInfo typeReferenceInfo) : base(typeReferenceInfo) { }

    public override void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write("0");
    }

    public override void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression) { }

    public override void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.Write("default!");
    }
    
    public override bool GetIsResolved() => false;
}