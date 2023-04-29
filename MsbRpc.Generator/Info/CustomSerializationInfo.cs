using System;

namespace MsbRpc.Generator.Info;

public readonly struct CustomSerializationInfo : IEquatable<CustomSerializationInfo>
{
    /// <summary>
    ///     fully qualified type name
    /// </summary>
    public readonly string Name;

    public readonly CustomSerializerKind Kind;
    public readonly string SerializationMethodName;
    public readonly string DeserializationMethodName;
    public readonly string SizeMemberName;

    public CustomSerializationInfo(CustomSerializationInfoWithTargetType info)
    {
        Name = info.Name;
        Kind = info.Kind;
        SerializationMethodName = info.SerializationMethodName;
        DeserializationMethodName = info.DeserializationMethodName;
        SizeMemberName = info.SizeMemberName;
    }

    public bool Equals(CustomSerializationInfo other)
        => Name == other.Name
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
            hashCode = (hashCode * 397) ^ (int)Kind;
            hashCode = (hashCode * 397) ^ SerializationMethodName.GetHashCode();
            hashCode = (hashCode * 397) ^ DeserializationMethodName.GetHashCode();
            hashCode = (hashCode * 397) ^ SizeMemberName.GetHashCode();
            return hashCode;
        }
    }
}