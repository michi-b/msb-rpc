using MsbRpc.GeneratorAttributes;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Implementation.Input;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}