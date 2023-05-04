using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class Serialization
{
    public delegate string GetSizeExpressionDelegate(string targetExpression);

    public delegate string GetSerializationStatementDelegate(string bufferWriterExpression, string valueExpression);

    public delegate string GetDeserializationExpressionDelegate(string bufferReaderExpression);

    private readonly GetDeserializationExpressionDelegate _getDeserializationExpression;
    private readonly GetSerializationStatementDelegate _getSerializationStatement;

    private readonly GetSizeExpressionDelegate _getSizeExpression;

    public Serialization(SimpleDefaultSerializationKind simpleDefaultSerializationKind, bool isNullable)
    {
        GetSizeExpressionDelegate getSizeExpression = simpleDefaultSerializationKind.GetSizeExpressionStringFactory();
        GetSerializationStatementDelegate getSerializationStatement = simpleDefaultSerializationKind.GetSerializationStatementStringFactory();
        GetDeserializationExpressionDelegate getDeserializationExpression = simpleDefaultSerializationKind.GetDeserializationExpressionStringFactory();
        _getSizeExpression = isNullable ? GetNullableSizeExpression(getSizeExpression) : getSizeExpression;
        _getSerializationStatement = isNullable ? GetNullableSerializationStatement(getSerializationStatement) : getSerializationStatement;
        _getDeserializationExpression = isNullable ? GetNullableDeserializationExpression(getDeserializationExpression) : getDeserializationExpression;
    }

    public Serialization(CustomSerializationNode customSerializationNode, bool isNullable)
    {
        GetSizeExpressionDelegate getSizeExpression = customSerializationNode.GetSizeExpression;
        GetDeserializationExpressionDelegate getDeserializationExpression = customSerializationNode.GetDeserializationExpression;
        GetSerializationStatementDelegate getSerializationStatement = customSerializationNode.GetSerializationStatement;
        _getSizeExpression = isNullable ? GetNullableSizeExpression(getSizeExpression) : getSizeExpression;
        _getDeserializationExpression = isNullable ? GetNullableDeserializationExpression(getDeserializationExpression) : getDeserializationExpression;
        _getSerializationStatement = isNullable ? GetNullableSerializationStatement(getSerializationStatement) : getSerializationStatement;
    }

    public string GetSizeExpression(string targetExpression) => _getSizeExpression(targetExpression);

    public string GetSerializationStatement(string bufferWriterExpression, string valueExpression) => _getSerializationStatement(bufferWriterExpression, valueExpression);

    public string GetDeserializationExpression(string bufferReaderExpression) => _getDeserializationExpression(bufferReaderExpression);

    private static GetSizeExpressionDelegate GetNullableSizeExpression(GetSizeExpressionDelegate innerDelegate)
        => targetExpression
            => $"{targetExpression} == null"
               + " ? " + Fields.PrimitiveSerializerBoolSize
               + " : " + innerDelegate(targetExpression) + " + " + Fields.PrimitiveSerializerBoolSize;

    private static GetDeserializationExpressionDelegate GetNullableDeserializationExpression(GetDeserializationExpressionDelegate innerDelegate)
        => bufferReaderExpression
            => $"{bufferReaderExpression}.{Methods.BufferReaderReadBool}()"
               + " ? " + innerDelegate(bufferReaderExpression)
               + " : null";

    private static GetSerializationStatementDelegate GetNullableSerializationStatement(GetSerializationStatementDelegate innerDelegate)
        => (bufferWriterExpression, valueExpression)
            => $"if ({valueExpression} == null) {{"
               + $"{bufferWriterExpression}.{Methods.BufferWriterWrite}(false);"
               + "} else {"
               + $"{bufferWriterExpression}.{Methods.BufferWriterWrite}(true); "
               + innerDelegate(bufferWriterExpression, valueExpression + ".Value")
               + "}";
}