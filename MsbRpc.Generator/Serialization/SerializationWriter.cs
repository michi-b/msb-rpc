using System.CodeDom.Compiler;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization;

public readonly struct SerializationWriter
{
    public delegate void SizeExpressionFactory(IndentedTextWriter writer, string targetExpression);

    public delegate void SerializationStatementFactory(IndentedTextWriter writer, string bufferWriterExpression, string targetExpression);

    public delegate void DeserializationExpressionFactory(IndentedTextWriter writer, string bufferReaderExpression);

    private readonly DeserializationExpressionFactory _deserializationExpressionFactory;

    private readonly SerializationStatementFactory _serializationStatementFactory;

    private readonly SizeExpressionFactory _sizeExpressionFactory;

    public SerializationWriter(SimpleDefaultSerializationKind serializationKind)
    {
        _sizeExpressionFactory = (writer, expression)
            => writer.Write(serializationKind.GetSizeExpressionStringFactory().Invoke(expression));
        _serializationStatementFactory = (writer, bufferWriterExpression, valueExpression)
            => writer.Write(serializationKind.GetSerializationStatementStringFactory().Invoke(bufferWriterExpression, valueExpression));
        _deserializationExpressionFactory = (writer, expression)
            => writer.Write(serializationKind.GetDeserializationExpressionStringFactory().Invoke(expression));
    }

    public SerializationWriter
    (
        SizeExpressionFactory sizeExpressionFactory,
        SerializationStatementFactory serializationStatementFactory,
        DeserializationExpressionFactory deserializationExpressionFactory
    )
    {
        _sizeExpressionFactory = sizeExpressionFactory;
        _serializationStatementFactory = serializationStatementFactory;
        _deserializationExpressionFactory = deserializationExpressionFactory;
    }

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression) => _sizeExpressionFactory(writer, targetExpression);

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
        => _serializationStatementFactory(writer, bufferWriterExpression, valueExpression);

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
        => _deserializationExpressionFactory(writer, bufferReaderExpression);
}