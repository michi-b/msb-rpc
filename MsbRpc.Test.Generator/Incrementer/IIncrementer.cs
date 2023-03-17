using MsbRpc.Contracts;
using MsbRpc.Generator.Attributes;

// ReSharper disable CheckNamespace
namespace Incrementer;

[GenerateServer]
[RpcContract(RpcContractType.ClientToServer)]
public interface IIncrementer : IRpcContract
{
    int Increment(int value);
    public string? IncrementString(string? value);
    public void Store(int value);
    public void IncrementStored();
    public int GetStored();
    public void Finish();
}