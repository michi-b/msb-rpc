using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Serialization;

public class SerializationResolver
{
    private static readonly ReadOnlyDictionary<string, SerializationWriter> DefaultSerializations;
    private readonly Dictionary<string, SerializationWriter> _serializationWriters;
    private readonly Dictionary<string, GenericSerializationWriterFactory> _genericSerializationWriterFactories;
    private readonly Dictionary<TypeInfo, SerializationWriter> _genericSerializationWriters;

    static SerializationResolver()
    {
        var defaultSerializations = new Dictionary<string, SerializationWriter>(100);

        foreach (DefaultSerializationKind serializationKind in DefaultSerializationKindUtility.All)
        {
            defaultSerializations.Add(serializationKind.GetName(), new SerializationWriter(serializationKind));
        }

        DefaultSerializations = new ReadOnlyDictionary<string, SerializationWriter>(defaultSerializations);
    }

    public SerializationResolver
        (ImmutableArray<CustomSerializationInfo> customSerializations)
    {
        _serializationWriters = new Dictionary<string, SerializationWriter>(DefaultSerializations);
        _genericSerializationWriterFactories = new Dictionary<string, GenericSerializationWriterFactory>();
        _genericSerializationWriters = new Dictionary<TypeInfo, SerializationWriter>();
    }

    public bool TryGetSerializationWriter(TypeInfo typeInfo, out SerializationWriter serializationWriter)
    {
        if(typeInfo.TypeArguments.Count > 0)
        {
            if(_genericSerializationWriters.TryGetValue(typeInfo, out serializationWriter))
            {
                return true;
            }
            
            if (_genericSerializationWriterFactories.TryGetValue(typeInfo.Name, out GenericSerializationWriterFactory genericSerializationWriterFactory))
            {
                if (genericSerializationWriterFactory.TryInstantiate(this, typeInfo.TypeArguments, out serializationWriter))
                {
                    _genericSerializationWriters.Add(typeInfo, serializationWriter);
                    return true;
                }
            }

            return false;
        }
        return _serializationWriters.TryGetValue(typeInfo.Name, out serializationWriter);
    }

    public SerializationWriter GetSerializationWriter(DefaultSerializationKind defaultSerializationKind) => _serializationWriters[defaultSerializationKind.GetName()];

    public bool TryGetGenericSerializationWriterFactory(string name, out GenericSerializationWriterFactory resGenericSerializationWriterFactory) 
        => _genericSerializationWriterFactories.TryGetValue(name, out resGenericSerializationWriterFactory);
}