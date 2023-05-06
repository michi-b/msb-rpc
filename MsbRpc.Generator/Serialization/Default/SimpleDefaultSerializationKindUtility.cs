using System.Collections.Generic;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization.Default;

public static class SimpleDefaultSerializationKindUtility
{
    public const int DictionaryCapacity = 100;

    private static readonly ReadOnlyDictionary<TypeDeclarationInfo, SimpleDefaultSerializationKind> Values;

    public static readonly SimpleDefaultSerializationKind[] All =
    {
        SimpleDefaultSerializationKind.Byte,
        SimpleDefaultSerializationKind.Sbyte,
        SimpleDefaultSerializationKind.Bool,
        SimpleDefaultSerializationKind.Char,
        SimpleDefaultSerializationKind.Int,
        SimpleDefaultSerializationKind.Long,
        SimpleDefaultSerializationKind.Short,
        SimpleDefaultSerializationKind.Uint,
        SimpleDefaultSerializationKind.Ulong,
        SimpleDefaultSerializationKind.Ushort,
        SimpleDefaultSerializationKind.Float,
        SimpleDefaultSerializationKind.Double,
        SimpleDefaultSerializationKind.Decimal,
        SimpleDefaultSerializationKind.String
    };

    static SimpleDefaultSerializationKindUtility()
    {
        Dictionary<TypeDeclarationInfo, SimpleDefaultSerializationKind> values = new(DictionaryCapacity);
        foreach (SimpleDefaultSerializationKind serializationKind in All)
        {
            values.Add(serializationKind.GetTargetType().Declaration, serializationKind);
        }

        Values = new ReadOnlyDictionary<TypeDeclarationInfo, SimpleDefaultSerializationKind>(values);
    }
}