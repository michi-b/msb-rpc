using MsbRpc.Generator.Attributes;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Input;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}