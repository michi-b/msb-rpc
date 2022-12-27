using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.ManualRpcTest.Incrementer.Input;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}