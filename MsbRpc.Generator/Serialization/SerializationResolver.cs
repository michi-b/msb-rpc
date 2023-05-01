using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Serialization;

public class SerializationResolver
{
    private static readonly ReadOnlyDictionary<string, SerializationWriter> DefaultSerializations;
    private readonly Dictionary<string, SerializationWriter> _serializations;

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
        => _serializations = new Dictionary<string, SerializationWriter>(DefaultSerializations);

    public SerializationWriter GetSerializationWriter(DefaultSerializationKind defaultSerializationKind) => _serializations[defaultSerializationKind.GetName()];

    public bool TryGetSerializationWriter(string name, out SerializationWriter serializationWriter) => _serializations.TryGetValue(name, out serializationWriter);
}