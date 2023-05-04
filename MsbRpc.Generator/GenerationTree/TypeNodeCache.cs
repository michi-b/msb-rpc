﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree;

internal class TypeNodeCache
{
    private readonly Dictionary<TypeInfo, TypeNode> _cache = new();

    private readonly IReadOnlyDictionary<TypeInfo, CustomSerializationNode> _customSerializations;

    public TypeNodeCache(ImmutableDictionary<TypeInfo, CustomSerializationInfo> customSerializationInfos)
    {
        Dictionary<TypeInfo, CustomSerializationNode> customSerializations = customSerializationInfos
            .ToDictionary(pair => pair.Key, pair => new CustomSerializationNode(pair.Value));
        _customSerializations = new ReadOnlyDictionary<TypeInfo, CustomSerializationNode>(customSerializations);
    }

    public TypeNode GetOrAdd(TypeInfo typeInfo)
    {
        if (_cache.TryGetValue(typeInfo, out TypeNode? node))
        {
            return node;
        }

        node = new TypeNode(typeInfo, _customSerializations);

        _cache.Add(typeInfo, node);

        return node;
    }
}