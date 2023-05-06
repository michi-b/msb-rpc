using System.Collections.Generic;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization.Default.Generic;

public static class GenericDefaultSerializationKindUtility
{
    public const int DictionaryCapacity = 100;
    
    public const string NullableTypeName = "System.Nullable";
    
    private static readonly ReadOnlyDictionary<TypeDeclarationInfo, GenericDefaultSerializationKind> Values;
    
    public static readonly GenericDefaultSerializationKind[] All =
    {
        GenericDefaultSerializationKind.Nullable,
    };
    
    static GenericDefaultSerializationKindUtility()
    {
        Dictionary<TypeDeclarationInfo, GenericDefaultSerializationKind> values = new(DictionaryCapacity);
        
        foreach (GenericDefaultSerializationKind serializationKind in All)
        {
            values.Add(serializationKind.GetTypeDeclarationInfo(), serializationKind);
        }

        Values = new ReadOnlyDictionary<TypeDeclarationInfo, GenericDefaultSerializationKind>(values);
    }
}