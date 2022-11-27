using MsbRpc.GeneratorAttributes;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Input;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}