using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;

namespace MsbRpc.Generator.Serialization.Default.Generic.Writers;

public class NullableSerialization : GenericSerialization
{
    private readonly ISerialization _valueSerialization;

    public NullableSerialization(ISerialization valueSerialization) => _valueSerialization = valueSerialization;

    public override void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write($"{IndependentNames.Types.NullableSerializer}<{_valueSerialization.GetDeclarationSyntax()}>");
        writer.Write($".{IndependentNames.Methods.SerializerGetSize}({targetExpression}, (value) => ");
        _valueSerialization.WriteSizeExpression(writer, "value");
        writer.Write(");");
    }

    public override void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        throw new NotImplementedException();
    }

    public override void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        throw new NotImplementedException();
    }

    public override string GetDeclarationSyntax() => throw new NotImplementedException();
}