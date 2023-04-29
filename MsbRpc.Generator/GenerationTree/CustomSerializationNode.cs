using System;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class CustomSerializationNode
{
    private const string StatementClose = ");";

    public SerializationNode.GetSizeExpressionDelegate CreateGetSerializedSizeExpression { get; }

    public SerializationNode.GetSerializationStatementDelegate CreateGetSerializationStatement { get; }

    public SerializationNode.GetDeserializationExpressionDelegate CreateGetDeserializationExpression { get; }

    public CustomSerializationNode(CustomSerializationInfo info)
    {
        switch (info.Kind)
        {
            case CustomSerializerKind.ConstantSize:
                string sizeExpression = $"{info.Name}.{info.SizeMemberName}";
                string serializationMethodOpen = $"{info.Name}.{info.SerializationMethodName}(";
                string deserializationMethodOpen = $"{info.Name}.{info.DeserializationMethodName}(";
                CreateGetSerializedSizeExpression = _ => sizeExpression;
                CreateGetSerializationStatement = (bufferWriterExpression, valueExpression)
                    => $"{serializationMethodOpen}{bufferWriterExpression}, {valueExpression}{StatementClose}";
                CreateGetDeserializationExpression = bufferReaderExpression
                    => $"{deserializationMethodOpen}{bufferReaderExpression}{StatementClose}";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}