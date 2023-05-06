using System.Collections.Generic;
using System.Collections.Immutable;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization.Default;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Serialization;

using SerializationRegistry = Dictionary<TypeReferenceInfo, ISerialization>;

// using SerializationFactoryRegistry = Dictionary<TypeDeclarationInfo, SerializationFactory>;

public class SerializationResolver
{
    private static readonly SerializationRegistry DefaultSerializations;

    // private static readonly SerializationFactoryRegistry DefaultGenericSerializations;
    // private readonly SerializationFactoryRegistry _genericSerializationWriterFactories;
    private readonly SerializationRegistry _serializations;

    static SerializationResolver()
    {
        DefaultSerializations = new SerializationRegistry(SimpleDefaultSerializationKindUtility.DictionaryCapacity);

        DefaultSerializations.Add(TypeReferenceInfo.CreateSimple(Types.Void), new VoidSerialization());
        
        foreach (SimpleDefaultSerializationKind serializationKind in SimpleDefaultSerializationKindUtility.All)
        {
            DefaultSerializations.Add(serializationKind.GetTargetType(), new SimpleDefaultSerialization(serializationKind));
        }

        // DefaultGenericSerializations = new SerializationFactoryRegistry(GenericDefaultSerializationKindUtility.DictionaryCapacity);
        //
        // foreach (GenericDefaultSerializationKind serializationKind in GenericDefaultSerializationKindUtility.All)
        // {
        //     DefaultGenericSerializations.Add(serializationKind.GetTypeDeclarationInfo(), new SerializationFactory(serializationKind));
        // }
    }

    public SerializationResolver(ImmutableArray<CustomSerializationInfo> customSerializations) => _serializations = new SerializationRegistry(DefaultSerializations);

    // _genericSerializationWriterFactories = new SerializationFactoryRegistry();
    public ISerialization Resolve(TypeReferenceInfo typeReference)
    {
        if (_serializations.TryGetValue(typeReference, out ISerialization existingSerialization))
        {
            return existingSerialization;
        }

        //todo: re-introduce generic serialization
        // if (typeReference.TypeArguments.Count > 0)
        // {
        //     if (_genericSerializationWriterFactories.TryGetValue(typeReference.Declaration, out SerializationFactory genericSerializationWriterFactory))
        //     {
        //         if (genericSerializationWriterFactory.TryInstantiate(this, typeReference.TypeArguments, out ISerialization newSerialization))
        //         {
        //             _serializationWriters.Add(typeReference, newSerialization);
        //             return newSerialization;
        //         }
        //     }
        // }

        //return unresolved serialization, if it could not be resolved
        ISerialization unresolvedSerialization = new UnresolvedSerialization(typeReference);
        _serializations.Add(typeReference, unresolvedSerialization);
        return unresolvedSerialization;
    }
}