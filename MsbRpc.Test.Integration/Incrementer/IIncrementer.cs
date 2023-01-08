using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Integration.Incrementer;

[RpcContract(RpcContractType.ClientToServerRoot)]
public interface IIncrementer
{
    int Increment(int value);
    public void Store(int value);
    public void IncrementStored();
    public int GetStored();
    public void Finish();
}