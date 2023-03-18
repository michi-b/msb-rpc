using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    private readonly string _fullName;
    private readonly string _name;

    public readonly string? ConstantSizeExpression;
    public readonly string DeclarationSyntax;
    public readonly bool IsConstantSize;
    public readonly bool IsNullable;
    public readonly bool IsValidParameter;
    public readonly bool IsValidReturnType;
    public readonly bool IsVoid;

    public readonly SerializationKind SerializationKind;

    public TypeNode(string fullName, SerializationKind serializationKind, bool isNullable)
    {
        SerializationKind = serializationKind;

        IsNullable = isNullable;
        _fullName = fullName;

        if (serializationKind == SerializationKind.Unresolved)
        {
            DeclarationSyntax = isNullable ? fullName + '?' : fullName;
            _name = _fullName;
            ConstantSizeExpression = null;
            IsConstantSize = false;
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
            ConstantSizeExpression = SerializationKind.GetConstantSizeExpression();
            IsConstantSize = ConstantSizeExpression != null;
            IsValidParameter = serializationKind.GetIsValidParameterType();
            IsValidReturnType = serializationKind.GetIsValidReturnType();
            IsVoid = serializationKind == SerializationKind.Void;
        }
    }

    public string GetResponseReadStatement() => $"{_name} {Variables.Result} = {GetBufferReadExpression(Variables.ResponseReader)};";

    public string GetBufferReadExpression(string bufferReaderExpression)
    {
        string bufferReadMethodName = SerializationKind.GetBufferReadMethodName(IsNullable) ?? string.Empty;
        return $"{bufferReaderExpression}.{bufferReadMethodName}()";
    }

    public override string ToString() => $"{_fullName} ({SerializationKind.GetName()})";
}