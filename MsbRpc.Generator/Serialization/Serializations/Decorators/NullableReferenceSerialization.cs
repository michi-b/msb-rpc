using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.Serialization.Serializations.Decorators;

public class NullableReferenceSerialization : ISerialization
{
    private readonly ISerialization _nonNullableSerialization;

    public bool IsVoid => false;
    public bool IsResolved => _nonNullableSerialization.IsResolved;
    public bool IsConstantSize => false;
    public string DeclarationSyntax => $"{_nonNullableSerialization.DeclarationSyntax}?";

    public bool NeedsSemicolonAfterSerializationStatement => false;

    public NullableReferenceSerialization(ISerialization nonNullableSerialization) => _nonNullableSerialization = nonNullableSerialization;

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write($"({targetExpression} == null ? {Fields.PrimitiveSerializerBoolSize} : {Fields.PrimitiveSerializerBoolSize} + ");
        writer.WriteSerializationSizeExpression(_nonNullableSerialization, targetExpression);
        writer.Write(")");
    }

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        writer.WriteLine($"if ({valueExpression} == null)");

        using (writer.GetBlock())
        {
            writer.WriteLine($"{bufferWriterExpression}.{Methods.BufferWriterWrite}(false);");
        }

        writer.WriteLine("else");

        using (writer.GetBlock(Appendix.None))
        {
            writer.WriteLine($"{bufferWriterExpression}.{Methods.BufferWriterWrite}(true);");
            writer.WriteFinalizedSerializationStatement(_nonNullableSerialization, bufferWriterExpression, valueExpression);
        }
    }

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.Write($"({bufferReaderExpression}.{Methods.BufferReaderReadBool}() ? ");
        writer.WriteDeserializationExpression(_nonNullableSerialization, bufferReaderExpression);
        writer.Write(" : null)");
    }
}