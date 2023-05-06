using System;
using MsbRpc.Generator.CodeWriters.Utility;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization.Default.Generic;

public static class GenericDefaultSerializationKindExtensions
{
    private static readonly TypeDeclarationInfo NullableTypeDeclarationInfo = new(IndependentNames.Types.Nullable, 1);

    private static readonly IGenericSerializationFactory NullableSerializationFactory = new NullableSerialization.Factory();

    public static TypeDeclarationInfo GetTypeDeclarationInfo(this GenericDefaultSerializationKind target)
        => target switch
        {
            GenericDefaultSerializationKind.Nullable => NullableTypeDeclarationInfo,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };

    public static IGenericSerializationFactory GetSerializationFactory(this GenericDefaultSerializationKind target)
        => target switch
        {
            GenericDefaultSerializationKind.Nullable => NullableSerializationFactory,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
        };
}