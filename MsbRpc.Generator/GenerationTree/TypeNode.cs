using MsbRpc.Generator.GenerationTree.Names;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNode
{
    public readonly string? ConstantSizeExpression;

    public readonly bool IsConstantSize;

    public readonly bool IsValidParameter;

    public readonly bool IsValidReturnType;

    public readonly bool IsVoid;

    public readonly TypeNames Names;

    public readonly SerializationKind SerializationKind;

    public TypeNode(string fullName, SerializationKind serializationKind)
    {
        SerializationKind = serializationKind;

        if (serializationKind == SerializationKind.Unresolved)
        {
            Names = TypeNames.CreateInvalid(fullName);
            ConstantSizeExpression = null;
            IsConstantSize = false;
            IsValidParameter = false;
            IsValidReturnType = false;
            IsVoid = false;
        }
        else
        {
            Names = new TypeNames(fullName, SerializationKind);
            ConstantSizeExpression = SerializationKind.GetConstantSizeExpression();
            IsConstantSize = ConstantSizeExpression != null;
            IsValidParameter = serializationKind.GetIsValidParameterType();
            IsValidReturnType = serializationKind.GetIsValidReturnType();
            IsVoid = serializationKind == SerializationKind.Void;
        }
    }
}