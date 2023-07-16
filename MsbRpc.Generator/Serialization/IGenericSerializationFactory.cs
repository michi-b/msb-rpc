#region

using MsbRpc.Generator.Info;

#endregion

namespace MsbRpc.Generator.Serialization;

public interface IGenericSerializationFactory
{
    public ISerialization Create(TypeReferenceInfo typeReference, SerializationResolver serializationResolver);
}