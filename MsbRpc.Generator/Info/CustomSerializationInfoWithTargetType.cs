#region

using System;
using MsbRpc.Generator.Enums;

#endregion

namespace MsbRpc.Generator.Info;

public readonly struct CustomSerializationInfoWithTargetType : IEquatable<CustomSerializationInfoWithTargetType>
{
    /// <summary>
    ///     fully qualified type name
    /// </summary>
    public readonly TypeReferenceInfo SerializerTypeReference;

    public readonly TypeReferenceInfo TargetTypeReference;
    public readonly CustomSerializerKind Kind;
    public readonly string SerializationMethodName;
    public readonly string DeserializationMethodName;
    public readonly string SizeMemberName;

    public CustomSerializationInfoWithTargetType
    (
        TypeReferenceInfo serializerTypeReference,
        TypeReferenceInfo targetTypeReference,
        CustomSerializerKind kind,
        string serializationMethodName,
        string deserializationMethodName,
        string sizeMemberName
    )
    {
        SerializerTypeReference = serializerTypeReference;
        TargetTypeReference = targetTypeReference;
        Kind = kind;
        SerializationMethodName = serializationMethodName;
        DeserializationMethodName = deserializationMethodName;
        SizeMemberName = sizeMemberName;
    }

    public bool Equals(CustomSerializationInfoWithTargetType other)
        => SerializerTypeReference.Equals(other.SerializerTypeReference)
           && TargetTypeReference.Equals(other.TargetTypeReference)
           && Kind == other.Kind
           && SerializationMethodName == other.SerializationMethodName
           && DeserializationMethodName == other.DeserializationMethodName
           && SizeMemberName == other.SizeMemberName;

    public override bool Equals(object? obj) => obj is CustomSerializationInfoWithTargetType other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = SerializerTypeReference.GetHashCode();
            hashCode = (hashCode * 397) ^ TargetTypeReference.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Kind;
            hashCode = (hashCode * 397) ^ SerializationMethodName.GetHashCode();
            hashCode = (hashCode * 397) ^ DeserializationMethodName.GetHashCode();
            hashCode = (hashCode * 397) ^ SizeMemberName.GetHashCode();
            return hashCode;
        }
    }
}