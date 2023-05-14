using System.Collections.Generic;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Serialization.Default;
using MsbRpc.Generator.Serialization.Serializations;
using MsbRpc.Generator.Serialization.Serializations.Decorators;
using static MsbRpc.Generator.Utility.IndependentNames;

namespace MsbRpc.Generator.Serialization;

using SerializationRegistry = Dictionary<TypeReferenceInfo, ISerialization>;
using GenericSerializationFactoryRegistry = Dictionary<TypeDeclarationInfo, IGenericSerializationFactory>;

public class SerializationResolver
{
    private static readonly SerializationRegistry DefaultSerializations;

    private static readonly GenericSerializationFactoryRegistry DefaultGenericSerializationsFactories;

    private static readonly SimpleDefaultSerialization[] _SimpleDefaultSerializations;

    private readonly GenericSerializationFactoryRegistry _genericSerializationsFactories;

    private readonly SerializationRegistry _serializations;

    static SerializationResolver()
    {
        DefaultSerializations = new SerializationRegistry(SimpleDefaultSerializationKindUtility.DictionaryCapacity);

        DefaultSerializations.Add(TypeReferenceInfo.CreateSimple(Types.Void), new VoidSerialization());

        const int simpleDefaultSerializationsCount = SimpleDefaultSerializationKindUtility.Count;
        _SimpleDefaultSerializations = new SimpleDefaultSerialization[simpleDefaultSerializationsCount];
        for (int i = 0; i < simpleDefaultSerializationsCount; i++)
        {
            SimpleDefaultSerializationKind serializationKind = SimpleDefaultSerializationKindUtility.All[i];
            SimpleDefaultSerialization serialization = new(serializationKind);
            _SimpleDefaultSerializations[i] = serialization;
            DefaultSerializations.Add(serializationKind.GetTargetType(), serialization);
        }

        DefaultGenericSerializationsFactories = new GenericSerializationFactoryRegistry(GenericDefaultSerializationKindUtility.DictionaryCapacity);

        foreach (GenericDefaultSerializationKind serializationKind in GenericDefaultSerializationKindUtility.All)
        {
            DefaultGenericSerializationsFactories.Add(serializationKind.GetTypeDeclarationInfo(), serializationKind.GetSerializationFactory());
        }
    }

    public SerializationResolver()
        : this(CustomSerialization.EmptyArray) { }

    public SerializationResolver(KeyValuePair<TypeReferenceInfo, CustomSerializationInfo>[] customSerializations)
    {
        _serializations = new SerializationRegistry(DefaultSerializations);

        foreach (KeyValuePair<TypeReferenceInfo, CustomSerializationInfo> customSerialization in customSerializations)
        {
            _serializations.Add(customSerialization.Key, new CustomSerialization(customSerialization.Key, customSerialization.Value));
        }

        _genericSerializationsFactories = new GenericSerializationFactoryRegistry(DefaultGenericSerializationsFactories);
    }

    public static SimpleDefaultSerialization GetSimpleDefaultSerialization(SimpleDefaultSerializationKind kind) => _SimpleDefaultSerializations[(int)kind];

    public static SimpleDefaultSerialization GetEnumUnderlyingTypeSerialization
        (EnumSerializationKind kind)
        => GetSimpleDefaultSerialization(kind.GetSimpleDefaultSerializationKind());

    // _genericSerializationWriterFactories = new SerializationFactoryRegistry();
    public ISerialization Resolve(TypeReferenceInfo type)
    {
        if (_serializations.TryGetValue(type, out ISerialization existingSerialization))
        {
            return existingSerialization;
        }

        //try instantiate nullable reference serialization
        if (type.IsNullableReference)
        {
            TypeReferenceInfo nonNullableType = type.MakeNullable(false);
            ISerialization nonNullableSerialization = Resolve(nonNullableType);
            ISerialization nullableSerialization = new NullableReferenceSerialization(nonNullableSerialization);
            _serializations.Add(type, nullableSerialization);
            return nullableSerialization;
        }

        TypeDeclarationInfo typeDeclaration = type.Declaration;

        //try instantiate enum serialization
        if (type.Declaration.EnumSerializationKind is { } enumSerializationKind)
        {
            EnumSerialization enumSerialization = new(typeDeclaration, enumSerializationKind);
            _serializations.Add(type, enumSerialization);
            return enumSerialization;
        }

        //try instantiate generic serialization
        if (type.TypeArguments.Count > 0)
        {
            if (_genericSerializationsFactories.TryGetValue(typeDeclaration, out IGenericSerializationFactory serializationFactory))
            {
                ISerialization newSerializationInstance = serializationFactory.Create(type, this);
                _serializations.Add(type, newSerializationInstance);
                return newSerializationInstance;
            }
        }

        //return unresolved serialization, if it could not be resolved
        ISerialization unresolvedSerialization = new UnresolvedSerialization(type);
        _serializations.Add(type, unresolvedSerialization);
        return unresolvedSerialization;
    }
}