using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization.Serializations;

public sealed class CustomSerialization : ISerialization
{
    public static readonly KeyValuePair<TypeReferenceInfo, CustomSerializationInfo>[] EmptyArray
        = Array.Empty<KeyValuePair<TypeReferenceInfo, CustomSerializationInfo>>();

    private readonly string _deserializationMethod;
    private readonly string _serializationMethod;
    private readonly string _sizeMember;

    public bool IsVoid => false;

    public bool IsResolved => true;
    public bool IsConstantSize { get; }

    public string DeclarationSyntax { get; }

    public bool NeedsSemicolonAfterSerializationStatement => true;

    public CustomSerialization(TypeReferenceInfo targetType, CustomSerializationInfo serializationInfo)
    {
        DeclarationSyntax = targetType.GetDeclarationSyntax();
        TypeReferenceInfo serializerType = serializationInfo.SerializerTypeReference;
        IsConstantSize = serializationInfo.Kind == CustomSerializerKind.ConstantSize;

        if (serializerType.NamedDeclaration is { } namedDeclaration)
        {
            string serializerName = namedDeclaration.Name;
            _sizeMember = serializerName + "." + serializationInfo.SizeMemberName;
            _serializationMethod = serializerName + "." + serializationInfo.SerializationMethodName;
            _deserializationMethod = serializerName + "." + serializationInfo.DeserializationMethodName;
        }
        else
        {
            throw new InvalidOperationException("serializer type must be a named type");
        }
    }

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write(IsConstantSize ? _sizeMember : $"{_sizeMember}({targetExpression})");
    }

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        writer.Write($"{_serializationMethod}({bufferWriterExpression}, {valueExpression})");
    }

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.Write($"{_deserializationMethod}({bufferReaderExpression})");
    }
}