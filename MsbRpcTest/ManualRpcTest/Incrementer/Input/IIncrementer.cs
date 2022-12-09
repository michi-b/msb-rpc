using MsbRpc.Generator.Attributes;

namespace MsbRpcTest.ManualRpcTest.Incrementer.Input;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}