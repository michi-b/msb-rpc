using System;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization.Serializations;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Serialization.Default;

public static class GenericDefaultSerializationKindExtensions
{
    private static readonly NamedTypeDeclarationInfo NullableNamedTypeDeclarationInfo = new(IndependentNames.Types.Nullable, 1);

    private static readonly IGenericSerializationFactory NullableSerializationFactory = new NullableSerialization.Factory();

    public static NamedTypeDeclarationInfo GetTypeDeclarationInfo(this GenericDefaultSerializationKind target)
        => target switch
        {
            GenericDefaultSerializationKind.Nullable => NullableNamedTypeDeclarationInfo,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    public static IGenericSerializationFactory GetSerializationFactory(this GenericDefaultSerializationKind target)
        => target switch
        {
            GenericDefaultSerializationKind.Nullable => NullableSerializationFactory,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
}