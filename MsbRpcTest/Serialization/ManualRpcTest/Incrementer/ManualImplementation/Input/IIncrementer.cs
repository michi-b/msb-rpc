using MsbRpc.GeneratorAttributes;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.ManualImplementation.Input;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}