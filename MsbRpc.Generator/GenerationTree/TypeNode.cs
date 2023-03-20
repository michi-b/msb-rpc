using System.IO;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    private readonly string? _constantSizeExpression;
    private readonly string _fullName;
    private readonly string _name;
    public readonly string DeclarationSyntax;
    public readonly bool IsNullable;
    public readonly bool IsValidParameter;
    public readonly bool IsValidReturnType;
    public readonly bool IsVoid;

    public readonly SerializationKind SerializationKind;

    public bool IsConstantSize => _constantSizeExpression != null;

    public TypeNode(string fullName, SerializationKind serializationKind, bool isNullable)
    {
        SerializationKind = serializationKind;

        IsNullable = isNullable;
        _fullName = fullName;

        if (serializationKind == SerializationKind.Unresolved)
        {
            DeclarationSyntax = isNullable ? fullName + '?' : fullName;
            _name = _fullName;
            _constantSizeExpression = null;
            IsValidParameter = false;
            IsValidReturnType = false;
            IsVoid = false;
        }
        else
        {
            string? keyword = serializationKind.GetKeyword();
            _name = keyword ?? _fullName;
            DeclarationSyntax = keyword != null
                ? isNullable
                    ? keyword + '?'
                    : keyword
                : isNullable
                    ? fullName + '?'
                    : fullName;
            _constantSizeExpression = SerializationKind.GetConstantSizeExpression();
            IsValidParameter = serializationKind.GetIsValidParameterType();
            IsValidReturnType = serializationKind.GetIsValidReturnType();
            IsVoid = serializationKind == SerializationKind.Void;
        }
    }

    public bool WriteSizeVariableInitialization(TextWriter writer, string sizeVariableName, string targetExpression)
    {
        string? sizeExpression = GetSizeExpression(targetExpression);
        
        if (sizeExpression == null)
        {
            return false;
        }
        
        if (_constantSizeExpression != null)
        {
            writer.Write("const ");
        }
        writer.WriteLine($"int {sizeVariableName} = {sizeExpression};");
        return true;
    }

    public string GetResponseReadStatement() => $"{_name} {Variables.Result} = {GetBufferReadExpression(Variables.ResponseReader)};";

    public string GetBufferReadExpression(string bufferReaderExpression)
    {
        string bufferReadMethodName = SerializationKind.GetBufferReadMethodName(IsNullable) ?? string.Empty;
        return $"{bufferReaderExpression}.{bufferReadMethodName}()";
    }

    public override string ToString() => $"{_fullName} ({SerializationKind.GetName()})";

    private string? GetSizeExpression(string targetExpression)
    {
        if (_constantSizeExpression != null)
        {
            return _constantSizeExpression;
        }

        if (SerializationKind == SerializationKind.String)
        {
            string serializer = IsNullable ? Types.NullableStringSerializer : Types.StringSerializer;
            return $"{serializer}.GetSize({targetExpression})";
        }

        return null;
    }
}