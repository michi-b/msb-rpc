using System;

namespace MsbRpc.Generator.Info;

public readonly struct CustomSerializationInfoWithTargetType : IEquatable<CustomSerializationInfoWithTargetType>
{
    /// <summary>
    ///     fully qualified type name
    /// </summary>
    public readonly string Name;
    public readonly TypeInfo TargetType;
    public readonly CustomSerializerKind Kind;
    public readonly string SerializationMethodName;
    public readonly string DeserializationMethodName;
    public readonly string SizeMemberName;

    public CustomSerializationInfoWithTargetType
    (
        string name,
        TypeInfo targetType,
        CustomSerializerKind kind,
        string serializationMethodName,
        string deserializationMethodName,
        string sizeMemberName
    )
    {
        Name = name;
        TargetType = targetType;
        Kind = kind;
        SerializationMethodName = serializationMethodName;
        DeserializationMethodName = deserializationMethodName;
        SizeMemberName = sizeMemberName;
    }

    public bool Equals(CustomSerializationInfoWithTargetType other)
        => Name == other.Name
           && TargetType.Equals(other.TargetType)
           && Kind == other.Kind
           && SerializationMethodName == other.SerializationMethodName
           && DeserializationMethodName == other.DeserializationMethodName
           && SizeMemberName == other.SizeMemberName;

    public override bool Equals(object? obj) => obj is CustomSerializationInfoWithTargetType other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ TargetType.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Kind;
            hashCode = (hashCode * 397) ^ SerializationMethodName.GetHashCode();
            hashCode = (hashCode * 397) ^ DeserializationMethodName.GetHashCode();
            hashCode = (hashCode * 397) ^ SizeMemberName.GetHashCode();
            return hashCode;
        }
    }
}