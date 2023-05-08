using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization.Serializations;

public sealed class CustomSerialization : ISerialization
{
    public static readonly KeyValuePair<TypeReferenceInfo, CustomSerializationInfo>[] EmptyArray
        = Array.Empty<KeyValuePair<TypeReferenceInfo, CustomSerializationInfo>>();

    private readonly string _declarationSyntax;
    private readonly string _deserializationMethod;
    private readonly bool _isConstantSize;
    private readonly string _serializationMethod;
    private readonly string _sizeMember;

    public CustomSerialization(TypeReferenceInfo targetType, CustomSerializationInfo serializationInfo)
    {
        _declarationSyntax = targetType.GetDeclarationSyntax();
        string serializerName = serializationInfo.SerializerTypeReference.Declaration.Name;
        _isConstantSize = serializationInfo.Kind == CustomSerializerKind.ConstantSize;
        _sizeMember = serializerName + "." + serializationInfo.SizeMemberName;
        _serializationMethod = serializerName + "." + serializationInfo.SerializationMethodName;
        _deserializationMethod = serializerName + "." + serializationInfo.DeserializationMethodName;
    }

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        writer.Write(_isConstantSize ? _sizeMember : $"{_sizeMember}({targetExpression})");
    }

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        writer.Write($"{_serializationMethod}({bufferWriterExpression}, {valueExpression})");
    }

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.Write($"{_deserializationMethod}({bufferReaderExpression})");
    }

    public bool IsVoid => false;

    public bool IsResolved => true;
    public bool IsConstantSize => _isConstantSize;

    public string DeclarationSyntax => _declarationSyntax;
}