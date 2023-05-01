using System.CodeDom.Compiler;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization;

public class SerializationWriter
{
    public delegate void WriteSizeExpressionDelegate(IndentedTextWriter writer, string targetExpression);

    public delegate void WriteSerializationStatementDelegate(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression);

    public delegate void WriteDeserializationExpressionDelegate(IndentedTextWriter writer, string bufferReaderExpression);

    private readonly WriteDeserializationExpressionDelegate _writeDeserializationExpression;

    private readonly WriteSerializationStatementDelegate _writeSerializationStatement;

    private readonly WriteSizeExpressionDelegate _writeSizeExpression;

    public SerializationWriter(DefaultSerializationKind serializationKind)
    {
        _writeSizeExpression = (writer, expression)
            => writer.Write(serializationKind.GetGetSizeExpression().Invoke(expression));
        _writeSerializationStatement = (writer, bufferWriterExpression, valueExpression)
            => writer.Write(serializationKind.GetGetSerializationStatement().Invoke(bufferWriterExpression, valueExpression));
        _writeDeserializationExpression = (writer, expression)
            => writer.Write(serializationKind.GetGetDeserializationExpression().Invoke(expression));
    }

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression) => _writeSizeExpression(writer, targetExpression);

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
        => _writeSerializationStatement(writer, bufferWriterExpression, valueExpression);

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
        => _writeDeserializationExpression(writer, bufferReaderExpression);
}