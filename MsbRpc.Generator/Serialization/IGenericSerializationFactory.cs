using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.Serialization;

public interface IGenericSerializationFactory
{
    public ISerialization Create(TypeReferenceInfo typeReference, SerializationResolver serializationResolver);
}