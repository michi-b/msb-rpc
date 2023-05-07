using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization.Serializations;

public class CustomSerialization : ISerialization
{
    public static readonly KeyValuePair<TypeReferenceInfo, CustomSerializationInfo>[] EmptyArray
        = Array.Empty<KeyValuePair<TypeReferenceInfo, CustomSerializationInfo>>();

    private readonly string _declarationSyntax;
    private readonly string _deserializationMethod;
    private readonly string _serializationMethod;
    private readonly string _sizeMember;

    public CustomSerialization(TypeReferenceInfo targetType, CustomSerializationInfo serializationInfo)
    {
        _declarationSyntax = targetType.GetDeclarationSyntax();
        string serializerName = serializationInfo.SerializerTypeReference.Declaration.Name;
        _sizeMember = serializerName + "." + serializationInfo.SizeMemberName;
        _serializationMethod = serializerName + "." + serializationInfo.SerializationMethodName;
        _deserializationMethod = serializerName + "." + serializationInfo.DeserializationMethodName;
    }

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression) { }

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression) { }

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression) { }

    public bool GetIsVoid() => false;

    public virtual bool GetIsResolved() => true;

    public string GetDeclarationSyntax() => _declarationSyntax;
}