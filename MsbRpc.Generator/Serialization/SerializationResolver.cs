using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Serialization;

public class SerializationResolver
{
    private static readonly ReadOnlyDictionary<TypeInfo, SerializationWriter> DefaultSerializations;
    private readonly Dictionary<TypeInfo, GenericSerializationWriterFactory> _genericSerializationWriterFactories;
    private readonly Dictionary<TypeInfo, SerializationWriter> _genericSerializationWriters;
    private readonly Dictionary<TypeInfo, SerializationWriter> _serializationWriters;

    static SerializationResolver()
    {
        var defaultSerializations = new Dictionary<TypeInfo, SerializationWriter>(100);

        foreach (SimpleDefaultSerializationKind serializationKind in SimpleDefaultSerializationKindUtility.All)
        {
            defaultSerializations.Add(serializationKind.GetTypeInfo(), new SerializationWriter(serializationKind));
        }

        DefaultSerializations = new ReadOnlyDictionary<TypeInfo, SerializationWriter>(defaultSerializations);
    }

    public SerializationResolver(ImmutableArray<CustomSerializationInfo> customSerializations)
    {
        _serializationWriters = new Dictionary<TypeInfo, SerializationWriter>(DefaultSerializations);
        _genericSerializationWriterFactories = new Dictionary<TypeInfo, GenericSerializationWriterFactory>();
        _genericSerializationWriters = new Dictionary<TypeInfo, SerializationWriter>();
    }

    public bool TryGetSerializationWriter(TypeInfo typeInfo, out SerializationWriter serializationWriter)
    {
        if (typeInfo.TypeArguments.Count > 0)
        {
            if (_genericSerializationWriters.TryGetValue(typeInfo, out serializationWriter))
            {
                return true;
            }

            if (_genericSerializationWriterFactories.TryGetValue(typeInfo, out GenericSerializationWriterFactory genericSerializationWriterFactory))
            {
                if (genericSerializationWriterFactory.TryInstantiate(this, typeInfo.TypeArguments, out serializationWriter))
                {
                    _genericSerializationWriters.Add(typeInfo, serializationWriter);
                    return true;
                }
            }

            return false;
        }

        return _serializationWriters.TryGetValue(typeInfo, out serializationWriter);
    }

    public SerializationWriter GetSerializationWriter(SimpleDefaultSerializationKind simpleDefaultSerializationKind)
        => _serializationWriters[simpleDefaultSerializationKind.GetTypeInfo()];

    public bool TryGetGenericSerializationWriterFactory(TypeInfo typeInfo, out GenericSerializationWriterFactory resGenericSerializationWriterFactory)
        => _genericSerializationWriterFactories.TryGetValue(typeInfo, out resGenericSerializationWriterFactory);
}