using System;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class CustomSerializationNode
{
    private const string StatementClose = ");";

    public Serialization.GetSizeExpressionDelegate GetSizeExpression { get; }

    public Serialization.GetSerializationStatementDelegate GetSerializationStatement { get; }

    public Serialization.GetDeserializationExpressionDelegate GetDeserializationExpression { get; }

    public CustomSerializationNode(CustomSerializationInfo info)
    {
        switch (info.Kind)
        {
            case CustomSerializerKind.ConstantSize:
                string sizeExpression = $"{info.SerializerType.Name}.{info.SizeMemberName}";
                string serializationMethodOpen = $"{info.SerializerType.Name}.{info.SerializationMethodName}(";
                string deserializationMethodOpen = $"{info.SerializerType.Name}.{info.DeserializationMethodName}(";
                GetSizeExpression = _ => sizeExpression;
                GetSerializationStatement = (bufferWriterExpression, valueExpression)
                    => $"{serializationMethodOpen}{bufferWriterExpression}, {valueExpression}{StatementClose}";
                GetDeserializationExpression = bufferReaderExpression
                    => $"{deserializationMethodOpen}{bufferReaderExpression}{StatementClose}";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}