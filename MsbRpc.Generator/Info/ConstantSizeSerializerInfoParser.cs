using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Extensions;

namespace MsbRpc.Generator.Info;

internal static class ConstantSizeSerializerInfoParser
{
    public static CustomSerializationInfoWithTargetType? Parse(INamedTypeSymbol serializerType, AttributeData attribute)
    {
        string name = serializerType.GetFullName();
        // string targetType =
        string? serializationMethodName = null;
        string? deserializationMethodName = null;
        string? sizeMemberName = null;
        bool areMethodsResolved = false;

        foreach (ISymbol member in serializerType.GetMembers())
        {
            if (!areMethodsResolved && member is IMethodSymbol { IsStatic: true } staticMethod)
            {
                if (serializationMethodName == null && staticMethod.IsSerializationMethod())
                {
                    serializationMethodName = staticMethod.Name;
                    areMethodsResolved = deserializationMethodName != null;
                }
                else if (deserializationMethodName == null && staticMethod.IsDeserializationMethod())
                {
                    deserializationMethodName = staticMethod.Name;
                    areMethodsResolved = serializationMethodName != null;
                }
            }
            else if (sizeMemberName == null && member is IFieldSymbol { IsConst: true } constField)
            {
                if (constField.IsSizeField())
                {
                    sizeMemberName = constField.Name;
                }
            }
        }

        if (!areMethodsResolved || sizeMemberName == null)
        {
            return null;
        }

        return new CustomSerializationInfoWithTargetType
        (
            name,
            "todo",
            CustomSerializerKind.ConstantSize,
            serializationMethodName!,
            deserializationMethodName!,
            sizeMemberName
        );
    }

    private static bool IsSerializationMethod(this IMethodSymbol staticSerializerMethod)
        =>
            //todo: implement
            false;

    private static bool IsDeserializationMethod(this IMethodSymbol staticSerializerMethod)
        =>
            //todo: implement
            false;

    private static bool IsSizeField(this IFieldSymbol constField)
        =>
            //todo: implement
            false;
}