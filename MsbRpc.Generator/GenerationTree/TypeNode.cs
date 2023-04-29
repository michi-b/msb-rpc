using System;
using System.Collections.Generic;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    public readonly string DeclarationSyntax;
    public readonly bool IsNullable;
    public readonly bool IsVoid;
    public readonly SerializationNode? Serialization;
    public bool IsResolved => IsVoid || Serialization is not null;

    public TypeNode(TypeInfo typeInfo, IReadOnlyDictionary<string, CustomSerializationNode> customSerializations)
    {
        IsNullable = typeInfo.IsNullable;

        DefaultSerializationKind serializationKind = DefaultSerializationKindUtility.GetDefaultSerializationKind(typeInfo.Name);

        if (serializationKind == DefaultSerializationKind.Unresolved)
        {
            DeclarationSyntax = IsNullable ? typeInfo.Name + '?' : typeInfo.Name;
            IsVoid = false;
        }
        else
        {
            DeclarationSyntax = serializationKind.TryGetKeyword(out string keyword)
                ? IsNullable ? keyword + '?' : keyword
                : IsNullable
                    ? typeInfo.Name + '?'
                    : typeInfo.Name;
            switch (serializationKind)
            {
                case DefaultSerializationKind.Void:
                    IsVoid = true;
                    break;
                case DefaultSerializationKind.Byte:
                case DefaultSerializationKind.Sbyte:
                case DefaultSerializationKind.Bool:
                case DefaultSerializationKind.Char:
                case DefaultSerializationKind.Int:
                case DefaultSerializationKind.Long:
                case DefaultSerializationKind.Short:
                case DefaultSerializationKind.Uint:
                case DefaultSerializationKind.Ulong:
                case DefaultSerializationKind.Ushort:
                case DefaultSerializationKind.Float:
                case DefaultSerializationKind.Double:
                case DefaultSerializationKind.Decimal:
                case DefaultSerializationKind.String:
                    Serialization = new SerializationNode(serializationKind);
                    break;
                case DefaultSerializationKind.Unresolved:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

// public void WriteTargetSizeVariableInitialization(TextWriter writer, string sizeVariableName, string target)
// {
//     WriteSizeVariableInitialization(writer, sizeVariableName, GetSizeExpression(target));
// }
//
// public void WriteSizeVariableInitialization(TextWriter writer, string sizeVariableName, string sizeExpression)
// {
//     writer.WriteLine
//     (
//         IsConstantSize
//             ? $"const int {sizeVariableName} = {sizeExpression};"
//             : $"int {sizeVariableName} = {sizeExpression};"
//     );
// }
//
// public string GetResponseReadStatement() => $"{DeclarationSyntax} {Variables.Result} = {GetBufferReadExpression(Variables.ResponseReader)};";
//
// public string GetBufferReadExpression(string bufferReaderExpression)
// {
//     string? bufferReaderReadMethod = DefaultSerializationKind.GetBufferReaderReadMethodName(_isNullable);
//     return bufferReaderReadMethod != null
//         ? $"{bufferReaderExpression}.{bufferReaderReadMethod}()"
//         : "default!";
// }
//
// public override string ToString() => $"{_fullName} ({DefaultSerializationKind.GetName()})";
//
// public string GetBufferWriterWriteStatement(string bufferWriterExpression, string variableName)
// {
//     string bufferWriterWriteMethod = DefaultSerializationKind.GetBufferWriterWriteMethodName(_isNullable) ?? string.Empty;
//     return $"{bufferWriterExpression}.{bufferWriterWriteMethod}({variableName});";
// }