using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Integration.Incrementer;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}