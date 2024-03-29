﻿#region

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using MsbRpc.Generator.Enums;
using MsbRpc.Generator.Extensions;

#endregion

namespace MsbRpc.Generator.Info;

internal static class ConstantSizeSerializationInfoParser
{
    private static readonly HashSet<string> ValidSerializedSizeTypes = new()
    {
        "Byte",
        "SByte",
        "Int16",
        "UInt16",
        "Int32",
        "UInt32"
        // don't want to give the illusion that serialized size can be >= 2^31
        // "System.Int64",
        // "System.UInt64",
    };

    /// <param name="serializerTypeSymbol">type that is attributed with "ConstantSizeSerializerAttribute"</param>
    /// <param name="attribute">attribute data of type "ConstantSizeSerializerAttribute"</param>
    public static CustomSerializationInfoWithTargetType? Parse(INamedTypeSymbol serializerTypeSymbol, AttributeData attribute)
    {
        string? serializationMethodName = null;
        string? deserializationMethodName = null;
        string? sizeMemberName = null;
        bool areMethodsResolved = false;

        IEnumerable<KeyValuePair<string, TypedConstant>> attributeArguments = attribute.GetArguments();
        TypedConstant typeArgument = attributeArguments.First(argument => argument.Key == "type").Value;

        if (typeArgument.Value is not INamedTypeSymbol targetTypeSymbol)
        {
            return null;
        }

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        // foreach is more readable to me here
        foreach (ISymbol member in serializerTypeSymbol.GetMembers())
        {
            Accessibility? accessibility = member.DeclaredAccessibility;
            if (accessibility != Accessibility.Public && accessibility != Accessibility.Internal)
            {
                continue;
            }

            if (!areMethodsResolved && member is IMethodSymbol { IsStatic: true } staticMethod)
            {
                if (serializationMethodName == null && staticMethod.IsSerializationMethod(targetTypeSymbol))
                {
                    serializationMethodName = staticMethod.Name;
                    areMethodsResolved = deserializationMethodName != null;
                }
                else if (deserializationMethodName == null && staticMethod.IsDeserializationMethod(targetTypeSymbol))
                {
                    deserializationMethodName = staticMethod.Name;
                    areMethodsResolved = serializationMethodName != null;
                }
            }
            else if (sizeMemberName == null && member is IFieldSymbol { IsConst: true } constField)
            {
                if (constField.IsSerializedSizeField())
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
            new TypeReferenceInfo(serializerTypeSymbol),
            new TypeReferenceInfo(targetTypeSymbol),
            CustomSerializerKind.ConstantSize,
            serializationMethodName!,
            deserializationMethodName!,
            sizeMemberName
        );
    }

    private static bool IsSerializationMethod(this IMethodSymbol accessibleStaticSerializerMethod, ISymbol targetType)
    {
        if (!accessibleStaticSerializerMethod.ReturnsVoid)
        {
            return false;
        }

        ImmutableArray<IParameterSymbol> parameters = accessibleStaticSerializerMethod.Parameters;
        if (parameters.Length != 2)
        {
            return false;
        }

        if (!accessibleStaticSerializerMethod.HasAttribute(NamedTypeSymbolExtensions.GetIsSerializationMethodAttribute))
        {
            return false;
        }

        IParameterSymbol bufferWriterParameter = parameters[0];
        IParameterSymbol valueParameter = parameters[1];

        return bufferWriterParameter.Type is INamedTypeSymbol bufferWriterParameterType
               && bufferWriterParameterType.GetIsBufferWriter()
               && valueParameter.Type.Equals(targetType, SymbolEqualityComparer.IncludeNullability);
    }

    private static bool IsDeserializationMethod(this IMethodSymbol accessibleStaticSerializerMethod, ISymbol targetType)
    {
        ImmutableArray<IParameterSymbol> parameters = accessibleStaticSerializerMethod.Parameters;

        if (parameters.Length != 1)
        {
            return false;
        }

        if (!accessibleStaticSerializerMethod.HasAttribute(NamedTypeSymbolExtensions.GetIsDeserializationMethodAttribute))
        {
            return false;
        }

        if (!accessibleStaticSerializerMethod.ReturnType.Equals(targetType, SymbolEqualityComparer.IncludeNullability))
        {
            return false;
        }

        IParameterSymbol bufferReaderParameter = parameters[0];

        return bufferReaderParameter.Type is INamedTypeSymbol bufferReaderParameterType
               && bufferReaderParameterType.GetIsBufferReader();
    }

    private static bool IsSerializedSizeField(this IFieldSymbol accessibleConstField)
        => accessibleConstField.HasAttribute(NamedTypeSymbolExtensions.GetIsSerializedSizeAttribute)
           && GetIsValidSizeType(accessibleConstField.Type);

    private static bool GetIsValidSizeType(ITypeSymbol type)
    {
        ITypeSymbol originalDefinition = type.OriginalDefinition;
        string originalDefinitionName = originalDefinition.Name;
        return originalDefinition.IsValueType
               && originalDefinition.ContainingNamespace.GetIsSystem()
               && IsValidSerializedSizeTypeName(originalDefinitionName);
    }

    private static bool IsValidSerializedSizeTypeName(string typeName) => ValidSerializedSizeTypes.Contains(typeName);
}