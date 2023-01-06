using MsbRpc.Contracts;
using MsbRpc.Generator.Attributes;

// ReSharper disable CheckNamespace
namespace Incrementer;

[RpcContract(RpcContractType.Server)]
public interface IIncrementer : IRpcContract
{
    int Increment(int value);
    public void Store(int value);
    public void IncrementStored();
    public int GetStored();
    public void Finish();
}