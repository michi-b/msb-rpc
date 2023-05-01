using System.Collections.Generic;

namespace MsbRpc.Generator.Serialization;

public class Resolver
{
    private static IReadOnlyDictionary<string, Serialization> _defaultSerializations;
    private Dictionary<string, Serialization> _serializations = new();

    static Resolver()
    {
        Dictionary<string, Serialization> defaultSerializations = new Dictionary<string, Serialization>(100);
        
    }
}