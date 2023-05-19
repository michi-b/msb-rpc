using System.CodeDom.Compiler;
using MsbRpc.Generator.Extensions;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization.Serializations.Abstract;
using MsbRpc.Generator.Utility;
using static MsbRpc.Generator.Utility.IndependentNames;

namespace MsbRpc.Generator.Serialization.Serializations;

public sealed class NullableSerialization : GenericSerialization
{
    private const string ValueArgumentName = "innerValue";

    private readonly bool _canUseNullableAnnotationInsteadOfWrapper;

    private readonly ISerialization _innerValueSerialization;

    private readonly string _serializerName;

    public override bool InnerValueIsConstantSize => _innerValueSerialization.InnerValueIsConstantSize;

    public override string DeclarationSyntax
        => _canUseNullableAnnotationInsteadOfWrapper
            ? $"{_innerValueSerialization.DeclarationSyntax}?"
            : $"{Types.Nullable}<{_innerValueSerialization.DeclarationSyntax}>";

    private NullableSerialization(ISerialization innerValueSerialization)
    {
        _innerValueSerialization = innerValueSerialization;
        if (innerValueSerialization is SimpleDefaultSerialization simpleDefaultSerialization)
        {
            _canUseNullableAnnotationInsteadOfWrapper = simpleDefaultSerialization.CanUseNullableAnnotationInsteadOfWrapper;
        }

        _serializerName = $"{Types.NullableSerializer}<{_innerValueSerialization.DeclarationSyntax}>";
    }

    public override void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.WriteLine($"{_serializerName}.{Methods.SerializerGetSize}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"{targetExpression},");

            //pass lambda to write size of inner value if it is not constant size, otherwise pass the constant size (different overload)
            if (!InnerValueIsConstantSize)
            {
                writer.Write($"({ValueArgumentName}) => ");
            }

            writer.WriteSerializationSizeExpression(_innerValueSerialization, ValueArgumentName);
            writer.WriteLine();
        }
    }

    public override void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        writer.WriteLine($"{_serializerName}.{Methods.SerializerWrite}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"ref {bufferWriterExpression},");
            writer.WriteLine($"{valueExpression},");

            writer.WriteLine($"({BufferWriterArgumentName}, {ValueArgumentName}) => ");
            using (writer.GetBlock())
            {
                writer.WriteFinalizedSerializationStatement(_innerValueSerialization, BufferWriterArgumentName, ValueArgumentName);
            }
        }
    }

    public override void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.WriteLine($"{_serializerName}.{Methods.SerializerRead}");
        using (writer.GetParenthesesBlock(Appendix.None))
        {
            writer.WriteLine($"ref {bufferReaderExpression},");
            writer.Write($"({BufferReaderArgumentName}) => ");
            writer.WriteDeserializationExpression(_innerValueSerialization, BufferReaderArgumentName);
        }
    }

    public class Factory : IGenericSerializationFactory
    {
        public ISerialization Create
            (TypeReferenceInfo typeReference, SerializationResolver serializationResolver)
            => new NullableSerialization(serializationResolver.Resolve(typeReference.TypeArguments[0]));
    }
}