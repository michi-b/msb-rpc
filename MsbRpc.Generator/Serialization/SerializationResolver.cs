using System.Collections.Generic;
using System.Collections.ObjectModel;
using MsbRpc.Generator.Info;
using MsbRpc.Generator.Utility;

namespace MsbRpc.Generator.Serialization;

public class SerializationResolver
{
    private static ReadOnlyDictionary<string, SerializationWriter> _defaultSerializations;
    private Dictionary<string, GenerationTree.Serialization> _serializations = new();

    static SerializationResolver()
    {
        var defaultSerializations = new Dictionary<string, SerializationWriter>(100);

        foreach (DefaultSerializationKind serializationKind in DefaultSerializationKindUtility.All)
        {
            defaultSerializations.Add(serializationKind.GetName(), new SerializationWriter(serializationKind));
        }
        _defaultSerializations = new ReadOnlyDictionary<string, SerializationWriter>(defaultSerializations);
    }
}