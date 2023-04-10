using System.IO;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    private readonly string? _constantSizeExpression;
    private readonly string _fullName;
    private readonly bool _isNullable;
    public readonly string DeclarationSyntax;
    public readonly bool IsValidParameter;
    public readonly bool IsValidReturnType;
    public readonly bool IsVoid;
    public readonly SerializationKind SerializationKind;

    public TypeNode(string fullName, SerializationKind serializationKind, bool isNullable)
    {
        SerializationKind = serializationKind;

        _isNullable = isNullable;
        _fullName = fullName;

        if (serializationKind == SerializationKind.Unresolved)
        {
            DeclarationSyntax = isNullable ? fullName + '?' : fullName;
            _constantSizeExpression = null;
            IsValidParameter = false;
            IsValidReturnType = false;
            IsVoid = false;
        }
        else
        {
            string? keyword = serializationKind.GetKeyword();
            DeclarationSyntax = keyword != null
                ? isNullable
                    ? keyword + '?'
                    : keyword
                : isNullable
                    ? fullName + '?'
                    : fullName;

            _constantSizeExpression = SerializationKind.GetConstantSizeExpression(isNullable);
            IsValidParameter = serializationKind.GetIsBufferReadAndWritable();
            IsValidReturnType = serializationKind.GetIsValidReturnType();
            IsVoid = serializationKind == SerializationKind.Void;
        }
    }

    public void WriteSizeVariableInitialization(TextWriter writer, string sizeVariableName, string sizeExpression)
    {
        if (_constantSizeExpression != null)
        {
            writer.Write("const ");
        }

        writer.WriteLine($"int {sizeVariableName} = {sizeExpression};");
    }

    public string GetResponseReadStatement() => $"{DeclarationSyntax} {Variables.Result} = {GetBufferReadExpression(Variables.ResponseReader)};";

    public string GetBufferReadExpression(string bufferReaderExpression)
    {
        string bufferReaderReadMethod = SerializationKind.GetBufferReaderReadMethodName(_isNullable) ?? string.Empty;
        return $"{bufferReaderExpression}.{bufferReaderReadMethod}()";
    }

    public override string ToString() => $"{_fullName} ({SerializationKind.GetName()})";

    public string GetBufferWriterWriteStatement(string bufferWriterExpression, string variableName)
    {
        string bufferWriterWriteMethod = SerializationKind.GetBufferWriterWriteMethodName(_isNullable) ?? string.Empty;
        return $"{bufferWriterExpression}.{bufferWriterWriteMethod}({variableName});";
    }

    public string? GetSizeExpression(string targetExpression)
    {
        if (_constantSizeExpression != null)
        {
            return _constantSizeExpression;
        }

        if (SerializationKind == SerializationKind.String)
        {
            string serializer = _isNullable ? Types.NullableStringSerializer : Types.StringSerializer;
            return $"{serializer}.GetSize({targetExpression})";
        }

        return null;
    }
}