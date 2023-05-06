using System;
using System.CodeDom.Compiler;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization.Serializations.Abstract;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Serialization.Serializations;

public sealed class NullableSerialization : GenericSerialization
{
    private const string ValueArgumentName = "innerValue";

    private readonly bool _canUseNullableAnnotationInsteadOfWrapper;

    private readonly ISerialization _innerValueSerialization;

    private NullableSerialization(ISerialization innerValueSerialization)
    {
        _innerValueSerialization = innerValueSerialization;
        if (innerValueSerialization is SimpleDefaultSerialization simpleDefaultSerialization)
        {
            _canUseNullableAnnotationInsteadOfWrapper = simpleDefaultSerialization.CanUseNullableAnnotationInsteadOfWrapper;
        }
    }

    public override void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write($"{Types.NullableSerializer}<{_innerValueSerialization.GetDeclarationSyntax()}>");
        writer.WriteLine($".{Methods.SerializerGetSize}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{targetExpression},");
            writer.Write($"({ValueArgumentName}) => ");
            _innerValueSerialization.WriteSizeExpression(writer, ValueArgumentName);
            writer.WriteLine();
        }
    }

    public override void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        throw new NotImplementedException();
    }

    public override void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        throw new NotImplementedException();
    }

    public override string GetDeclarationSyntax()
        => _canUseNullableAnnotationInsteadOfWrapper
            ? $"{_innerValueSerialization.GetDeclarationSyntax()}?"
            : $"{Types.Nullable}<{_innerValueSerialization.GetDeclarationSyntax()}>";

    public class Factory : IGenericSerializationFactory
    {
        public ISerialization Create
            (TypeReferenceInfo typeReference, SerializationResolver serializationResolver)
            => new NullableSerialization(serializationResolver.Resolve(typeReference.TypeArguments[0]));
    }
}