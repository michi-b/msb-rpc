using System.CodeDom.Compiler;
using System.Diagnostics;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization.Serializations;

namespace MsbRpc.Generator.Serialization.Default;

public class EnumSerialization : ISerialization
{
    private readonly SimpleDefaultSerialization _underlyingTypeSerialization;
    public bool IsVoid => false;
    public bool IsResolved => true;
    public bool IsConstantSize => true;
    public string DeclarationSyntax { get; }
    public bool NeedsSemicolonAfterSerializationStatement => true;

    public EnumSerialization(TypeDeclarationInfo enumDeclaration, EnumSerializationKind enumSerializationKind)
    {
        Debug.Assert(enumDeclaration.EnumSerializationKind == enumSerializationKind);
        _underlyingTypeSerialization = SerializationResolver.GetEnumUnderlyingTypeSerialization(enumSerializationKind);
        DeclarationSyntax = enumDeclaration.Name;
    }

    public void WriteSizeExpression(IndentedTextWriter writer, string targetExpression)
    {
        _underlyingTypeSerialization.WriteSizeExpression(writer, targetExpression);
    }

    public void WriteSerializationStatement(IndentedTextWriter writer, string bufferWriterExpression, string valueExpression)
    {
        string castedValueExpression = $"({_underlyingTypeSerialization.DeclarationSyntax}){valueExpression}";
        _underlyingTypeSerialization.WriteSerializationStatement(writer, bufferWriterExpression, castedValueExpression);
    }

    public void WriteDeserializationExpression(IndentedTextWriter writer, string bufferReaderExpression)
    {
        writer.Write($"({DeclarationSyntax})(");
        _underlyingTypeSerialization.WriteDeserializationExpression(writer, bufferReaderExpression);
        writer.Write(")");
    }
}