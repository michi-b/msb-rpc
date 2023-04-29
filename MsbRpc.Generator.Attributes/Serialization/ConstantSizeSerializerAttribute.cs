using System;
using JetBrains.Annotations;

namespace MsbRpc.Generator.Attributes.Serialization;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class ConstantSizeSerializerAttribute : Attribute
{
    [PublicAPI] public Type Type { get; }

    public ConstantSizeSerializerAttribute(Type type) => Type = type;
}