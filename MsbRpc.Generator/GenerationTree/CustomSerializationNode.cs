﻿using System;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class CustomSerializationNode
{
    private const string StatementClose = ");";

    public SerializationNode.GetSizeExpressionDelegate GetSizeExpression { get; }

    public SerializationNode.GetSerializationStatementDelegate GetSerializationStatement { get; }

    public SerializationNode.GetDeserializationExpressionDelegate GetDeserializationExpression { get; }

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