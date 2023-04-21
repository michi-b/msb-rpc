using System;
using System.IO;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    private const string ZeroSizeExpression = "0";
    private readonly string _fullName;
    private readonly bool _isNullable;
    public readonly string DeclarationSyntax;
    public readonly bool IsConstantSize;
    public readonly bool IsValidParameter;
    public readonly bool IsValidReturnType;
    public readonly SerializationKind SerializationKind;

    public TypeNode(string fullName, SerializationKind serializationKind, bool isNullable)
    {
        SerializationKind = serializationKind;

        _isNullable = isNullable;
        _fullName = fullName;

        if (serializationKind == SerializationKind.Unresolved)
        {
            DeclarationSyntax = isNullable ? fullName + '?' : fullName;
            IsValidParameter = false;
            IsValidReturnType = false;
            IsConstantSize = true;
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
            IsValidParameter = serializationKind.GetIsBufferReadAndWritable();
            IsValidReturnType = serializationKind.GetIsValidReturnType();
            IsConstantSize = serializationKind.GetIsConstantSize();
        }
    }

    public void WriteTargetSizeVariableInitialization(TextWriter writer, string sizeVariableName, string target)
    {
        WriteSizeVariableInitialization(writer, sizeVariableName, GetSizeExpression(target));
    }

    public void WriteSizeVariableInitialization(TextWriter writer, string sizeVariableName, string sizeExpression)
    {
        writer.WriteLine
        (
            IsConstantSize
                ? $"const int {sizeVariableName} = {sizeExpression};"
                : $"int {sizeVariableName} = {sizeExpression};"
        );
    }

    public string GetResponseReadStatement() => $"{DeclarationSyntax} {Variables.Result} = {GetBufferReadExpression(Variables.ResponseReader)};";

    public string GetBufferReadExpression(string bufferReaderExpression)
    {
        string? bufferReaderReadMethod = SerializationKind.GetBufferReaderReadMethodName(_isNullable);
        return bufferReaderReadMethod != null
            ? $"{bufferReaderExpression}.{bufferReaderReadMethod}()"
            : "default!";
    }

    public override string ToString() => $"{_fullName} ({SerializationKind.GetName()})";

    public string GetBufferWriterWriteStatement(string bufferWriterExpression, string variableName)
    {
        string bufferWriterWriteMethod = SerializationKind.GetBufferWriterWriteMethodName(_isNullable) ?? string.Empty;
        return $"{bufferWriterExpression}.{bufferWriterWriteMethod}({variableName});";
    }

    public string GetSizeExpression(string targetExpression)
    {
        return SerializationKind switch
        {
            SerializationKind.Unresolved => ZeroSizeExpression,
            SerializationKind.Byte => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Sbyte => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Bool => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Char => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Int => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Long => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Short => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Uint => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Ulong => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Ushort => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Float => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Double => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.Decimal => SerializationKind.GetPrimitiveSizeExpression(_isNullable),
            SerializationKind.String => $"{(_isNullable ? Types.NullableStringSerializer : Types.StringSerializer)}.GetSize({targetExpression})",
            SerializationKind.Void => ZeroSizeExpression,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}