using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Generator.Incrementer.Input;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}