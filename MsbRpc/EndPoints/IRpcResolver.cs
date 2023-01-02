using System;
using MsbRpc.Serialization.Buffers;

namespace MsbRpc.EndPoints;

public interface IRpcResolver<in TProcedure> where TProcedure : Enum
{
    Message Execute(TProcedure procedure, BufferReader reader);
}