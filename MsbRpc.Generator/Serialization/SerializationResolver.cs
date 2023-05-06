using System.Collections.Generic;
using System.Collections.Immutable;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization.Default;
using MsbRpc.Generator.Serialization.Serializations;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.Serialization;

using SerializationRegistry = Dictionary<TypeReferenceInfo, ISerialization>;
using GenericSerializationFactoryRegistry = Dictionary<TypeDeclarationInfo, IGenericSerializationFactory>;

public class SerializationResolver
{
    private static readonly SerializationRegistry DefaultSerializations;

    private static readonly GenericSerializationFactoryRegistry DefaultGenericSerializationsFactories;

    private readonly GenericSerializationFactoryRegistry _genericSerializationsFactories;

    private readonly SerializationRegistry _serializations;

    static SerializationResolver()
    {
        DefaultSerializations = new SerializationRegistry(SimpleDefaultSerializationKindUtility.DictionaryCapacity);

        DefaultSerializations.Add(TypeReferenceInfo.CreateSimple(Types.Void), new VoidSerialization());

        foreach (SimpleDefaultSerializationKind serializationKind in SimpleDefaultSerializationKindUtility.All)
        {
            DefaultSerializations.Add(serializationKind.GetTargetType(), new SimpleDefaultSerialization(serializationKind));
        }

        DefaultGenericSerializationsFactories = new GenericSerializationFactoryRegistry(GenericDefaultSerializationKindUtility.DictionaryCapacity);

        foreach (GenericDefaultSerializationKind serializationKind in GenericDefaultSerializationKindUtility.All)
        {
            DefaultGenericSerializationsFactories.Add(serializationKind.GetTypeDeclarationInfo(), serializationKind.GetSerializationFactory());
        }
    }

    public SerializationResolver(ImmutableArray<CustomSerializationInfo> customSerializations)
    {
        _serializations = new SerializationRegistry(DefaultSerializations);
        _genericSerializationsFactories = new GenericSerializationFactoryRegistry(DefaultGenericSerializationsFactories);
    }

    // _genericSerializationWriterFactories = new SerializationFactoryRegistry();
    public ISerialization Resolve(TypeReferenceInfo typeReference)
    {
        if (_serializations.TryGetValue(typeReference, out ISerialization existingSerialization))
        {
            return existingSerialization;
        }

        if (typeReference.TypeArguments.Count > 0)
        {
            if (_genericSerializationsFactories.TryGetValue(typeReference.Declaration, out IGenericSerializationFactory serializationFactory))
            {
                ISerialization newSerializationInstance = serializationFactory.Create(typeReference, this);
                _serializations.Add(typeReference, newSerializationInstance);
                return newSerializationInstance;
            }
        }

        //return unresolved serialization, if it could not be resolved
        ISerialization unresolvedSerialization = new UnresolvedSerialization(typeReference);
        _serializations.Add(typeReference, unresolvedSerialization);
        return unresolvedSerialization;
    }
}