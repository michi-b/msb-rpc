using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

public interface IRpcResolver<in TProcedure> where TProcedure : Enum
{
    BufferWriter Resolve(TProcedure procedure, BufferReader reader);
}