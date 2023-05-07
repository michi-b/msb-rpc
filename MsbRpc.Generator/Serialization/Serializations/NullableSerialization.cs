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

    private readonly string _serializerName;

    private NullableSerialization(ISerialization innerValueSerialization)
    {
        _innerValueSerialization = innerValueSerialization;
        if (innerValueSerialization is SimpleDefaultSerialization simpleDefaultSerialization)
        {
            _canUseNullableAnnotationInsteadOfWrapper = simpleDefaultSerialization.CanUseNullableAnnotationInsteadOfWrapper;
        }

        _serializerName = $"{Types.NullableSerializer}<{_innerValueSerialization.GetDeclarationSyntax()}>";
    }

    public override void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.WriteLine($"{_serializerName}.{Methods.SerializerGetSize}");

        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{targetExpression},");

            writer.Write($"({ValueArgumentName}) => ");
            writer.WriteSerializationSizeExpression(_innerValueSerialization, ValueArgumentName);
            writer.WriteLine();
        }
    }

    public override void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        writer.WriteLine($"{_serializerName}.{Methods.SerializerWrite}");
        using (writer.GetParenthesesBlock(Appendix.SemicolonAndNewline))
        {
            writer.WriteLine($"ref {bufferWriterExpression},");
            writer.WriteLine($"{valueExpression},");

            writer.WriteLine($"({BufferWriterArgumentName}, {ValueArgumentName}) => ");
            using (writer.GetBlock())
            {
                writer.WriteSerializationStatement(_innerValueSerialization, BufferWriterArgumentName, ValueArgumentName);
                writer.WriteLine();
            }
        }
    }

    public override void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.WriteLine($"{_serializerName}.{Methods.SerializerRead}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"ref {bufferReaderExpression},");

            writer.WriteLine($"({BufferReaderArgumentName}) => ");
            using (writer.GetBlock())
            {
                writer.WriteDeserializationExpression(_innerValueSerialization, BufferReaderArgumentName);
                writer.WriteLine();
            }
        }
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