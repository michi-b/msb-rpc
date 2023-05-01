using System;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization;

namespace MsbRpc.Generator.GenerationTree;

internal class CustomSerializationNode
{
    private const string StatementClose = ");";

    public Serialization.Serialization.GetSizeExpressionDelegate GetSizeExpression { get; }

    public Serialization.Serialization.GetSerializationStatementDelegate GetSerializationStatement { get; }

    public Serialization.Serialization.GetDeserializationExpressionDelegate GetDeserializationExpression { get; }

    public CustomSerializationNode(CustomSerializationInfo info)
    {
        switch (info.Kind)
        {
            case CustomSerializerKind.ConstantSize:
                string sizeExpression = $"{info.Name}.{info.SizeMemberName}";
                string serializationMethodOpen = $"{info.Name}.{info.SerializationMethodName}(";
                string deserializationMethodOpen = $"{info.Name}.{info.DeserializationMethodName}(";
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