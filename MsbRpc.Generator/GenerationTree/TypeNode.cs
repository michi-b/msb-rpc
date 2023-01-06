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

    public readonly string? ReadFromRequestReaderExpression;
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

        if (IsValidParameter)
        {
            string? bufferReadMethodName = SerializationKind.GetBufferReadMethodName();
            ReadFromRequestReaderExpression = bufferReadMethodName != null
                ? $"{Variables.RequestReader}.{bufferReadMethodName}()"
                : "NotYetImplemented";
        }
        else
        {
            ReadFromRequestReaderExpression = null;
        }
    }

    public override string ToString() => $"{FullName} ({SerializationKind.GetName()})";
}