using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Serialization.Default.Generic;

public class NullableSerialization : GenericSerialization
{
    private readonly ISerialization _valueSerialization;

    private NullableSerialization(ISerialization valueSerialization) => _valueSerialization = valueSerialization;

    public override void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write($"{Types.NullableSerializer}<{_valueSerialization.GetDeclarationSyntax()}>");
        writer.Write($".{Methods.SerializerGetSize}({targetExpression}, (value) => ");
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

    public override string GetDeclarationSyntax() => $"{Types.Nullable}<{_valueSerialization.GetDeclarationSyntax()}>";

    public class Factory : IGenericSerializationFactory
    {
        public ISerialization Create
            (TypeReferenceInfo typeReference, SerializationResolver serializationResolver)
            => new NullableSerialization(serializationResolver.Resolve(typeReference.TypeArguments[0]));
    }
}