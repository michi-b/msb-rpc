using MsbRpc.Generator.Info;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    public readonly string? ConstantSizeExpression;
    public readonly string FullName;
    public readonly bool IsConstantSize;
    public readonly bool IsValidParameter;
    public readonly bool IsValidReturnType;
    public readonly bool IsVoid;
    public readonly string Name;

    public readonly SerializationKind SerializationKind;

    public TypeNode(string fullName, SerializationKind serializationKind)
    {
        SerializationKind = serializationKind;

        if (serializationKind == SerializationKind.Unresolved)
        {
            FullName = fullName;
            Name = fullName;
            ConstantSizeExpression = null;
            IsConstantSize = false;
            IsValidParameter = false;
            IsValidReturnType = false;
            IsVoid = false;
        }
        else
        {
            FullName = fullName;
            string? keyword = serializationKind.GetKeyword();
            Name = keyword ?? FullName;
            ConstantSizeExpression = SerializationKind.GetConstantSizeExpression();
            IsConstantSize = ConstantSizeExpression != null;
            IsValidParameter = serializationKind.GetIsValidParameterType();
            IsValidReturnType = serializationKind.GetIsValidReturnType();
            IsVoid = serializationKind == SerializationKind.Void;
        }
    }

    public string GetResponseReadStatement() => $"{Name} {Variables.Result} = {GetBufferReadExpression(Variables.ResponseReader)};";

    public string GetBufferReadExpression(string bufferReaderExpression)
    {
        string? bufferReadMethodName = SerializationKind.GetBufferReadMethodName() ?? string.Empty;
        return $"{bufferReaderExpression}.{bufferReadMethodName}()";
    }

    public override string ToString() => $"{FullName} ({SerializationKind.GetName()})";
}