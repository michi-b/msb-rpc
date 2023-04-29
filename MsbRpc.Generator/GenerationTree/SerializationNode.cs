using System;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class SerializationNode
{
    public delegate string GetSizeExpressionDelegate(string targetExpression);

    public delegate string GetSerializationStatementDelegate(string bufferWriterExpression, string valueExpression);

    public delegate string GetDeserializationExpressionDelegate(string bufferReaderExpression);

    private readonly GetDeserializationExpressionDelegate _getDeserializationExpression;
    private readonly GetSerializationStatementDelegate _getSerializationStatement;

    private readonly GetSizeExpressionDelegate _getSizeExpression;

    public SerializationNode(DefaultSerializationKind defaultSerializationKind)
    {
        GetSizeExpressionDelegate? getSizeExpression = defaultSerializationKind.GetGetSizeExpression();
        GetSerializationStatementDelegate? getSerializationStatement = defaultSerializationKind.GetGetSerializationStatement();
        GetDeserializationExpressionDelegate? getDeserializationExpression = defaultSerializationKind.GetGetDeserializationExpression();
        if (getSizeExpression != null && getSerializationStatement != null && getDeserializationExpression != null)
        {
            _getSizeExpression = getSizeExpression;
            _getSerializationStatement = getSerializationStatement;
            _getDeserializationExpression = getDeserializationExpression;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(defaultSerializationKind), defaultSerializationKind, "serialization kind is not serializable");
        }
    }

    public string GetSizeExpression(string targetExpression) => _getSizeExpression(targetExpression);

    public string GetSerializationStatement(string bufferWriterExpression, string valueExpression) => _getSerializationStatement(bufferWriterExpression, valueExpression);

    public string GetDeserializationExpression(string bufferReaderExpression) => _getDeserializationExpression(bufferReaderExpression);
}