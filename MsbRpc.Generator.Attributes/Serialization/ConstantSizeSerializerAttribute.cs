using System;
using JetBrains.Annotations;

namespace MsbRpc.Generator.Attributes.Serialization;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ConstantSizeSerializerAttribute : Attribute
{
    public ConstantSizeSerializerAttribute(Type type) => Type = type;
    
    [PublicAPI]
    public Type Type { get; }
}